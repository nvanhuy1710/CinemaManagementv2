using Cinema.Enum;
using Cinema.Module.Seat.DTO;

namespace Cinema.Module.Room.DTO
{
    public class RoomDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Row { get; set; }

        public int Col { get; set; }

        public RoomStatus? RoomStatus { get; set; }

        public List<SeatDTO> Seats { get; set; }
    }
}
