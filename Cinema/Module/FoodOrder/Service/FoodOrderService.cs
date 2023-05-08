using AutoMapper;
using Cinema.Model;
using Cinema.Module.FoodOrder.DTO;
using Cinema.Module.FoodOrder.Repository;

namespace Cinema.Module.FoodOrder.Service
{
    public class FoodOrderService : IFoodOrderService
    {

        private readonly IFoodOrderRepository _foodOrderRepository;

        private readonly IMapper _mapper;

        public FoodOrderService(IFoodOrderRepository foodOrderRepository, IMapper mapper)
        {
            _foodOrderRepository = foodOrderRepository;
            _mapper = mapper;
        }

        public FoodOrderDTO AddFoodOrder(FoodOrderDTO foodOrderDTO)
        {
            return _mapper.Map<FoodOrderDTO>(_foodOrderRepository.AddFoodOrder(_mapper.Map<FoodOrderModel>(foodOrderDTO)));
        }

        public void DeleteFoodOrder(int id)
        {
            _foodOrderRepository.DeleteFoodOrder(id);
        }

        public FoodOrderDTO GetFoodOrder(int id)
        {
            return _mapper.Map<FoodOrderDTO>(_foodOrderRepository.GetFoodOrder(id));
        }

        public List<FoodOrderDTO> GetFoodOrderByBillId(int id)
        {
            return _foodOrderRepository.GetFoodOrderByBillId(id).Select(p => _mapper.Map<FoodOrderDTO>(p)).ToList();
        }

    }
}
