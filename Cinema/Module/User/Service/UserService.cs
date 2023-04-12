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

        public UserDTO AddUser(UserDTO userDTO)
        {
            UserModel userModel =  _userRepository.AddUser(_mapper.Map<UserModel>(userDTO));
            UserDTO result = _mapper.Map<UserDTO>(userModel);
            result.Email = userDTO.Email;
            return result;

        }

        public UserDTO UpdateUser(UserDTO userDTO)
        {
            return _mapper.Map<UserDTO>(_userRepository.UpdateUser(_mapper.Map<UserModel>(userDTO)));
        }

        public void DeleteUser(int id)
        {
            _userRepository.DeleteUser(id);
        }

        public List<UserDTO> GetAllUsers()
        {
            List<UserDTO> userDTOs = _userRepository.getAllUsers()
                .Select(user => {
                    UserDTO userDTO = _mapper.Map<UserDTO>(user);
                    userDTO.Email = _accountService.GetAccount(user.AccountId).Email;
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
            return userDTO;
        }

        public UserDTO GetUserByEmail(string email)
        {
            UserDTO userDTO = _mapper.Map<UserDTO>(_userRepository.GetUserByEmail(email));
            userDTO.Email = email;
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
            UserDTO userDTO = AddUser(new UserDTO
            {
                Name = registerData.Name,
                Email = account.Email,
                Gender = registerData.Gender,
                Birth = registerData.Birth.Date,
                Phone = registerData.Phone,
                Address = registerData.Address,
                AccountId = account.Id,
            });
            return userDTO;
        }
    }
}
