using Cinema.Enum;
using Cinema.Module.Role.DTO;

namespace Cinema.Module.Account.DTO
{
    public class AccountDTO
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public AccountStatus AccountStatus { get; set; }

        public string RoleName { get; set; }
    }
}
