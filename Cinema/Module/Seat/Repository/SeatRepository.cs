using Cinema.Data;
using Cinema.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Cinema.Module.Seat.Repository
{
    public class SeatRepository : ISeatRepository
    {

        private readonly DataContext _context;

        public SeatRepository(DataContext context)
        {
            _context = context;
        }

        public SeatModel AddSeat(SeatModel seat)
        {
            EntityEntry<SeatModel> entityEntry = _context.Seats.Add(seat);
            _context.SaveChanges();
            return entityEntry.Entity;
        }

        public void DeleteSeat(int id)
        {
            _context.Seats.Remove(_context.Seats.Where(p => p.Id == id).Single());
        }

        public SeatModel GetSeat(int id)
        {
            return _context.Seats.Include(p => p.SeatType).Where(p => p.Id == id).Single();
        }

        public List<SeatModel> GetSeats()
        {
            return _context.Seats.Include(p => p.SeatType).ToList();
        }

        public List<SeatModel> GetSeatsByRoomId(int id)
        {
            return _context.Seats.Include(p => p.SeatType).Where(p => p.RoomId == id).ToList();
        }

        public SeatModel UpdateSeat(SeatModel seat)
        {
            SeatModel oldSeat = _context.Seats.Where(p => p.Id == seat.Id).Single();
            oldSeat.Position = seat.Position;
            oldSeat.SeatTypeId = seat.SeatTypeId;
            oldSeat.RoomId = seat.RoomId;
            EntityEntry<SeatModel> entityEntry = _context.Seats.Update(oldSeat);
            _context.SaveChanges();
            return entityEntry.Entity;
        }
    }
}
