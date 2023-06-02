using Cinema.Module.SeatType.DTO;
using Cinema.Module.SeatType.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Controllers
{
    [Route("api/")]
    [ApiController]
    public class SeatTypeController : ControllerBase
    {
        private readonly ISeatTypeService _seatTypeService;

        public SeatTypeController(ISeatTypeService seatTypeService)
        {
            _seatTypeService = seatTypeService;
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("seat-type")]
        public IActionResult AddSeatType([FromBody] SeatTypeDTO seatType)
        {
            SeatTypeDTO result = _seatTypeService.AddSeatType(seatType);
            return CreatedAtAction(nameof(GetSeatType), new {id = result.Id}, result);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("seat-type")]
        public IActionResult UpdateSeatType([FromBody] SeatTypeDTO seatType)
        {
            return Ok(_seatTypeService.UpdateSeatType(seatType));
        }

        [Authorize]
        [HttpGet("seat-type/{id}")]
        public IActionResult GetSeatType(int id)
        {
            return Ok(_seatTypeService.GetSeatType(id));
        }

        [Authorize]
        [HttpGet("seat-type")]
        public IActionResult GetSeatTypes()
        {
            return Ok(_seatTypeService.GetSeatTypes());
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("seat-type/{id}")]
        public IActionResult DeleteSeatTypes(int id)
        {
            _seatTypeService.DeleteSeatType(id);
            return NoContent();
        }
    }
}
