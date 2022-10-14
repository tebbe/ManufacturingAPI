using PPS.Data.Entities.GroupOfCompany;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PPS.Data.Entities.Ledger
{
    public class AccountNature : BaseEntity
    {
        [Required]
        [StringLength(100)]
        [Column(TypeName = "VARCHAR")]
        public string AccountNatureName { get; set; }

        [Required]
        public int CompanyId { get; set; }

        public virtual ICollection<AccountType> AccountType { get; set; }
    }
}