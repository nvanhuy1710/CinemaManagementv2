using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Model
{
    [Table("Show")]
    public class ShowModel
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int FilmId { get; set; }

        public int RoomId { get; set; }

        public bool IsDeleted { get; set; }

        [ForeignKey("FilmId")]
        public virtual FilmModel Film { get; set; }

        [ForeignKey("RoomId")]
        public virtual RoomModel Room { get; set; }
    }
}
