using Cinema.Module.FoodOrder.DTO;

namespace Cinema.Module.FoodOrder.Service
{
    public interface IFoodOrderService
    {
        FoodOrderDTO AddFoodOrder(FoodOrderDTO foodOrderDTO);

        FoodOrderDTO GetFoodOrder(int id);

        List<FoodOrderDTO> GetFoodOrderByBillId(int id);

        void DeleteFoodOrder(int id);
    }
}
