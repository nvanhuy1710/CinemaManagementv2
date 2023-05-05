using Cinema.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinema.Model
{
    [Table("Room")]
    public class RoomModel
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public int Row { get; set; }

        public int Col { get; set; }

        [EnumDataType(typeof(string))]
        public RoomStatus RoomStatus { get; set; }

        public virtual ICollection<SeatModel> SeatModels { get; set; }

        public virtual ICollection<ShowModel> ShowModels { get; set; }

        public RoomModel()
        {
            this.SeatModels = new HashSet<SeatModel>();
            this.ShowModels = new HashSet<ShowModel>();
        }
    }
}
