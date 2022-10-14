using PPS.Data.Entities.GroupOfCompany;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PPS.Data.Entities.Ledger
{
    public class AccountPrimaryHead : BaseEntity
    {
        [Required]
        [StringLength(100)]
        [Column(TypeName = "VARCHAR")]
        public string AccountPrimaryHeadName { get; set; }

        [Required]
        [ForeignKey("AccountType")]
        public int AccountTypeId { get; set; }
        public virtual AccountType AccountType { get; set; }

        //[Required]
        //[StringLength(2)]
        //[Column(TypeName = "VARCHAR")]
        //public string Type { get; set; }

        [Required]
        public int CompanyId { get; set; }

        public virtual ICollection<AccountHead> AccountHead { get; set; }
    }
}