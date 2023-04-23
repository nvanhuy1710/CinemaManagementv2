using Cinema.Model;
using Cinema.Module.Genre.DTO;

namespace Cinema.Module.Genre.Repository
{
    public interface IGenreRepository
    {
        GenreModel AddGenre(GenreModel model);

        GenreModel UpdateGenre(GenreModel model);

        void DeleteGenre(int id);

        GenreModel GetGenre(int id);

        List<GenreModel> GetAllGenres();
    }
}
