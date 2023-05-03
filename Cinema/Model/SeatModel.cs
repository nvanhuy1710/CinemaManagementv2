using Cinema.Enum;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Model
{
    public class SeatModel
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Position { get; set; }

        public int SeatTypeId { get; set; }

        public int RoomId { get; set; }

        [ForeignKey("RoomId")]
        public virtual RoomModel Room { get; set; }

        [ForeignKey("SeatTypeId")]
        public virtual SeatTypeModel SeatType { get; set; }
    }
}
