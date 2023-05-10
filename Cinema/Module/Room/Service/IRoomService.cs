using Cinema.Enum;
using Cinema.Model;
using Cinema.Module.Room.DTO;

namespace Cinema.Module.Room.Service
{
    public interface IRoomService
    {
        RoomDTO AddRoom(RoomDTO room);

        RoomDTO UpdateRoom(RoomDTO room);

        void ChangeStatusRoom(int roomId, RoomStatus roomStatus);

        RoomDTO GetRoom(int id);

        List<RoomDTO> GetRooms();

        void DeleteRoom(int id);
    }
}
