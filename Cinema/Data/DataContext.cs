using Cinema.Model;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Cinema.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<AccountModel> Accounts { get; set; }

        public DbSet<UserModel> Users { get; set; }

        public DbSet<RoleModel> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AccountModel>()
                .Property(u => u.AccountStatus)
                .HasConversion<string>()
                .HasMaxLength(50);
        }
    }
}