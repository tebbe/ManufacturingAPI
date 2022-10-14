using PPS.Data.Entities.GroupOfCompany;
using PPS.Data.Entities.Transaction;
using PPS.Data.Entities.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PPS.Data.Entities.Ledger
{
    public class AccountHead : BaseEntity
    {
        public AccountHead()
        {
            TransactionDetail = new HashSet<TransactionDetail>();
            AccountHeadOpening = new HashSet<AccountHeadOpening>();
        }
        [StringLength(10)]
        [Column(TypeName = "VARCHAR")]
        public string AccountHeadCode { get; set; }

        [StringLength(100)]
        [Column(TypeName = "VARCHAR")]
        public string AccountHeadName { get; set; }
               
        [Required]
        [ForeignKey("AccountPrimaryHead")]
        public int AccountPrimaryHeadId { get; set; }
        public virtual AccountPrimaryHead AccountPrimaryHead { get; set; }

        public bool Active { get; set; }

        [StringLength(2)]
        [Column(TypeName = "VARCHAR")]
        public string LedgerType { get; set; }

        [Required]
        public int CompanyId { get; set; }

        public virtual ICollection<TransactionDetail> TransactionDetail { get; set; }
        public virtual ICollection<AccountHeadOpening> AccountHeadOpening { get; set; }
    }
}