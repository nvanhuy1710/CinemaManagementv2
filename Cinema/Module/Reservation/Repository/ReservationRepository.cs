using Cinema.Data;
using Cinema.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Cinema.Module.Reservation.Repository
{
    public class ReservationRepository : IReservationRepository
    {

        private readonly DataContext _context;

        public ReservationRepository(DataContext context)
        {
            _context = context;
        }

        public ReservationModel AddReservation(ReservationModel reservation)
        {
            EntityEntry<ReservationModel> entityEntry = _context.Reservations.Add(reservation);
            _context.SaveChanges();
            return entityEntry.Entity;
        }

        public List<ReservationModel> GetReservationByBillId(int billId)
        {
            return _context.Reservations.Include(p => p.SeatModel).Where(p => p.BillId == billId).ToList();
        }

        public List<ReservationModel> GetReservationByShowId(int showId)
        {
            return _context.Reservations.Include(p => p.SeatModel).Where(p => p.ShowId == showId).ToList();

        }
    }
}
