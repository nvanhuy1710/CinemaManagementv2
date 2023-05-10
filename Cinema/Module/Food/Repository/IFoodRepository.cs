using Cinema.Model;

namespace Cinema.Module.Food.Repository
{
    public interface IFoodRepository
    {
        List<FoodModel> GetFoods();

        FoodModel GetFood(int id);
    }
}
