﻿using Cinema.Data;
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
            return _context.Shows.Include(p => p.Film).Include(p => p.Room).Where(p => p.Id == id).Single();
        }

        public List<ShowModel> GetShowInTime(DateTime startDate, DateTime endDate)
        {
            return _context.Shows.Include(p => p.Film).Include(p => p.Room).Where(p => p.StartTime.Date >= startDate && p.StartTime.Date <= endDate).ToList();
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

        public List<ShowModel> GetShowByInfor(int filmId, int roomId, DateTime date)
        {
            IQueryable<ShowModel> query = _context.Shows.Include(p => p.Film).Include(p => p.Room);

            if (filmId != 0)
            {
                query = query.Where(p => p.FilmId == filmId);
            }

            if (roomId != 0)
            {
                query = query.Where(p => p.RoomId == roomId);
            }

            query = query.Where(p => p.StartTime.Date == date.Date);

            return query.ToList();
        }
    }
}
