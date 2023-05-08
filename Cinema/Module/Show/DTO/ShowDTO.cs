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

        public string FilmName { get; set; }

        public string Poster { get; set; }

        public int FilmId { get; set; }

        public int AgeLimit { get; set; }

        public int RoomId { get; set; }
    }
}
