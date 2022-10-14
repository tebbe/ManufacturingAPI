using PPS.Data.Entities.GroupOfCompany;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PPS.Data.Entities.Ledger
{
    public class AccountType : BaseEntity
    {
        [Required]
        [StringLength(100)]
        [Column(TypeName = "VARCHAR")]
        public string AccountTypeName { get; set; }

        [Required]
        [ForeignKey("AccountNature")]
        public int AccountNatureId { get; set; }
        public virtual AccountNature AccountNature { get; set; }

        [Required]
        public int CompanyId { get; set; }

        public virtual ICollection<AccountPrimaryHead> AccountPrimaryHead { get; set; }
    }
}