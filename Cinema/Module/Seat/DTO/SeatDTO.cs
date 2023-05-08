using Cinema.Module.Room.DTO;
using Cinema.Module.SeatType.DTO;

namespace Cinema.Module.Seat.DTO
{
    public class SeatDTO
    {
        public int Id { get; set; }

        public string Position { get; set; }

        public SeatTypeDTO? SeatTypeDTO { get; set; }

        public int SeatTypeId { get; set; }

        public int RoomId { get; set; }

        public bool? IsBooked { get; set; } = false;
    }
}
