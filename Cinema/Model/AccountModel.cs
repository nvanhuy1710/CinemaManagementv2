﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Cinema.Enum;
using System.Reflection;

namespace Cinema.Model
{
    [Table("Account")]
    public class AccountModel
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        [EnumDataType(typeof(AccountStatus))]
        public AccountStatus accountStatus { get; set; }

        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        public virtual RoleModel Role { get; set; }
    }
}
