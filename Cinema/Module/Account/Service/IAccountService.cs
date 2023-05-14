using Cinema.Model;
using Cinema.Module.Account.DTO;
using Cinema.Module.Account.Register;
using Cinema.Module.User.DTO;
using Microsoft.AspNetCore.Identity;

namespace Cinema.Module.Account.Service
{
    public interface IAccountService
    {
        AccountDTO Login(LoginData loginData);

        AccountDTO GetAccount(string email);

        AccountDTO GetAccount(int id);

        AccountDTO AddAccount(AccountDTO account);

        AccountDTO UpdateAccount(AccountDTO account);

        void DeleteAccount(int id);

        List<AccountDTO> GetAllAccounts();

        void ChangePassword(string oldPassword, string newPassword, int id);

        void ResetPassword(string newPassword, string email);
    }
}
