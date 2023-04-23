using Cinema.Data;
using Cinema.Model;
using Cinema.Module.Genre.DTO;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Cinema.Module.Genre.Repository
{
    public class GenreRepository : IGenreRepository
    {

        private readonly DataContext _context;

        public GenreRepository(DataContext context)
        {
            _context = context;
        }

        public GenreModel AddGenre(GenreModel model)
        {
            EntityEntry<GenreModel> entityEntry = _context.Genres.Add(model);
            _context.SaveChanges();
            return entityEntry.Entity;
        }

        public void DeleteGenre(int id)
        {
            _context.Genres.Remove(_context.Genres.Where(p => p.Id == id).Single());
            _context.SaveChanges();
        }

        public List<GenreModel> GetAllGenres()
        {
            return _context.Genres.ToList();
        }

        public GenreModel GetGenre(int id)
        {
            return _context.Genres.Where(p => p.Id == id).Single();
        }

        public GenreModel UpdateGenre(GenreModel model)
        {
            GenreModel genre = GetGenre(model.Id);
            genre.Name = model.Name;
            EntityEntry<GenreModel> entityEntry = _context.Genres.Update(genre);
            _context.SaveChanges();
            return entityEntry.Entity;
        }
    }
}
