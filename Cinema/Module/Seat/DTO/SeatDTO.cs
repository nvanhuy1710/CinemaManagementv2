using Cinema.Module.Room.DTO;
using Cinema.Module.SeatType.DTO;

namespace Cinema.Module.Seat.DTO
{
    public class SeatDTO
    {
        public int Id { get; set; }

        public string Postion { get; set; }

        public SeatTypeDTO SeatType { get; set; }

        public int SeatTypeId { get; set; }

        public int RoomId { get; set; }
    }
}
