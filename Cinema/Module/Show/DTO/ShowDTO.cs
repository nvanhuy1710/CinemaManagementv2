using Cinema.Model;
using Cinema.Module.Film.DTO;
using Cinema.Module.Room.DTO;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinema.Module.Show.DTO
{
    public class ShowDTO
    {
        public int Id { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime StartTime { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime EndTime { get; set; }

        public string? FilmName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Director { get; set; }

        public string? RoomName { get; set; }

        public string? Poster { get; set; }

        public int FilmId { get; set; }

        public int? AgeLimit { get; set; }

        public int RoomId { get; set; }
    }
}
