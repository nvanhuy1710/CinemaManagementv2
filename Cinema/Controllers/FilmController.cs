using Cinema.Module.Account.DTO;
using Cinema.Module.Film.DTO;
using Cinema.Module.Film.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;

namespace Cinema.Controllers
{
    [Route("api/")]
    [ApiController]
    public class FilmController : ControllerBase
    {
        private readonly IFilmService _filmService;

        public FilmController(IFilmService filmService)
        {
            _filmService = filmService;
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("film")]
        public IActionResult AddFilm()
        {
            try
            {
                var uploadedImagefile = Request.Form.Files.GetFile("poster");
                var uploadedAdImagefile = Request.Form.Files.GetFile("adposter");
                string json = Request.Form["film"];
                FilmDTO filmDTO = JsonConvert.DeserializeObject<FilmDTO>(json);
                string path = string.Empty;
                string adPath = string.Empty;
                if(uploadedImagefile != null)
                {
                    path = _filmService.SavePoster(filmDTO.Name, filmDTO.Director, uploadedImagefile);
                }
                if (uploadedAdImagefile != null)
                {
                    adPath = _filmService.SavePoster(filmDTO.Name, filmDTO.Director, uploadedAdImagefile, true);
                }
                FilmDTO film = _filmService.AddFilm(filmDTO, path, adPath);
                return Ok(film);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("film")]
        public IActionResult GetFilms(string? name = null)
        {
            return Ok(_filmService.GetFilms(name));
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("film/all")]
        public IActionResult GetAllFilms()
        {
            return Ok(_filmService.GetFilms());
        }

        [AllowAnonymous]
        [HttpGet("film/{id}")]
        public IActionResult GetFilm(int id)
        {
            return Ok(_filmService.GetFilm(id));
        }

        [AllowAnonymous]
        [HttpGet("film/showing")]
        public IActionResult GetCurrentFilm()
        {
            return Ok(_filmService.GetCurrentFilms());
        }

        [AllowAnonymous]
        [HttpGet("film/incoming")]
        public IActionResult GetIncomingFilm()
        {
            return Ok(_filmService.GetIncomingFilms());
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("film")]
        public IActionResult UpdateFilm([FromBody] FilmDTO filmDTO)
        {
            try
            {
                var film = _filmService.UpdateFilm(filmDTO);
                return Ok(film);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("film/poster/{id}")]
        public IActionResult UpdatePoster(int id)
        {
            try
            {
                var _uploadedImagefiles = Request.Form.Files.GetFile("poster");
                FilmDTO oldFilm = _filmService.GetFilm(id);
                if(_uploadedImagefiles != null)
                {
                    oldFilm.PosterUrl = _filmService.SavePoster(oldFilm.Name, oldFilm.Director, _uploadedImagefiles);
                    _filmService.UpdateFilm(oldFilm);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("film/ad-poster/{id}")]
        public IActionResult UpdateAdPoster(int id)
        {
            try
            {
                var _uploadedImagefiles = Request.Form.Files.GetFile("adposter");
                FilmDTO oldFilm = _filmService.GetFilm(id);
                if (_uploadedImagefiles != null)
                {
                    oldFilm.AdPosterUrl = _filmService.SavePoster(oldFilm.Name, oldFilm.Director, _uploadedImagefiles, true);
                    _filmService.UpdateFilm(oldFilm);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("film/{id}")]
        public IActionResult DeleteFilm(int id)
        {
            try
            {
                _filmService.DeleteFilm(id);
                return NoContent();
            } catch(InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("film")]
        public IActionResult GetDeletedFilms(string? name = null)
        {
            return Ok(_filmService.GetDeletedFilms(name));
        }
    }
}
