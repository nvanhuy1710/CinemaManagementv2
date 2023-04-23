using Cinema.Module.Genre.DTO;

namespace Cinema.Module.Genre.Service
{
    public interface IGenreService
    {
        GenreDTO AddGenre(GenreDTO genreDTO);

        GenreDTO UpdateGenre(GenreDTO genreDTO);

        List<GenreDTO> GetGenres();

        GenreDTO GetGenre(int id);

        void DeleteGenre(int id);
    }
}
