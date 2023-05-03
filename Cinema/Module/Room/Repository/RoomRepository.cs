using Cinema.Data;
using Cinema.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Cinema.Module.Room.Repository
{
    public class RoomRepository : IRoomRepository
    {

        private readonly DataContext _context;

        public RoomRepository(DataContext context)
        {
            _context = context;
        }

        public RoomModel AddRoom(RoomModel room)
        {
            EntityEntry<RoomModel> entityEntry = _context.Rooms.Add(room);
            _context.SaveChanges();
            return entityEntry.Entity;
        }

        public void DeleteRoom(int id)
        {
            _context.Rooms.Remove(_context.Rooms.Where(p => p.Id == id).Single());
        }

        public RoomModel GetRoom(int id)
        {
            return _context.Rooms.Where(p => p.Id == id).Single();
        }

        public List<RoomModel> GetRooms()
        {
            return _context.Rooms.ToList();
        }

        public RoomModel UpdateRoom(RoomModel room)
        {
            RoomModel oldRoom = _context.Rooms.Where(p => p.Id == room.Id).Single();
            oldRoom.Row = room.Row;
            oldRoom.Col = room.Col;
            oldRoom.RoomStatus = room.RoomStatus;
            EntityEntry<RoomModel> entityEntry = _context.Rooms.Update(oldRoom);
            _context.SaveChanges();
            return entityEntry.Entity;
        }
    }
}
