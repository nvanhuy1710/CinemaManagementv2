using Cinema.Module.Account.DTO;
using Cinema.Module.Film.DTO;
using Cinema.Module.Film.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;

namespace Cinema.Controllers
{
    [Route("/api")]
    [ApiController]
    public class FilmController : ControllerBase
    {
        private readonly IFilmService _filmService;

        public FilmController(IFilmService filmService)
        {
            _filmService = filmService;
        }

        [Route("/film")]
        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public IActionResult AddFilm()
        {
            try
            {
                var _uploadedImagefiles = Request.Form.Files.GetFile("image");
                string json = Request.Form["film"];
                FilmDTO filmDTO = JsonConvert.DeserializeObject<FilmDTO>(json);
                FilmDTO film = _filmService.AddFilm(filmDTO);
                if(_uploadedImagefiles  != null)
                {
                    _filmService.SavePoster(film.Id, film.Name, _uploadedImagefiles);
                }
                return Ok(film);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("/film")]
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetFilms()
        {
            return Ok(_filmService.GetFilms());
        }

        [Route("/film/{id}")]
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetFilm(int id)
        {
            return Ok(_filmService.GetFilm(id));
        }

        [Route("/film")]
        [Authorize(Roles = "ADMIN")]
        [HttpPut]
        public IActionResult UpdateFilm([FromBody] FilmDTO filmDTO)
        {
            var film = _filmService.UpdateFilm(filmDTO);
            return Ok(film);
        }

        [Route("/film/poster/{id}")]
        [Authorize(Roles = "ADMIN")]
        [HttpPut]
        public IActionResult UpdatePoster(int id)
        {
            try
            {
                var _uploadedImagefiles = Request.Form.Files.GetFile("image");
                FilmDTO oldFilm = _filmService.GetFilm(id);
                if(_uploadedImagefiles != null)
                {
                    _filmService.SavePoster(oldFilm.Id, oldFilm.Name, _uploadedImagefiles);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("/film/{id}")]
        [Authorize(Roles = "ADMIN")]
        [HttpDelete]
        public IActionResult DeleteFilm(int id)
        {
            _filmService.DeleteFilm(id);
            return NoContent();
        }
    }
}
