using Cinema.Model;
using Cinema.Module.Film.DTO;
using Cinema.Module.Room.DTO;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinema.Module.Show.DTO
{
    public class ShowDTO
    {
        public int Id { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int FilmId { get; set; }

        public int RoomId { get; set; }

        public FilmDTO? FilmDTO { get; set; }

        public RoomDTO? RoomDTO { get; set; }
    }
}
