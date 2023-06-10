using AutoMapper;
using AutoMapper.Internal;
using Cinema.Helper;
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
using Org.BouncyCastle.Crypto.Macs;
using System;
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

        public void ForgotPassword(string email)
        {
            try
            {
                GetUserByEmail(email);
                string newPassword = GenerateNewPassword();
                string newHashPassword = HashPassword.HashByPBKDF2(newPassword);
                AccountDTO accountDTO = _accountService.GetAccount(email);
                _accountService.ChangePassword(accountDTO.Password, newHashPassword, accountDTO.Id);
                SendNewPassEmail(email, newPassword);

            }
            catch (InvalidOperationException)
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

        private string GenerateNewPassword()
        {
            var random = new Random();
            string newChar = Guid.NewGuid().ToString("N").Substring(2, 8);
            return random.Next(0, 9).ToString() + "a" + newChar;
        }

        private void SendNewPassEmail(string email, string newPassword)
        {
            var message = new MimeMessage();
            message.Sender = MailboxAddress.Parse("nvanhuy1710@gmail.com"); // Địa chỉ email người gửi, có thể là một địa chỉ tùy ý
            message.To.Add(MailboxAddress.Parse(email)); // Địa chỉ email của người nhận
            message.Subject = "New Password";

            var builder = new BodyBuilder();
            string body = $"<html><body><p>Xin chào,</p>" +
                  $"<p>Theo yêu cầu của bạn, rạp phim <strong>LHD</strong> gửi lại cho bạn thông tin mật khẩu mới.</p>" +
                  $"<p>Mật khẩu mới của bạn là: <strong>{newPassword}</strong></p>" +
                  $"<p>Cảm ơn bạn đã sử dụng dịch vụ LHD. Chúc bạn một ngày tốt lành!</p>" +
                  $"<p><br/></p>" +
                  $"<p style=\"margin-top: -10px;\">Trân trọng,</p>" +
                  $"<p style=\"margin-bottom: 0;\">LHD</p></body></html>";
            builder.HtmlBody = body;

            message.Body = builder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                // Thay đổi thông tin máy chủ SMTP tùy thuộc vào nhà cung cấp email bạn sử dụng
                client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

                // Nếu máy chủ SMTP yêu cầu xác thực, hãy cung cấp thông tin đăng nhập
                client.Authenticate("nvanhuy1710@gmail.com", "yqnyoqezzufguskc");

                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
