using Cinema.Data;
using Cinema.Model;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Module.User.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public UserModel AddUser(UserModel user)
        {
            UserModel userModel = _context.Users.Add(user).Entity;
            _context.SaveChanges();
            return userModel;
        }

        public UserModel UpdateUser(UserModel user)
        {
            UserModel oldUser = _context.Users.Include(p => p.Account).FirstOrDefault(p => p.Id == user.Id);
            if(oldUser != null)
            {
                oldUser.Name = user.Name;
                oldUser.Gender = user.Gender;
                oldUser.Address = user.Address;
                oldUser.Birth = user.Birth;
                oldUser.Phone = user.Phone;
                _context.Users.Update(oldUser);
                _context.SaveChanges();
            }
            return oldUser;
        }

        public void DeleteUser(int id)
        {
            _context.Remove(_context.Users.Where(p => p.Id == id));
            _context.SaveChanges();
        }

        public List<UserModel> getAllUsers()
        {
            return _context.Users.Select(p => p).ToList();
        }

        public UserModel GetUser(int id)
        {
            return _context.Users.Where(p => p.Id == id).Single();
        }

        public UserModel GetUserByEmail(string email)
        {
            return _context.Users.Where(p => p.Account.Email == email).Single();
        }
    }
}
