using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Cinema.Enum;

namespace Cinema.Model
{
    [Table("Bill")]
    public class BillModel
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }

        public DateTime DatePurchased { get; set; }

        [EnumDataType(typeof(string))]
        public BillStatus BillStatus { get; set; }

        [ForeignKey("UserId")]
        public virtual UserModel UserModel { get; set; }

        public virtual ICollection<FoodOrderModel> FoodOrderModels { get; set; }

        public virtual ICollection<ReservationModel> ReservationModels { get; set; }

        public BillModel()
        {
            this.FoodOrderModels = new HashSet<FoodOrderModel>();
            this.ReservationModels = new HashSet<ReservationModel>();
        }
    }
}
