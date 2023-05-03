using Cinema.Module.Seat.DTO;

namespace Cinema.Module.Seat.Service
{
    public interface ISeatService
    {
        SeatDTO AddSeat(SeatDTO seatDTO);

        List<SeatDTO> AddSeats(List<SeatDTO> seats);

        SeatDTO GetSeat(int id);

        List<SeatDTO> GetSeats();

        List<SeatDTO> GetSeatByRoomId(int roomId);

        SeatDTO UpdateSeat(SeatDTO seatDTO);

        void DeleteSeat(int id);
    }
}
