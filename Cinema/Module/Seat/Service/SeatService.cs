using AutoMapper;
using Cinema.Helper;
using Cinema.Model;
using Cinema.Module.Seat.DTO;
using Cinema.Module.Seat.Repository;

namespace Cinema.Module.Seat.Service
{
    public class SeatService : ISeatService
    {

        private readonly ISeatRepository _seatRepository;

        private readonly IMapper _mapper;

        public SeatService(ISeatRepository seatRepository, IMapper mapper)
        {
            _seatRepository = seatRepository;
            _mapper = mapper;
        }

        public SeatDTO AddSeat(SeatDTO seatDTO)
        {
            return _mapper.Map<SeatDTO>(_seatRepository.AddSeat(_mapper.Map<SeatModel>(seatDTO)));
        }

        public List<SeatDTO> AddSeats(List<SeatDTO> seats)
        {
            List<SeatDTO> result = new List<SeatDTO>();
            foreach(SeatDTO seat in seats)
            {
               result.Add(AddSeat(seat));
            }
            return result;
        }

        public void DeleteSeat(int id)
        {
            _seatRepository.DeleteSeat(id);
        }

        public SeatDTO GetSeat(int id)
        {
            return _mapper.Map<SeatDTO>(_seatRepository.GetSeat(id));
        }

        public List<SeatDTO> GetSeatByRoomId(int roomId)
        {
            return _seatRepository.GetSeatsByRoomId(roomId).Select(p => _mapper.Map<SeatDTO>(p)).ToList();
        }

        public List<SeatDTO> GetSeats()
        {
            return _seatRepository.GetSeats().Select(p => _mapper.Map<SeatDTO>(p)).ToList();
        }

        public SeatDTO UpdateSeat(SeatDTO seatDTO)
        {
            return _mapper.Map<SeatDTO>(_seatRepository.UpdateSeat(_mapper.Map<SeatModel>(seatDTO)));
        }
    }
}
