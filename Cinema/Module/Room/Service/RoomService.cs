using AutoMapper;
using Cinema.Enum;
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
            if(_roomRepository.GetRoom(room.Name) != null)
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

        public RoomDTO UpdateRoom(RoomDTO room)
        {
            if(GetRoom(room.Id).RoomStatus != Enum.RoomStatus.REPAIRING)
            {
                throw new InvalidOperationException("The room must be in a state of repairing to be updated!");
            }
            RoomModel oldRoom = _roomRepository.GetRoom(room.Name);
            if (oldRoom != null && oldRoom.Name == room.Name && oldRoom.Id != room.Id)
            {
                throw new InvalidOperationException($"Name: '{room.Name}' existed!");
            }
            foreach(SeatDTO seatDTO in _seatService.GetSeatByRoomId(room.Id))
            {
                _seatService.DeleteSeat(seatDTO.Id);
            }
            RoomDTO roomDTO = _mapper.Map<RoomDTO>(_roomRepository.UpdateRoom(_mapper.Map<RoomModel>(room)));
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
            List<RoomDTO> result = _roomRepository.GetRooms().Select(p => GetRoom(p.Id)).ToList();
            return result;
        }

        public void ChangeStatusRoom(int roomId, RoomStatus roomStatus)
        {
            if (_roomRepository.ChangeStatusRoom(roomId, roomStatus) == null) throw new InvalidOperationException("Room have showtime!");
        }
    }
}
