using Cinema.Data;
using Cinema.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Cinema.Module.Film.Repository
{
    public class FilmRepository : IFilmRepository
    {

        private readonly DataContext _context;

        public FilmRepository(DataContext context)
        {
            _context = context;
        }

        public FilmModel AddFilm(FilmModel film)
        {
            EntityEntry<FilmModel> entityEntry = _context.Films.Add(film);
            _context.SaveChanges();
            return entityEntry.Entity;
        }

        public void DeleteFilm(int id)
        {
            _context.Films.Remove(_context.Films.Where(p => p.Id == id).Single());
            _context.SaveChanges();
        }

        public FilmModel GetFilm(int id)
        {
            return _context.Films.Include(p => p.FilmGenreModels).ThenInclude(y => y.Genre).Where(p => p.Id == id).Single();
        }

        public List<FilmModel> GetFilms()
        {
            return _context.Films.Include(p => p.FilmGenreModels).ThenInclude(y => y.Genre).ToList();
        }

        public FilmModel UpdateFilm(FilmModel film)
        {
            FilmModel OldFilm = GetFilm(film.Id);
            OldFilm.Name = film.Name;
            OldFilm.Country = film.Country;
            OldFilm.Length = film.Length;
            OldFilm.Director = film.Director;
            OldFilm.Actor = film.Actor;
            OldFilm.AgeLimit = film.AgeLimit;
            OldFilm.FilmStatus = film.FilmStatus;
            EntityEntry<FilmModel> entityEntry = _context.Update(OldFilm);
            _context.SaveChanges();
            return entityEntry.Entity;
        }
    }
}
