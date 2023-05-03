using Cinema.Model;

namespace Cinema.Module.SeatType.Repository
{
    public interface ISeatTypeRepository
    {
        SeatTypeModel GetSeatType(int id);

        List<SeatTypeModel> GetSeatTypes();

        SeatTypeModel AddSeatType(SeatTypeModel seatType);

        SeatTypeModel UpdateSeatType(SeatTypeModel seatType);

        void DeleteSeatType(int id);
    }
}
