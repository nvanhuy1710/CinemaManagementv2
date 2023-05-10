using Cinema.Enum;
using Cinema.Model;

namespace Cinema.Module.Room.Repository
{
    public interface IRoomRepository
    {
        RoomModel AddRoom(RoomModel room);

        RoomModel UpdateRoom(RoomModel room);

        RoomModel ChangeStatusRoom(int roomId, RoomStatus roomStatus);

        RoomModel GetRoom(int id);

        RoomModel GetRoom(string roomName);

        List<RoomModel> GetRooms();

        void DeleteRoom(int id);
    }
}
