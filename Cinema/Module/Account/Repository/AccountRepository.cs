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

        public void DeleteAccount(int id)
        {
            _context.Accounts.Remove(_context.Accounts.Where(p => p.Id == id).Single());
            _context.SaveChanges();
        }

        public AccountModel GetAccount(string email)
        {
            return _context.Accounts.Include(p => p.Role).Where(p => p.Email == email).Single();
        }

        public AccountModel GetAccount(int id)
        {
            return _context.Accounts.Include(p => p.Role).Where(p => p.Id == id).Single();
        }

        public List<AccountModel> GetAllAccounts()
        {
            return _context.Accounts.ToList();
        }

        public AccountModel Login(LoginData loginData)
        {
            try
            {
                return _context.Accounts.Include(p => p.Role).Where(p => p.Email == loginData.Email && p.Password == loginData.Password).Single();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public AccountModel UpdateAccount(AccountModel account)
        {
            AccountModel accountModel = _context.Accounts.Update(account).Entity;
            _context.SaveChanges();
            return accountModel;
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
