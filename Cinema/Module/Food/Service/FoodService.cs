using AutoMapper;
using Cinema.Module.Food.DTO;
using Cinema.Module.Food.Repository;

namespace Cinema.Module.Food.Service
{
    public class FoodService : IFoodService
    {

        private readonly IFoodRepository _foodRepository;

        private readonly IMapper _mapper;

        public FoodService(IFoodRepository foodRepository, IMapper mapper)
        {
            _foodRepository = foodRepository;
            _mapper = mapper;
        }

        public FoodDTO GetFood(int id)
        {
            return _mapper.Map<FoodDTO>(_foodRepository.GetFood(id));
        }

        public List<FoodDTO> GetFoods()
        {
            return _foodRepository.GetFoods().Select(p => _mapper.Map<FoodDTO>(p)).ToList();
        }
    }
}
