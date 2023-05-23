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
        public IActionResult AddShow([FromBody] List<ShowDTO> showDTOs)
        {
            try
            {
                List<ShowDTO> result = _showService.AddShow(showDTOs);
                List<int> ids = result.Select(p => p.Id).ToList();
                return CreatedAtAction(nameof(GetShow), new { id = ids }, result);
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
            ShowDTO show = _showService.GetShow(id);
            if (show != null)
            {
                return Ok(show);
            }
            else
            {
                return NotFound();
            }
        }

        [AllowAnonymous]
        [HttpGet("show")]
        public IActionResult GetShowByInfor(DateTime date, int filmId = 0, int roomId = 0)
        {
            return Ok(_showService.GetShowByInfor(filmId, roomId, date));
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("show/time")]
        public IActionResult GetShowInTime(DateTime startDate, DateTime endDate)
        {
            return Ok(_showService.GetShowInTime(startDate, endDate));
        }
    }
}
