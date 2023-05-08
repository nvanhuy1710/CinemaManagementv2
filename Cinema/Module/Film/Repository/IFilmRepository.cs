using Cinema.Model;

namespace Cinema.Module.Film.Repository
{
    public interface IFilmRepository
    {
        FilmModel AddFilm(FilmModel film);

        FilmModel GetFilm(int id);

        List<FilmModel> GetFilms();

        List<FilmModel> GetCurrentFilms();

        List<FilmModel> GetIncomingFilms();

        FilmModel UpdateFilm(FilmModel film);

        void UpdateImage(int id, string Poster, string AdPoster);

        void DeleteFilm(int id);
    }
}
