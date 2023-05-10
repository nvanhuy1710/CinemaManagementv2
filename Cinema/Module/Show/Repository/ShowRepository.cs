using Cinema.Data;
using Cinema.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Cinema.Module.Show.Repository
{
    public class ShowRepository : IShowRepository
    {

        private readonly DataContext _context;

        public ShowRepository(DataContext context)
        {
            _context = context;
        }

        public ShowModel AddShow(ShowModel model)
        {
            model.IsDeleted = false;
            EntityEntry<ShowModel> entityEntry = _context.Shows.Add(model);
            _context.SaveChanges();
            return entityEntry.Entity;
        }

        public void DeleteShow(int id)
        {
            _context.Shows.Remove(_context.Shows.Where(p => p.Id == id).Single());
        }

        public ShowModel GetShow(int id)
        {
            return _context.Shows.Include(p => p.Film).Include(p => p.Room).Where(p => p.Id == id && p.IsDeleted).Single();
        }

        public List<ShowModel> GetShowInTime(DateTime startDate, DateTime endDate)
        {
            return _context.Shows.Include(p => p.Film).Include(p => p.Room).Where(p => p.StartTime.Date >= startDate && p.StartTime.Date <= endDate && p.IsDeleted).ToList();
        }

        public ShowModel UpdateShow(ShowModel model)
        {
            ShowModel oldShow = _context.Shows.Where(p => p.Id == model.Id).Single();
            oldShow.StartTime = model.StartTime;
            oldShow.EndTime = model.EndTime;
            oldShow.FilmId = model.FilmId;
            oldShow.RoomId = model.RoomId;
            EntityEntry<ShowModel> entityEntry = _context.Shows.Update(oldShow);
            _context.SaveChanges();
            return entityEntry.Entity;
        }

        public List<List<ShowModel>> GetShowByInfor(int filmId, int roomId, DateTime date)
        {
            IQueryable<ShowModel> query = _context.Shows.Include(p => p.Film).Include(p => p.Room).Where(p => !p.IsDeleted);

            if (filmId != 0)
            {
                query = query.Where(p => p.FilmId == filmId && p.StartTime.Date >= DateTime.Now.Date);
            }

            if (roomId != 0)
            {
                query = query.Where(p => p.RoomId == roomId && p.StartTime.Date >= DateTime.Now.Date);
            }

            if(date != DateTime.MinValue)
            {
                query = query.Where(p => p.StartTime.Date == date.Date);
            }

            return query.GroupBy(s => s.FilmId).Select(g => g.ToList()).ToList();
        }
    }
}
