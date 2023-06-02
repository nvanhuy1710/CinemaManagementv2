using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinema.Model
{
    [Table("Food")]
    public class FoodModel
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public int Cost { get; set; }

        public virtual ICollection<FoodOrderModel> FoodOrderModels { get; set; }

        public FoodModel()
        {
            this.FoodOrderModels = new HashSet<FoodOrderModel>();
        }
    }
}
