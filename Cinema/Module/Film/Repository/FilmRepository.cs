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

        public List<FilmModel> GetAllFilms()
        {
            return _context.Films.ToList();
        }

        public List<FilmModel> GetCurrentFilms()
        {
            List<FilmModel> filmModels = _context.Films.Include(p => p.ShowModels).Include(p => p.FilmGenreModels).ThenInclude(y => y.Genre).ToList();
            List<FilmModel> currentFilms = filmModels.
                Where(p => p.ReleaseDate <= DateTime.Now.Date && p.FilmStatus != Enum.FilmStatus.DELETED).ToList();
            return currentFilms;
        }

        public List<FilmModel> GetDeletedFilms(string name)
        {
            IQueryable<FilmModel> query = _context.Films.Include(p => p.FilmGenreModels).ThenInclude(y => y.Genre).
                Where(p => p.FilmStatus == Enum.FilmStatus.DELETED);
            if (name != null) return query.Where(p => p.Name.Contains(name)).ToList();
            else return query.ToList();
        }

        public FilmModel GetFilm(int id)
        {
            return _context.Films.Include(p => p.FilmGenreModels).ThenInclude(y => y.Genre).
                Where(p => p.Id == id).Single();
        }

        public List<FilmModel> GetFilms(string name)
        {
            return _context.Films.Include(p => p.FilmGenreModels).ThenInclude(y => y.Genre).
                Where(p => p.Name.ToLower().Contains(name.ToLower()) && p.FilmStatus != Enum.FilmStatus.DELETED).ToList();
        }

        public List<FilmModel> GetFilms()
        {
            return _context.Films.Include(p => p.FilmGenreModels).ThenInclude(y => y.Genre).
                Where(p => p.FilmStatus != Enum.FilmStatus.DELETED).ToList();
        }

        public List<FilmModel> GetIncomingFilms()
        {
            return _context.Films.Include(p => p.FilmGenreModels).ThenInclude(y => y.Genre).
                Where(p => p.ReleaseDate >= DateTime.Now.Date && p.FilmStatus != Enum.FilmStatus.DELETED).ToList();
        }

        public FilmModel UpdateFilm(FilmModel film)
        {
            FilmModel OldFilm = GetFilm(film.Id);
            if(film.FilmStatus == Enum.FilmStatus.DELETED)
            {
                if(_context.Films.Include(p => p.ShowModels).Where(p => p.Id == film.Id).
                    Any(p => p.ShowModels.Any(y => y.StartTime.Date >= DateTime.Now.Date && !y.IsDeleted)))
                {
                    throw new InvalidOperationException("The movie has a schedule but hasn't shown yet");
                }
            }
            if(OldFilm.Length != film.Length)
            {
                if (_context.Films.Include(p => p.ShowModels).Where(p => p.Id == film.Id).
                    Any(p => p.ShowModels.Any(y => y.StartTime.Date >= DateTime.Now.Date && !y.IsDeleted)))
                {
                    throw new InvalidOperationException("Can't update length when have show");
                }
            }
            OldFilm.Name = film.Name;
            OldFilm.Country = film.Country;
            OldFilm.Length = film.Length;
            OldFilm.Director = film.Director;
            OldFilm.Actor = film.Actor;
            OldFilm.AgeLimit = film.AgeLimit;
            OldFilm.FilmStatus = film.FilmStatus;
            OldFilm.Content = film.Content;
            OldFilm.Trailer = film.Trailer;
            OldFilm.ReleaseDate = film.ReleaseDate;
            EntityEntry<FilmModel> entityEntry = _context.Update(OldFilm);
            _context.SaveChanges();
            return entityEntry.Entity;
        }

        public void UpdateImage(int id, string? Poster, string? AdPoster)
        {
            FilmModel filmModel = GetFilm(id);
            if(Poster != null) filmModel.PosterUrl = Poster;
            if(AdPoster != null) filmModel.AdPosterUrl = AdPoster;
            UpdateFilm(filmModel);
        }
    }
}
