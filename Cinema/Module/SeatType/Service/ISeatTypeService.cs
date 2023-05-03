using Cinema.Module.SeatType.DTO;

namespace Cinema.Module.SeatType.Service
{
    public interface ISeatTypeService
    {
        SeatTypeDTO GetSeatType(int id);

        List<SeatTypeDTO> GetSeatTypes();

        SeatTypeDTO AddSeatType(SeatTypeDTO seatType);

        SeatTypeDTO UpdateSeatType(SeatTypeDTO seatType);

        void DeleteSeatType(int id);
    }
}
