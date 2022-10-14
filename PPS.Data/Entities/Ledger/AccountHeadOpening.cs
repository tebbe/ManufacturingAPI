using PPS.Data.Entities.GroupOfCompany;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PPS.Data.Entities.Ledger
{
    public class AccountHeadOpening : BaseEntity
    {
        [Required]
        [ForeignKey("AccountHead")]
        public int AccountHeadId { get; set; }
        public virtual AccountHead AccountHead { get; set; }

        [Required]
        public double DrAmount { get; set; }

        [Required]
        public double CrAmount { get; set; }

        [Required]
        public int FiscalYear { get; set; }

        [Required]
        public int CompanyId { get; set; }
    }
}