using Cinema.Model;

namespace Cinema.Module.Film.Repository
{
    public interface IFilmRepository
    {
        FilmModel AddFilm(FilmModel film);

        FilmModel GetFilm(int id);

        List<FilmModel> GetFilms();

        FilmModel UpdateFilm(FilmModel film);

        void DeleteFilm(int id);
    }
}
