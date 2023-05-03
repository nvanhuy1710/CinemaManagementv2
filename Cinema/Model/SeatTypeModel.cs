using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Model
{
    [Table("SeatType")]
    public class SeatTypeModel
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public int Cost { get; set; }

        public virtual ICollection<SeatModel> Seats { get; set; }

        public SeatTypeModel() 
        {
            this.Seats = new HashSet<SeatModel>();
        }
    }
}
