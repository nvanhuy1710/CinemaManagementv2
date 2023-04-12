using Cinema.Module.Account.Register;
using Cinema.Module.User.DTO;

namespace Cinema.Module.User.Service
{
    public interface IUserService
    {
        List<UserDTO> GetAllUsers();

        UserDTO GetUser(int id);

        UserDTO GetUserByEmail(string username);

        UserDTO AddUser(UserDTO userDTO);

        UserDTO UpdateUser(UserDTO userDTO);

        UserDTO Register(RegisterData registerData);

        void DeleteUser(int id);
    }
}
