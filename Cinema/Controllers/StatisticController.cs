using Cinema.Module.Statistic.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Controllers
{
    [Route("api/")]
    [ApiController]
    public class StatisticController : ControllerBase
    {
        private readonly IStatisticService _statisticService;

        public StatisticController(IStatisticService statisticService)
        {
            _statisticService = statisticService;
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("statistic")]
        public IActionResult GetStatisticByYear(int year)
        {
            return Ok(_statisticService.GetStatistic(year));
        }
    }
}
