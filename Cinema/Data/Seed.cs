using Cinema.Helper;
using Cinema.Model;
using System.Data;

namespace Cinema.Data
{
    public class Seed
    {
        private readonly DataContext dataContext;
        public Seed(DataContext context)
        {
            this.dataContext = context;
        }
        public void SeedDataContext()
        {
            if (!dataContext.Roles.Any())
            {
                dataContext.Roles.AddRange(new RoleModel[]
                {
                    new RoleModel() {Name = "ADMIN"},
                    new RoleModel() {Name = "STAFF"},
                    new RoleModel() {Name = "USER"},
                });
                dataContext.SaveChanges();
            }
            if (!dataContext.Accounts.Any())
            {
                dataContext.Accounts.AddRange(new AccountModel[]
                {
                    new AccountModel() {Email = "User@gmail.com", Password = HashPassword.HashByPBKDF2("user"), AccountStatus = Enum.AccountStatus.ACTIVATED, RoleId = Convert.ToInt32(dataContext.Roles.Where(p => p.Name == "USER").Select(p => p.Id).Single())},
                    new AccountModel() {Email = "Admin@gmail.com", Password = HashPassword.HashByPBKDF2("admin"), AccountStatus = Enum.AccountStatus.ACTIVATED, RoleId = Convert.ToInt32(dataContext.Roles.Where(p => p.Name == "ADMIN").Select(p => p.Id).Single())},
                    new AccountModel() {Email = "Staff@gmail.com", Password = HashPassword.HashByPBKDF2("staff"), AccountStatus = Enum.AccountStatus.ACTIVATED, RoleId = Convert.ToInt32(dataContext.Roles.Where(p => p.Name == "STAFF").Select(p => p.Id).Single())},
                });
                dataContext.SaveChanges();
            }
            if (!dataContext.Users.Any())
            {
                dataContext.Users.AddRange(new UserModel[]
                {
                    new UserModel() {Name = "User", Phone = "09059821356", AccountId = Convert.ToInt32(dataContext.Accounts.Where(p => p.Email == "User@gmail.com").Select(p => p.Id).Single())},
                    new UserModel() {Name = "Admin", Phone = "090598213526", AccountId = Convert.ToInt32(dataContext.Accounts.Where(p => p.Email == "Admin@gmail.com").Select(p => p.Id).Single())},
                    new UserModel() {Name = "Staff", Phone = "097556564567", AccountId = Convert.ToInt32(dataContext.Accounts.Where(p => p.Email == "Staff@gmail.com").Select(p => p.Id).Single())},
                });
                dataContext.SaveChanges();
            }
            if(!dataContext.SeatTypes.Any())
            {
                dataContext.SeatTypes.AddRange(new SeatTypeModel[]
                {
                    new SeatTypeModel() {Name = "Thường", Cost = 40000,},
                    new SeatTypeModel() {Name = "VIP", Cost = 50000,},
                    new SeatTypeModel() {Name = "Đôi", Cost = 70000,},
                });
                dataContext.SaveChanges();
            }
        }
    }
}
