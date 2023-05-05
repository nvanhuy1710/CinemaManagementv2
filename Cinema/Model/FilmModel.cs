using Cinema.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinema.Model
{
    [Table("Film")]
    public class FilmModel
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Country { get; set; }

        public string? Content { get; set; }

        public int Length { get; set; }

        public string Director { get; set; }

        public string? Trailer { get; set; }

        public string Actor { get; set; }

        public int? AgeLimit { get; set; }

        public string PosterUrl { get; set; }

        public string AdPosterUrl { get; set; }

        public DateTime ReleaseDate { get; set; }

        [EnumDataType(typeof(string))]
        public FilmStatus FilmStatus { get; set; }

        public virtual ICollection<FilmGenreModel> FilmGenreModels { get; set; }

        public virtual ICollection<ShowModel> ShowModels { get; set; }

        public FilmModel()
        {
            this.FilmGenreModels = new HashSet<FilmGenreModel>();
            this.ShowModels = new HashSet<ShowModel>();
        }
    }
}
