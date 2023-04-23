using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinema.Model
{
    [Table("Genre")]
    public class GenreModel
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<FilmGenreModel> FilmGenreModels { get; set; }

        public GenreModel() 
        {
            this.FilmGenreModels = new HashSet<FilmGenreModel>();
        }
    }
}
