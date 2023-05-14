using Cinema.Model;
using Cinema.Module.Account.DTO;

namespace Cinema.Module.Account.Repository
{
    public interface IAccountRepository
    {
        AccountModel Login(LoginData loginData);

        AccountModel GetAccount(string username);

        AccountModel GetAccount(int id);

        AccountModel AddAccount(AccountModel account);

        AccountModel UpdateAccount(AccountModel account);

        void DeleteAccount(int id);

        List<AccountModel> GetAllAccounts();

        void ChangePassword(string password, int id);
    }
}
