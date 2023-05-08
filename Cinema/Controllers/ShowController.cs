using Cinema.Module.Show.DTO;
using Cinema.Module.Show.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Controllers
{

    [Route("api/")]
    [ApiController]
    public class ShowController : ControllerBase
    {
        private readonly IShowService _showService;

        public ShowController(IShowService showService)
        {
            _showService = showService;
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("show")]
        public IActionResult AddShow([FromBody] ShowDTO showDTO)
        {
            try
            {
                ShowDTO result = _showService.AddShow(showDTO);
                return CreatedAtAction(nameof(GetShow), new { id = showDTO.Id }, showDTO);
            } catch (InvalidOperationException ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("show")]
        public IActionResult UpdateShow([FromBody] ShowDTO showDTO)
        {
            try
            {
                ShowDTO result = _showService.UpdateShow(showDTO);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("show/{id}")]
        public IActionResult GetShow(int id)
        {
            return Ok(_showService.GetShow(id));
        }

        [AllowAnonymous]
        [HttpGet("show")]
        public IActionResult GetShowByInfor(DateTime date, int filmId = 0, int roomId = 0)
        {
            return Ok(_showService.GetShowByInfor(filmId, roomId, date));
        }
    }
}
