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
            if(!CheckExistName(room.Name))
            {
                throw new InvalidOperationException($"Name: '{room.Name}' existed!");
            }
            room.RoomStatus = Enum.RoomStatus.READY;
            RoomDTO roomDTO = _mapper.Map<RoomDTO>(_roomRepository.AddRoom(_mapper.Map<RoomModel>(room)));
            List<SeatDTO> seatDTOs = new List<SeatDTO>();
            foreach (SeatDTO seat in room.Seats)
            {
                seat.RoomId = roomDTO.Id;
                seatDTOs.Add(_seatService.AddSeat(seat));
            }
            roomDTO.Seats = seatDTOs;
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
            if (!CheckExistName(room.Name))
            {
                throw new InvalidOperationException($"Name: '{room.Name}' existed!");
            }
            throw new NotImplementedException();
        }

        private bool CheckExistName(string name)
        {
            if(_roomRepository.GetRoom(name) != null)
            {
                return false;
            }
            return true;
        }
    }
}
