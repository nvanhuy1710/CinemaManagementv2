using Cinema.Data;
using Cinema.Model;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Cinema.Module.FilmGenre.Repository
{
    public class FilmGenreRepository : IFilmGenreRepository
    {

        private readonly DataContext _context;

        public FilmGenreRepository(DataContext context)
        {
            _context = context;
        }
        public FilmGenreModel AddFilmGenre(FilmGenreModel filmGenre)
        {
            EntityEntry<FilmGenreModel> entityEntry = _context.FilmGenreModels.Add(filmGenre);
            _context.SaveChanges();
            return entityEntry.Entity;
        }

        public void DeleteFilmGenre(int id)
        {
            _context.FilmGenreModels.Remove(_context.FilmGenreModels.Where(p => p.Id == id).Single());
            _context.SaveChanges();
        }

        public List<FilmGenreModel> GetFilmGenreModelsByFilmId(int id)
        {
            return _context.FilmGenreModels.Where(p => p.FilmId == id).ToList();
        }
    }
}
