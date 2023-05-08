using Cinema.Module.Seat.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Controllers
{
    [Route("api/")]
    [ApiController]
    public class SeatController : ControllerBase
    {
        private readonly ISeatService _seatService;

        public SeatController(ISeatService seatService)
        {
            _seatService = seatService;
        }

        [Authorize]
        [HttpGet("seat")]
        public IActionResult GetSeatForBook(int roomId, int showId)
        {
            return Ok(_seatService.GetSeatForBook(roomId, showId));
        }
    }
}
