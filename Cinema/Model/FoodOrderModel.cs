using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinema.Model
{
    [Table("Food_Order")]
    public class FoodOrderModel
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int FoodId { get; set; }

        public int BillId { get; set; }

        public int Count { get; set; }

        [ForeignKey("BillId")]
        public virtual BillModel BillModel { get; set; }

        [ForeignKey("FoodId")]
        public virtual FoodModel FoodModel { get; set; }
    }
}
