using AutoMapper;
using Cinema.Model;
using Cinema.Module.SeatType.DTO;
using Cinema.Module.SeatType.Repository;

namespace Cinema.Module.SeatType.Service
{
    public class SeatTypeService : ISeatTypeService
    {
        private readonly ISeatTypeRepository _seatTypeRepository;

        private readonly IMapper _mapper;

        public SeatTypeService(ISeatTypeRepository seatTypeRepository, IMapper mapper)
        {
            _seatTypeRepository = seatTypeRepository;
            _mapper = mapper;
        }

        public SeatTypeDTO AddSeatType(SeatTypeDTO seatType)
        {
            return _mapper.Map<SeatTypeDTO>(_seatTypeRepository.AddSeatType(_mapper.Map<SeatTypeModel>(seatType)));
        }

        public void DeleteSeatType(int id)
        {
            _seatTypeRepository.DeleteSeatType(id);
        }

        public List<SeatTypeDTO> GetSeatTypes()
        {
            return _seatTypeRepository.GetSeatTypes().Select(p => _mapper.Map<SeatTypeDTO>(p)).ToList();
        }

        public SeatTypeDTO GetSeatType(int id)
        {
            return _mapper.Map<SeatTypeDTO>(_seatTypeRepository.GetSeatType(id));
        }

        public SeatTypeDTO UpdateSeatType(SeatTypeDTO seatType)
        {
            return _mapper.Map<SeatTypeDTO>(_seatTypeRepository.UpdateSeatType(_mapper.Map<SeatTypeModel>(seatType)));
        }
    }
}
