using Cinema.Enum;
using Cinema.Module.Genre.DTO;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Module.Film.DTO
{
    public class FilmDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Country { get; set; }

        public int Length { get; set; }

        public string Content { get; set; }

        public string Director { get; set; }

        public string Actor { get; set; }

        public int? AgeLimit { get; set; }

        public string Trailer { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string? PosterUrl { get; set; }

        public string? AdPosterUrl { get; set; }

        public FilmStatus FilmStatus { get; set; }

        public List<GenreDTO> Genres { get; set; }
    }
}
