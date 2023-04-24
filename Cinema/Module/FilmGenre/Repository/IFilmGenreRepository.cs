using Cinema.Model;

namespace Cinema.Module.FilmGenre.Repository
{
    public interface IFilmGenreRepository
    {
        FilmGenreModel AddFilmGenre(FilmGenreModel filmGenre);

        List<FilmGenreModel> GetFilmGenreModelsByFilmId(int id);

        void DeleteFilmGenre(int id);
    }
}
