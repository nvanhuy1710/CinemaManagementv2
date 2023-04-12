using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Model
{
    [Table("User")]
    public class UserModel
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Gender { get; set; }

        [Column(TypeName = "date")]
        public DateTime Birth { get; set; }

        public string Phone { get; set; }

        public string? Address { get; set; }

        [Column("Account_Id")]
        public int AccountId { get; set; }

        [ForeignKey("AccountId")]
        public virtual AccountModel Account { get; set; }
    }
}
