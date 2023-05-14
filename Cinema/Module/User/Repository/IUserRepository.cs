using Cinema.Model;

namespace Cinema.Module.User.Repository
{
    public interface IUserRepository
    {
        List<UserModel> getAllUsers();

        UserModel GetUser(int id);

        UserModel AddUser(UserModel user);

        UserModel UpdateUser(UserModel user);

        List<UserModel> GetStaff();

        void DeleteUser(int id);

        UserModel GetUserByEmail(string email);
    }
}
