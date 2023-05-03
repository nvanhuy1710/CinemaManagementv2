﻿using Cinema.Model;

namespace Cinema.Module.Room.Repository
{
    public interface IRoomRepository
    {
        RoomModel AddRoom(RoomModel room);

        RoomModel UpdateRoom(RoomModel room);

        RoomModel GetRoom(int id);

        List<RoomModel> GetRooms();

        void DeleteRoom(int id);
    }
}
