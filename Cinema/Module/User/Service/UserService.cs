using AutoMapper;
using Cinema.Model;
using Cinema.Module.Account.DTO;
using Cinema.Module.Account.Register;
using Cinema.Module.Account.Repository;
using Cinema.Module.Account.Service;
using Cinema.Module.Role.Repository;
using Cinema.Module.Role.Service;
using Cinema.Module.User.DTO;
using Cinema.Module.User.Repository;
using System.Security.Claims;
using System.Transactions;

namespace Cinema.Module.User.Service
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _userRepository;

        private readonly IAccountService _accountService;

        private readonly IMapper _mapper;

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
            _accountService.DeleteAccount(_accountService.GetAccount(GetUser(id).Email).Id);
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

        public UserDTO Register(RegisterData registerData)
        {
            if(_accountService.GetAccount(registerData.Email) != null)
            {
                return null;
            }
            AccountDTO accountDTO = new AccountDTO
            {
                Email = registerData.Email,
                Password = registerData.Password,
                RoleName = registerData.Role,
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
    }
}
