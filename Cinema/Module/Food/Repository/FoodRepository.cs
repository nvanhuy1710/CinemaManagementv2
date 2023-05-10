using Cinema.Data;
using Cinema.Model;

namespace Cinema.Module.Food.Repository
{
    public class FoodRepository : IFoodRepository
    {

        private readonly DataContext _context;

        public FoodRepository(DataContext context)
        {
            _context = context;
        }

        public FoodModel GetFood(int id)
        {
            return _context.Foods.Where(p => p.Id == id).Single();
        }

        public List<FoodModel> GetFoods()
        {
            return _context.Foods.ToList();
        }
    }
}
