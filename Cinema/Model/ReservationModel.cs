using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinema.Model
{
    [Table("Reservation")]
    public class ReservationModel
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int BillId { get; set; }

        public int? SeatId { get; set; }

        public int ShowId { get; set; }

        public string SeatTypeName { get; set; }

        public int Cost { get; set; }

        [ForeignKey("BillId")]
        public virtual BillModel BillModel { get; set; }

        [ForeignKey("SeatId")]
        public virtual SeatModel SeatModel { get; set; }

        [ForeignKey("ShowId")]
        public virtual ShowModel ShowModel { get; set; }
    }
}
