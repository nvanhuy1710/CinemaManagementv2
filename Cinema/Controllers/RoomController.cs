using Cinema.Enum;
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
            try 
            {
                RoomDTO result = _roomService.AddRoom(roomDTO);
                return CreatedAtAction(nameof(GetRoom), new { id = roomDTO.Id }, roomDTO);
            } catch(InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
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
            try
            {
                _roomService.DeleteRoom(id);
                return NoContent();
            } catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("room")]
        public IActionResult UpdateRoom([FromBody] RoomDTO roomDTO)
        {
            try
            {
                RoomDTO result = _roomService.UpdateRoom(roomDTO);
                return Ok(result);
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("room/status/{id}")]
        public IActionResult ChangeStatusRoom(int id, RoomStatus roomStatus)
        {
            try
            {
               _roomService.ChangeStatusRoom(id, roomStatus);
                return Ok();
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
