using Cinema.Model;

namespace Cinema.Module.User.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public bool Gender { get; set; }

        public DateTime Birth { get; set; }

        public string Phone { get; set; }

        public string? Address { get; set; }

        public string RoleName { get; set; }
    }
}
