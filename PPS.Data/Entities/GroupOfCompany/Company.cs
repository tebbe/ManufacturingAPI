using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PPS.Data.Entities.GroupOfCompany;
using System.Collections;
using System.Collections.Generic;
using PPS.Data.Entities.Transaction;
using PPS.Data.Entities.Account;
using PPS.Data.Entities.Ledger;

namespace PPS.Data.Entities.GroupOfCompany
{
    public class Company
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        [Column(TypeName = "NVARCHAR")]
        public string Name { get; set; }

        [StringLength(50)]
        [Column(TypeName = "VARCHAR")]
        public string ContactPerson { get; set; }

        [StringLength(50)]
        [Column(TypeName = "VARCHAR")]
        public string ContactNumber { get; set; }

        [Required]
        [StringLength(200)]
        [Column(TypeName = "VARCHAR")]
        public string Address { get; set; }

        [StringLength(50)]
        [Column(TypeName = "VARCHAR")]
        public string Phone { get; set; }

        [StringLength(50)]
        [Column(TypeName = "VARCHAR")]
        public string Fax { get; set; }

        [StringLength(500)]
        [Column(TypeName = "VARCHAR")]
        public string LogoPath { get; set; }

        [Required]
        [ForeignKey("Group")]
        public int GroupId { get; set; }

        public Group Group { get; set; }

        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<TransactionEntry> TransactionEntriy { get; set; }

        //public virtual ICollection<AccountNature> AccountNature { get; set; }
        //public virtual ICollection<AccountType> AccountType { get; set; }
        //public virtual ICollection<AccountPrimaryHead> AccountPrimaryHead { get; set; }
        //public virtual ICollection<AccountHead> AccountHead { get; set; }
        //public virtual ICollection<AccountHeadOpening> AccountHeadOpening { get; set; }
    }
}