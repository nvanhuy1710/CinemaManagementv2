﻿using AutoMapper;
using AutoMapper.Internal;
using Cinema.Model;
using Cinema.Module.Account.DTO;
using Cinema.Module.Account.Register;
using Cinema.Module.Account.Repository;
using Cinema.Module.Account.Service;
using Cinema.Module.Role.Repository;
using Cinema.Module.Role.Service;
using Cinema.Module.User.DTO;
using Cinema.Module.User.Repository;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Security.Claims;
using System.Transactions;

namespace Cinema.Module.User.Service
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _userRepository;

        private readonly IAccountService _accountService;

        private readonly IMapper _mapper;

        private static Dictionary<string, string> userPasswordResetCodes = new Dictionary<string, string>();

        public UserService(IUserRepository userRepository, IMapper mapper, IAccountService accountService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _accountService = accountService;
        }

        public UserDTO UpdateUser(UserDTO userDTO)
        {
            AccountDTO account = _accountService.GetAccount(userDTO.Email);
            account.RoleName = userDTO.RoleName;
            _accountService.UpdateAccount(account);
            UserModel newUser = _userRepository.UpdateUser(_mapper.Map<UserModel>(userDTO));   
            UserDTO user = _mapper.Map<UserDTO>(newUser);
            user.RoleName = userDTO.RoleName;
            user.Email = newUser.Account.Email;
            return user;
        }

        public void DeleteUser(int id)
        {
            UserModel userModel = _userRepository.GetUser(id);
            _accountService.DeleteAccount(userModel.AccountId);
        }

        public List<UserDTO> GetAllUsers()
        {
            List<UserDTO> userDTOs = _userRepository.getAllUsers()
                .Select(user => {
                    UserDTO userDTO = _mapper.Map<UserDTO>(user);
                    userDTO.Email = _accountService.GetAccount(user.AccountId).Email;
                    userDTO.RoleName = _accountService.GetAccount(user.AccountId).RoleName;
                    return userDTO;
                    })
                .ToList();
            return userDTOs;
        }

        public UserDTO GetUser(int id)
        {
            UserModel userModel = _userRepository.GetUser(id);
            UserDTO userDTO = _mapper.Map<UserDTO>(userModel);
            userDTO.Email = _accountService.GetAccount(userModel.AccountId).Email;
            userDTO.RoleName = _accountService.GetAccount(userModel.AccountId).RoleName;
            return userDTO;
        }

        public UserDTO GetUserByEmail(string email)
        {
            UserModel userModel = _userRepository.GetUserByEmail(email);
            UserDTO userDTO = _mapper.Map<UserDTO>(userModel);
            userDTO.Email = email;
            userDTO.RoleName = _accountService.GetAccount(userModel.AccountId).RoleName;
            return userDTO;
        }

        public UserDTO Register(RegisterData registerData, bool isStaff = false)
        {
            if(_accountService.GetAccount(registerData.Email) != null)
            {
                return null;
            }
            AccountDTO accountDTO = new AccountDTO
            {
                Email = registerData.Email,
                Password = registerData.Password,
                RoleName = (isStaff ? "STAFF" : "USER"),
            };
            AccountDTO account = _accountService.AddAccount(accountDTO);
            UserModel userModel = new UserModel
            {
                Name = registerData.Name,
                Gender = registerData.Gender,
                Birth = registerData.Birth,
                Phone = registerData.Phone,
                Address = registerData.Address,
                AccountId = account.Id,
            };
            UserDTO userDTO = _mapper.Map<UserDTO>(_userRepository.AddUser(userModel));
            userDTO.RoleName = registerData.Role;
            return userDTO;
        }

        public List<UserDTO> GetStaff()
        {
            List<UserDTO> userDTOs = _userRepository.GetStaff()
                .Select(user => {
                    UserDTO userDTO = _mapper.Map<UserDTO>(user);
                    userDTO.Email = user.Account.Email;
                    userDTO.RoleName = user.Account.Role.Name;
                    return userDTO;
                })
                .ToList();
            return userDTOs;
        }

        public void ChangePassword(PasswordChangeForm form, int userId)
        {
            _accountService.ChangePassword(form.oldPassword, form.newPassword, _userRepository.GetUser(userId).AccountId);
        }

        public string checkToken(string token, string email)
        {
            if (userPasswordResetCodes.ContainsKey(email) && userPasswordResetCodes[email] == token)
            {
                return userPasswordResetCodes[email];
            }
            else
            {
                throw new InvalidOperationException("Invalid token");
            }
        }

        public void ForgotPassword(string email)
        {
            try
            {
                //GetUserByEmail(email);
                string resetCode = GenerateResetCode();
                userPasswordResetCodes[email] = resetCode;

                SendResetCodeEmail(email, resetCode);

            }
            catch (ArgumentNullException)
            {
                throw new InvalidOperationException("User does not exist");
            }
        }

        public void ResetPassword(string token, string email, string password)
        {
            if (userPasswordResetCodes.ContainsKey(email) && userPasswordResetCodes[email] == token)
            {
                userPasswordResetCodes.Remove(email);
                _accountService.ResetPassword(password, email);
            }
            else
            {
                throw new InvalidOperationException("Invalid token");
            }
        }

        private string GenerateResetCode()
        {
            return Guid.NewGuid().ToString("N");
        }

        private void SendResetCodeEmail(string email, string resetCode)
        {
            var message = new MimeMessage();
            message.Sender = MailboxAddress.Parse("nvanhuy13257@gmail.com"); // Địa chỉ email người gửi, có thể là một địa chỉ tùy ý
            message.To.Add(MailboxAddress.Parse("jerry.langosh@ethereal.email")); // Địa chỉ email của người nhận
            message.Subject = "Reset Password Code";

            var builder = new BodyBuilder();
            builder.TextBody = $"Your password reset code is: {resetCode}";

            message.Body = builder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                // Thay đổi thông tin máy chủ SMTP tùy thuộc vào nhà cung cấp email bạn sử dụng
                client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

                // Nếu máy chủ SMTP yêu cầu xác thực, hãy cung cấp thông tin đăng nhập
                client.Authenticate("nvanhuy13257@gmail.com", "-huy17102003");

                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
