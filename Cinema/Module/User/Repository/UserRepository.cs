using Cinema.Data;
using Cinema.Model;

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
            UserModel userModel = _context.Users.Update(user).Entity;
            _context.SaveChanges();
            return userModel;
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
