using Cinema.Data;
using Cinema.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Runtime.CompilerServices;

namespace Cinema.Module.Bill.Repository
{
    public class BillRepository : IBillRepository
    {

        private readonly DataContext _context;

        public BillRepository(DataContext context)
        {
            _context = context;
        }

        public BillModel AddBill(BillModel model)
        {
            EntityEntry<BillModel> entityEntry = _context.Bills.Add(model);
            _context.SaveChanges();
            return entityEntry.Entity;
        }

        public List<BillModel> GetBill(int id)
        {
            return _context.Bills.Include(p => p.ReservationModels).Include(p => p.FoodOrderModels).
                ThenInclude(y => y.FoodModel).Where(p => p.UserId == id).ToList();
        }

        public void Refund(int id)
        {
            BillModel billModel = _context.Bills.Where(p => p.Id == id).Single();
            billModel.BillStatus = Enum.BillStatus.REFUNDED;
            _context.Bills.Update(billModel);
            _context.SaveChanges();
        }

        public List<BillModel> GetBillByDate(DateTime startDate, DateTime endDate, int userId = 0)
        {
            List<BillModel> bills =  _context.Bills.Include(p => p.ReservationModels).
                ThenInclude(y => y.ShowModel).ThenInclude(z => z.Film).
                Include(p => p.ReservationModels).ThenInclude(p => p.SeatModel).
                Include(p => p.FoodOrderModels).ThenInclude(y => y.FoodModel).
                Where(p => p.DatePurchased.Date >= startDate.Date &&
                    p.DatePurchased.Date <= endDate.Date).ToList();
            if(userId != 0) return bills.Where(p => p.UserId == userId).ToList();
            return bills;
        }
    }
}
