using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Model
{
    [Table("Role")]
    public class RoleModel
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<AccountModel> AccountModels { get; set; }

        public RoleModel()
        {
            this.AccountModels = new HashSet<AccountModel>();
        }
    }
}
