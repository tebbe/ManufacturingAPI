using PPS.Data.Entities.GroupOfCompany;
using PPS.Data.Entities.Ledger;
using PPS.Data.Entities.Transaction;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PPS.Data.Entities.Account
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Column(TypeName = "VARCHAR")]
        public string Email { get; set; }

        [Required]
        [StringLength(200)]
        [Column(TypeName = "VARCHAR")]
        public string Password { get; set; }

        [Required]
        [StringLength(50)]
        [Column(TypeName = "VARCHAR")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [Column(TypeName = "VARCHAR")]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        [Column(TypeName = "VARCHAR")]
        public string Phone { get; set; }

        [Required]
        [ForeignKey("Company")]
        public int CompanyId { get; set; }

        public virtual Company Company { get; set; }

        [Required]
        [ForeignKey("Status")]
        public int StatusId { get; set; }

        public virtual UserStatus Status { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<TransactionEntry> UpdatedBy1 { get; set; }
        public virtual ICollection<TransactionEntry> CreatedBy1 { get; set; }
        public virtual ICollection<AccountHead> UpdatedBy { get; set; }
        public virtual ICollection<AccountHead> CreatedBy { get; set; }
    }
}