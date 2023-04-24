using Cinema.Module.Film.DTO;

namespace Cinema.Module.Film.Service
{
    public interface IFilmService
    {
        FilmDTO AddFilm(FilmDTO filmDTO);

        FilmDTO UpdateFilm(FilmDTO filmDTO);

        void SavePoster(int id, string name, IFormFile file);

        void DeleteFilm(int id);

        FilmDTO GetFilm(int id);

        List<FilmDTO> GetFilms();
    }
}
