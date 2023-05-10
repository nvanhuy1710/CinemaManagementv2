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

        public List<ReservationModel> GetReservationBySeatId(int seatId)
        {
            return _context.Reservations.Include(p => p.SeatModel).Where(p => p.SeatId == seatId).ToList();
        }

        public List<ReservationModel> GetReservationByShowId(int showId)
        {
            return _context.Reservations.Include(p => p.SeatModel).Where(p => p.ShowId == showId).ToList();
        }

        public ReservationModel UpdateReservation(ReservationModel reservation)
        {
            ReservationModel reservationModel = _context.Reservations.Where(p => p.Id == reservation.Id).Single();
            reservationModel.SeatId = reservation.SeatId;
            reservationModel.ShowId = reservation.ShowId;
            reservationModel.BillId = reservationModel.BillId;
            EntityEntry<ReservationModel> entityEntry = _context.Reservations.Update(reservationModel);
            _context.SaveChanges();
            return entityEntry.Entity;
        }
    }
}
