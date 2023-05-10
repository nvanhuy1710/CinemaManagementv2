using AutoMapper;
using Cinema.Helper;
using Cinema.Model;
using Cinema.Module.Reservation.Repository;
using Cinema.Module.Seat.DTO;
using Cinema.Module.Seat.Repository;
using Cinema.Module.SeatType.DTO;

namespace Cinema.Module.Seat.Service
{
    public class SeatService : ISeatService
    {

        private readonly ISeatRepository _seatRepository;

        private readonly IReservationRepository _reservationRepository;

        private readonly IMapper _mapper;

        public SeatService(ISeatRepository seatRepository, IMapper mapper, IReservationRepository reservationRepository)
        {
            _seatRepository = seatRepository;
            _mapper = mapper;
            _reservationRepository = reservationRepository;
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
            List<ReservationModel> reservationModels = _reservationRepository.GetReservationBySeatId(id);
            foreach(ReservationModel reservationModel in reservationModels)
            {
                reservationModel.SeatId = null;
                _reservationRepository.UpdateReservation(reservationModel);
            }
            _seatRepository.DeleteSeat(id);
        }

        public SeatDTO GetSeat(int id)
        {
            SeatModel seatModel = _seatRepository.GetSeat(id);
            SeatDTO seatDTO = _mapper.Map<SeatDTO>(seatModel);
            seatDTO.SeatTypeDTO = _mapper.Map<SeatTypeDTO>(seatModel.SeatType);
            return seatDTO;
        }

        public List<SeatDTO> GetSeatByRoomId(int roomId)
        {
            List<SeatModel> seatModels = _seatRepository.GetSeatsByRoomId(roomId);
            List<SeatDTO> seatDTOs = new List<SeatDTO>();
            foreach(SeatModel seatModel in seatModels)
            {
                SeatDTO seatDTO = _mapper.Map<SeatDTO>(seatModel);
                seatDTO.SeatTypeDTO = _mapper.Map<SeatTypeDTO>(seatModel.SeatType);
                seatDTOs.Add(seatDTO);
            }
            return seatDTOs;
        }

        public List<SeatDTO> GetSeatForBook(int roomId, int showId)
        {
            List<ReservationModel> reservationModels = _reservationRepository.GetReservationByShowId(showId);
            List<SeatDTO> seatDTOs = GetSeatByRoomId(roomId);
            foreach(ReservationModel reservationModel in reservationModels)
            {
                foreach(SeatDTO seatDTO in seatDTOs)
                {
                    if (seatDTO.Id == reservationModel.SeatId) seatDTO.IsBooked = true;
                }
            }
            return seatDTOs;
        }

        public List<SeatDTO> GetSeats()
        {
            List<SeatModel> seatModels = _seatRepository.GetSeats();
            List<SeatDTO> seatDTOs = new List<SeatDTO>();
            foreach (SeatModel seatModel in seatModels)
            {
                SeatDTO seatDTO = _mapper.Map<SeatDTO>(seatModel);
                seatDTO.SeatTypeDTO = _mapper.Map<SeatTypeDTO>(seatModel.SeatType);
                seatDTOs.Add(seatDTO);
            }
            return seatDTOs;
        }

        public SeatDTO UpdateSeat(SeatDTO seatDTO)
        {
            return _mapper.Map<SeatDTO>(_seatRepository.UpdateSeat(_mapper.Map<SeatModel>(seatDTO)));
        }
    }
}
