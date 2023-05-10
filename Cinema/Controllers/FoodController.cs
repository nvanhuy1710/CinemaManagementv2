using Cinema.Module.Food.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Controllers
{
    [Route("api/")]
    [ApiController]
    public class FoodController : ControllerBase
    {

        private readonly IFoodService _foodService;

        public FoodController(IFoodService foodService)
        {
            _foodService = foodService;
        }

        [Authorize]
        [HttpGet("food")]
        public IActionResult GetFoods()
        {
            return Ok(_foodService.GetFoods());
        }

        [Authorize]
        [HttpGet("food/{id}")]
        public IActionResult GetFood(int id)
        {
            return Ok(_foodService.GetFood(id));
        }
    }
}
