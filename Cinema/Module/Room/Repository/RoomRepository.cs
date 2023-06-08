using Cinema.Data;
using Cinema.Enum;
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
            RoomModel oldRoom = _context.Rooms.Include(p => p.ShowModels).Where(p => p.Id == id).Single();
            if (oldRoom.ShowModels.Any(p => p.StartTime.Date >= DateTime.Now.Date))
            {
                throw new InvalidOperationException("Room has a schedule but hasn't shown yet");
            }
            _context.Rooms.Remove(_context.Rooms.Where(p => p.Id == id).Single());
        }

        public RoomModel GetRoom(int id)
        {
            IEnumerable<RoomModel> roomModels = _context.Rooms.Where(p => p.Id == id && p.RoomStatus != RoomStatus.DELETED);
            if (roomModels.Any())
            {
                return roomModels.First();
            }
            else
            {
                return null;
            }
        }

        public RoomModel GetRoom(string roomName)
        {
            IEnumerable<RoomModel> roomModels = _context.Rooms.Where(p => p.Name == roomName && p.RoomStatus != RoomStatus.DELETED);
            if(roomModels.Any())
            {
                return roomModels.First();
            }
            else
            {
                return null;
            }
        }

        public List<RoomModel> GetRooms()
        {
            return _context.Rooms.Where(p => p.RoomStatus != RoomStatus.DELETED).ToList();
        }

        public RoomModel UpdateRoom(RoomModel room)
        {
            RoomModel oldRoom = _context.Rooms.Include(p => p.ShowModels).Where(p => p.Id == room.Id).Single();
            if (room.RoomStatus == RoomStatus.DELETED)
            {             
                if (oldRoom.ShowModels.Any(p => p.StartTime.Date >= DateTime.Now.Date && !p.IsDeleted))
                {
                    throw new InvalidOperationException("Room has a schedule but hasn't shown yet");
                }
            }
            oldRoom.Name = room.Name;
            oldRoom.Row = room.Row;
            oldRoom.Col = room.Col;
            EntityEntry<RoomModel> entityEntry = _context.Rooms.Update(oldRoom);
            _context.SaveChanges();
            return entityEntry.Entity;
        }

        public RoomModel ChangeStatusRoom(int roomId, RoomStatus roomStatus)
        {
            RoomModel oldRoom = _context.Rooms.Include(p => p.ShowModels).Where(p => p.Id == roomId).Single();
            if (oldRoom.ShowModels.Any(p => p.StartTime.Date >= DateTime.Now.Date) && roomStatus == RoomStatus.REPAIRING) return null;
            oldRoom.RoomStatus = roomStatus;
            EntityEntry<RoomModel> entityEntry = _context.Rooms.Update(oldRoom);
            _context.SaveChanges();
            return entityEntry.Entity;
        }
    }
}
