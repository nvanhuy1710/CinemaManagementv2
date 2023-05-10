using Cinema.Model;
using Cinema.Module.Film.DTO;

namespace Cinema.Module.Film.Service
{
    public interface IFilmService
    {
        FilmDTO AddFilm(FilmDTO filmDTO, string PosterUrl, string AdPosterUrl);

        FilmDTO UpdateFilm(FilmDTO filmDTO);

        string SavePoster(string name, string director, IFormFile file, bool isAdPoster = false);

        List<FilmDTO> GetCurrentFilms();

        List<FilmDTO> GetIncomingFilms();

        void DeleteFilm(int id);

        FilmDTO GetFilm(int id);

        List<FilmDTO> GetFilms();

        List<FilmDTO> GetFilms(string name);

        List<FilmDTO> GetDeletedFilms(string name);
    }
}
