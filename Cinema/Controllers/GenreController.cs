using Cinema.Module.Genre.DTO;
using Cinema.Module.Genre.Service;
using Cinema.Module.Role.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Controllers
{
    [Route("api/")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly IGenreService _genreService;

        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("genre")]
        public IActionResult AddGenre([FromBody] GenreDTO genre)
        {
            GenreDTO genreDTO = _genreService.AddGenre(genre);
            return CreatedAtAction(nameof(GetGenre), new {id = genreDTO.Id}, genreDTO);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("genre")]
        public IActionResult GetGenres()
        {
            return Ok(_genreService.GetGenres());
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("genre/{id}")]
        public IActionResult GetGenre(int id)
        {
            try
            {
                return Ok(_genreService.GetGenre(id));
            }
            catch(InvalidOperationException)
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("genre")]
        public IActionResult UpdateGenre(GenreDTO genre)
        {
            return Ok(_genreService.UpdateGenre(genre));
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("genre")]
        public IActionResult DeleteGenre(int id)
        {
            _genreService.DeleteGenre(id);
            return NoContent();
        }
    }
}
