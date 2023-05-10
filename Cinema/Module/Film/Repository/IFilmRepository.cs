using Cinema.Model;

namespace Cinema.Module.Film.Repository
{
    public interface IFilmRepository
    {
        FilmModel AddFilm(FilmModel film);

        List<FilmModel> GetFilms(string name);

        FilmModel GetFilm(int id);

        List<FilmModel> GetFilms();

        List<FilmModel> GetAllFilms();

        List<FilmModel> GetCurrentFilms();

        List<FilmModel> GetIncomingFilms();

        List<FilmModel> GetDeletedFilms(string name);

        FilmModel UpdateFilm(FilmModel film);

        void UpdateImage(int id, string Poster, string AdPoster);

        void DeleteFilm(int id);
    }
}
