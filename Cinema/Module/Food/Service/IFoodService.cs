using Cinema.Module.Food.DTO;

namespace Cinema.Module.Food.Service
{
    public interface IFoodService
    {
        List<FoodDTO> GetFoods();

        FoodDTO GetFood(int id);
    }
}
