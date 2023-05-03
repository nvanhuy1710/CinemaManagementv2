using Cinema.Data;
using Cinema.Model;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Cinema.Module.SeatType.Repository
{
    public class SeatTypeRepository : ISeatTypeRepository
    {

        private readonly DataContext _context;
        public SeatTypeRepository(DataContext context)
        {
            _context = context;
        }

        public SeatTypeModel AddSeatType(SeatTypeModel seatType)
        {
            EntityEntry<SeatTypeModel> entityEntry = _context.SeatTypes.Add(seatType);
            _context.SaveChanges();
            return entityEntry.Entity;
        }

        public void DeleteSeatType(int id)
        {
            _context.SeatTypes.Remove(_context.SeatTypes.Where(p => p.Id == id).Single());
            _context.SaveChanges();
        }

        public SeatTypeModel GetSeatType(int id)
        {
            return _context.SeatTypes.Where(p => p.Id == id).Single();  
        }

        public List<SeatTypeModel> GetSeatTypes()
        {
            return _context.SeatTypes.ToList();
        }

        public SeatTypeModel UpdateSeatType(SeatTypeModel seatType)
        {
            SeatTypeModel model = GetSeatType(seatType.Id);
            model.Name = seatType.Name;
            model.Cost = seatType.Cost;
            EntityEntry<SeatTypeModel> entityEntry = _context.SeatTypes.Update(model);
            _context.SaveChanges();
            return entityEntry.Entity;
        }
    }
}
