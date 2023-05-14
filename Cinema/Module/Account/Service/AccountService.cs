using AutoMapper;
using Cinema.Model;
using Cinema.Module.Account.DTO;
using Cinema.Module.Account.Register;
using Cinema.Module.Account.Repository;
using Cinema.Module.Role.Service;
using Cinema.Module.User.DTO;
using Cinema.Module.User.Service;
using System.Transactions;

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
            accountModel.AccountStatus = Enum.AccountStatus.ACTIVATED;
            return _mapper.Map<AccountDTO>(_accountRepository.AddAccount(accountModel));
        }

        public void ChangePassword(string oldPassword, string newPassword, int id)
        {
            AccountModel accountModel = _accountRepository.GetAccount(id);
            if (accountModel.Password != oldPassword) throw new InvalidOperationException("Wrong password");
            _accountRepository.ChangePassword(newPassword, id);
        }

        public void ResetPassword(string newPassword, string email)
        {
            _accountRepository.ChangePassword(newPassword, GetAccount(email).Id);
        }

        public void DeleteAccount(int id)
        {
            AccountModel accountModel = _accountRepository.GetAccount(id);
            accountModel.AccountStatus = Enum.AccountStatus.DELETED;
            _accountRepository.UpdateAccount(accountModel);
        }

        public AccountDTO GetAccount(string email)
        {
            AccountModel accountModel = _accountRepository.GetAccount(email);
            if (accountModel == null) return null;
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
            AccountModel accountModel = _mapper.Map<AccountModel>(account);
            accountModel.RoleId = _roleService.getRoleByName(account.RoleName).Id;
            return _mapper.Map<AccountDTO>(_accountRepository.UpdateAccount(accountModel));
        }
    }
}
