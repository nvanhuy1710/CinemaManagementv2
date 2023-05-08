using Cinema.Model;

namespace Cinema.Module.FoodOrder.Repository
{
    public interface IFoodOrderRepository
    {
        FoodOrderModel AddFoodOrder(FoodOrderModel foodOrder);

        FoodOrderModel GetFoodOrder(int id);

        List<FoodOrderModel> GetFoodOrderByBillId(int id);

        void DeleteFoodOrder(int id);
    }
}
