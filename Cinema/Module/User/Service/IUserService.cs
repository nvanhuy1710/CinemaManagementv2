﻿using Cinema.Module.Account.Register;
using Cinema.Module.User.DTO;

namespace Cinema.Module.User.Service
{
    public interface IUserService
    {
        List<UserDTO> GetAllUsers();

        UserDTO GetUser(int id);

        UserDTO GetUserByEmail(string username);

        List<UserDTO> GetStaff();

        void ChangePassword(PasswordChangeForm form, int userId);

        UserDTO UpdateUser(UserDTO userDTO);

        UserDTO Register(RegisterData registerData, bool isStaff = false);

        void DeleteUser(int id);

        string checkToken(string token, string email);

        void ForgotPassword(string email);

        void ResetPassword(string token, string email, string password);
    }
}
