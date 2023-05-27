namespace Cinema.Module.Reservation.DTO
{
    public class ReservationDTO
    {
        public int Id { get; set; }

        public int BillId { get; set; }

        public int SeatId { get; set; }

        public int ShowId { get; set; }

        public string SeatTypeName { get; set; }

        public int Cost { get; set; }
    }
}
