using Cinema.Data;
using Cinema.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Cinema.Module.FoodOrder.Repository
{
    public class FoodOrderRepository : IFoodOrderRepository
    {
        private readonly DataContext _context;

        public FoodOrderRepository(DataContext context) 
        {
            _context = context;
        }
        public FoodOrderModel AddFoodOrder(FoodOrderModel foodOrder)
        {
            EntityEntry<FoodOrderModel> entityEntry = _context.FoodOrders.Add(foodOrder);
            _context.SaveChanges();
            return entityEntry.Entity;
        }

        public void DeleteFoodOrder(int id)
        {
            _context.FoodOrders.Remove(_context.FoodOrders.Where(p => p.Id == id).Single());
        }

        public FoodOrderModel GetFoodOrder(int id)
        {
            return _context.FoodOrders.Include(p => p.FoodModel).Where(p => p.Id != id).Single();
        }

        public List<FoodOrderModel> GetFoodOrderByBillId(int id)
        {
            return _context.FoodOrders.Include(p => p.FoodModel).Where(p => p.BillId == id).ToList();
        }
    }
}
