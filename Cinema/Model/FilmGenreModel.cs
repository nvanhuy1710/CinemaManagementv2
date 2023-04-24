using Cinema.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinema.Model
{
    [Table("Film_Genre")]
    public class FilmGenreModel
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int FilmId { get; set; }

        public int GenreId { get; set; }

        [ForeignKey("FilmId")]
        public virtual FilmModel Film { get; set; }

        [ForeignKey("GenreId")]
        public virtual GenreModel Genre { get; set; }
    }
}
