using Cinema.Module.Genre.DTO;
using Cinema.Module.Genre.Service;
using Cinema.Module.Role.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Controllers
{
    [Route("/api")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly IGenreService _genreService;

        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        [Route("/genre")]
        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public IActionResult AddGenre([FromBody] GenreDTO genre)
        {
            GenreDTO genreDTO = _genreService.AddGenre(genre);
            return CreatedAtAction(nameof(GetGenre), new {id = genreDTO.Id}, genreDTO);
        }

        [Route("/genre")]
        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult GetGenres()
        {
            return Ok(_genreService.GetGenres());
        }

        [Route("/genre/{id}")]
        [Authorize(Roles = "ADMIN")]
        [HttpGet]
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

        [Route("/genre")]
        [Authorize(Roles = "ADMIN")]
        [HttpPut]
        public IActionResult UpdateGenre(GenreDTO genre)
        {
            return Ok(_genreService.UpdateGenre(genre));
        }

        [Route("/genre")]
        [Authorize(Roles = "ADMIN")]
        [HttpDelete]
        public IActionResult DeleteGenre(int id)
        {
            _genreService.DeleteGenre(id);
            return NoContent();
        }
    }
}
