using AutoMapper;
using Cinema.Model;
using Cinema.Module.Account.DTO;
using Cinema.Module.Account.Register;
using Cinema.Module.Account.Repository;
using Cinema.Module.Role.Service;
using Cinema.Module.User.DTO;
using Cinema.Module.User.Service;

namespace Cinema.Module.Account.Service
{
    public class AccountService : IAccountService
    {

        private readonly IAccountRepository _accountRepository;

        private readonly IRoleService _roleService;

        private readonly IMapper _mapper;

        public AccountService(IAccountRepository accountRepository, IRoleService roleService, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _roleService = roleService;
            _mapper = mapper;
        }

        public AccountDTO AddAccount(AccountDTO account)
        {
            AccountModel accountModel = _mapper.Map<AccountModel>(account);
            accountModel.RoleId = _roleService.getRoleByName(account.RoleName).Id;
            accountModel.accountStatus = Enum.AccountStatus.ACTIVATED;
            return _mapper.Map<AccountDTO>(_accountRepository.AddAccount(accountModel));
        }

        public void DeleteAccount(int id)
        {
            AccountModel accountModel = _accountRepository.GetAccount(id);
            accountModel.accountStatus = Enum.AccountStatus.DELETED;
            _accountRepository.UpdateAccount(accountModel);
        }

        public AccountDTO GetAccount(string username)
        {
            AccountModel accountModel = _accountRepository.GetAccount(username);
            AccountDTO accountDTO = _mapper.Map<AccountDTO>(accountModel);
            accountDTO.RoleName = accountModel.Role.Name;
            return accountDTO;
        }

        public AccountDTO GetAccount(int id)
        {
            AccountModel accountModel = _accountRepository.GetAccount(id);
            AccountDTO accountDTO = _mapper.Map<AccountDTO>(accountModel);
            accountDTO.RoleName = accountModel.Role.Name;
            return accountDTO;
        }

        public List<AccountDTO> GetAllAccounts()
        {
            return _accountRepository.GetAllAccounts().Select(account =>
            {
                AccountDTO accountDTO = _mapper.Map<AccountDTO>(account);
                accountDTO.RoleName = account.Role.Name;
                return accountDTO;
            }).ToList();
        }

        public AccountDTO Login(LoginData loginData)
        {
            AccountModel account = _accountRepository.Login(loginData);
            AccountDTO accountDTO = _mapper.Map<AccountDTO>(account);
            accountDTO.RoleName = account.Role.Name;
            return accountDTO;
        }

        public AccountDTO UpdateAccount(AccountDTO account)
        {
            return _mapper.Map<AccountDTO>(_accountRepository
                .UpdateAccount(_mapper.Map<AccountModel>(account)));
        }
    }
}
