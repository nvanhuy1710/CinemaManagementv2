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
            return _mapper.Map<UserDTO>(_userRepository.AddUser(_mapper.Map<UserModel>(userDTO)));
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
            AccountDTO accountDTO = new AccountDTO
            {
                Email = registerData.Email,
                Password = registerData.Password,
                RoleName = registerData.Role,
            };
            List<AccountDTO> accountDTOs = _accountService.GetAllAccounts();
            foreach(AccountDTO dto in accountDTOs)
            {
                if(registerData.Email == dto.Email)
                {
                    return null;
                }
            }
            AccountDTO account = _accountService.AddAccount(accountDTO);
            UserDTO userDTO = AddUser(new UserDTO
            {
                Name = registerData.Name,
                Email = registerData.Email,
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
