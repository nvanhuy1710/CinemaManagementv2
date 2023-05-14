using AutoMapper;
using Cinema.Data;
using Cinema.Model;
using Cinema.Module.Account.DTO;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Module.Account.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DataContext _context;

        public AccountRepository(DataContext context)
        {
            _context = context;
        }

        public AccountModel AddAccount(AccountModel account)
        {
            AccountModel accountModel = _context.Accounts.Add(account).Entity;
            _context.SaveChanges();
            return accountModel;
        }

        public void ChangePassword(string password, int id)
        {
            AccountModel oldAccountModel = _context.Accounts.Where(p => p.Id == id).Single();
            oldAccountModel.Password = password;
            _context.Accounts.Update(oldAccountModel);
            _context.SaveChanges();
        }

        public void DeleteAccount(int id)
        {
            _context.Accounts.Remove(_context.Accounts.Where(p => p.Id == id).Single());
            _context.SaveChanges();
        }

        public AccountModel GetAccount(string email)
        {
            try
            {
                return _context.Accounts.Include(p => p.Role).Where(p => p.Email == email).Single();
            }catch(Exception ex)
            {
                return null;
            }
        }

        public AccountModel GetAccount(int id)
        {
            return _context.Accounts.Include(p => p.Role).Where(p => p.Id == id).Single();
        }

        public List<AccountModel> GetAllAccounts()
        {
            return _context.Accounts.Include(p => p.Role).ToList();
        }

        public AccountModel Login(LoginData loginData)
        {
            AccountModel accountModel =  _context.Accounts.Include(p => p.Role).Where(p => p.Email == loginData.Email && p.Password == loginData.Password).Single();
            return accountModel;
        }

        public AccountModel UpdateAccount(AccountModel account)
        {
            AccountModel oldAccountModel = _context.Accounts.Where(p => p.Email == account.Email).Single();
            oldAccountModel.RoleId = account.RoleId;
            oldAccountModel.AccountStatus = account.AccountStatus;
            _context.Accounts.Update(oldAccountModel);
            _context.SaveChanges();
            return oldAccountModel;
        }

        public bool validate(AccountModel account)
        {
            List<AccountModel> accountModels = _context.Accounts.ToList();
            foreach (var accountModel in accountModels)
            {
                if (account.Email == accountModel.Email || account.Password == accountModel.Password)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
