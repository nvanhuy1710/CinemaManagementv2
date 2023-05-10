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

        public DbSet<FilmModel> Films { get; set; }

        public DbSet<GenreModel> Genres { get; set; }

        public DbSet<FilmGenreModel> FilmGenreModels { get; set; }

        public DbSet<RoomModel> Rooms { get; set; }

        public DbSet<SeatModel> Seats { get; set; }

        public DbSet<SeatTypeModel> SeatTypes { get; set; }

        public DbSet<ShowModel> Shows { get; set; }

        public DbSet<BillModel> Bills { get; set; }

        public DbSet<ReservationModel> Reservations { get; set; }

        public DbSet<FoodModel> Foods { get; set; }

        public DbSet <FoodOrderModel> FoodOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AccountModel>()
                .Property(u => u.AccountStatus)
                .HasConversion<string>()
                .HasMaxLength(50);

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<FilmModel>()
                .Property(u => u.FilmStatus)
                .HasConversion<string>()
                .HasMaxLength(50);

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<RoomModel>()
                .Property(u => u.RoomStatus)
                .HasConversion<string>()
                .HasMaxLength(50);

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<BillModel>()
                .Property(u => u.BillStatus)
                .HasConversion<string>()
                .HasMaxLength(50);
        }
    }
}