using Cinema.Model;

namespace Cinema.Module.Reservation.Repository
{
    public interface IReservationRepository
    {

        ReservationModel AddReservation(ReservationModel reservation);

        List<ReservationModel> GetReservationByShowId(int showId);

        List<ReservationModel> GetReservationByBillId(int billId);

        List<ReservationModel> GetReservationBySeatId(int seatId);

        ReservationModel UpdateReservation(ReservationModel reservation);
    }
}
