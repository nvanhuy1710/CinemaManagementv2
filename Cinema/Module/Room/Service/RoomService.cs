using AutoMapper;
using Cinema.Model;
using Cinema.Module.Room.DTO;
using Cinema.Module.Room.Repository;
using Cinema.Module.Seat.DTO;
using Cinema.Module.Seat.Service;

namespace Cinema.Module.Room.Service
{
    public class RoomService : IRoomService
    {

        private readonly IRoomRepository _roomRepository;

        private readonly ISeatService _seatService;

        private readonly IMapper _mapper;

        public RoomService(IRoomRepository roomRepository, ISeatService seatService, IMapper mapper)
        {
            _roomRepository = roomRepository;
            _seatService = seatService;
            _mapper = mapper;
        }

        public RoomDTO AddRoom(RoomDTO room)
        {
            RoomDTO roomDTO = _mapper.Map<RoomDTO>(_roomRepository.AddRoom(_mapper.Map<RoomModel>(room)));
            foreach(SeatDTO seat in roomDTO.Seats)
            {
                seat.RoomId = roomDTO.Id;
                roomDTO.Seats.Add(_seatService.AddSeat(seat));
            }
            return roomDTO;
        }

        public void DeleteRoom(int id)
        {
            RoomModel room = _roomRepository.GetRoom(id);
            room.RoomStatus = Enum.RoomStatus.DELETED;
            _roomRepository.UpdateRoom(room);
        }

        public RoomDTO GetRoom(int id)
        {
            RoomDTO roomDTO = _mapper.Map<RoomDTO>(_roomRepository.GetRoom(id));
            roomDTO.Seats = _seatService.GetSeatByRoomId(roomDTO.Id);
            return roomDTO;
        }

        public List<RoomDTO> GetRooms()
        {
            List<RoomModel> rooms = _roomRepository.GetRooms();
            List<RoomDTO> result = new List<RoomDTO>();
            foreach(RoomModel room in rooms)
            {
                result.Add(GetRoom(room.Id));
            }
            return result;
        }

        public RoomDTO UpdateRoom(RoomDTO room)
        {
            throw new NotImplementedException();
        }
    }
}
