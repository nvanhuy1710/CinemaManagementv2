using Cinema.Module.Film.Service;
using Cinema.Module.Room.DTO;
using Cinema.Module.Room.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Controllers
{
    [Route("api/")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("room")]
        public IActionResult AddRoom([FromBody] RoomDTO roomDTO)
        {
            RoomDTO result = _roomService.AddRoom(roomDTO);
            return CreatedAtAction(nameof(GetRoom), new {id =  roomDTO.Id}, roomDTO);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("room/{id}")]
        public IActionResult GetRoom(int id)
        {
            return Ok(_roomService.GetRoom(id));
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("room")]
        public IActionResult GetRooms()
        {
            return Ok(_roomService.GetRooms());
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("room/{id}")]
        public IActionResult DeleteRooms(int id)
        {
            _roomService.DeleteRoom(id);
            return NoContent();
        }
    }
}
