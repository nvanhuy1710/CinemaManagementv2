using Cinema.Model;

namespace Cinema.Module.Seat.Repository
{
    public interface ISeatRepository
    {
        SeatModel AddSeat(SeatModel seat);

        SeatModel UpdateSeat(SeatModel seat);

        SeatModel GetSeat(int id);

        List<SeatModel> GetSeats();

        List<SeatModel> GetSeatsByRoomId(int id);

        void DeleteSeat(int id);
    }
}
