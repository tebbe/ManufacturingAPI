using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PPS.Data.Entities.Transaction;

namespace PPS.Data.Entities.TaxInfo
{
    public class Tax : BaseEntity
    {
        [Required]
        [StringLength(15)]
        [Column(TypeName = "VARCHAR")]
        public string TaxName { get; set; }
        public double TaxPercent { get; set; }

        public ICollection<TransactionDetail> TransactionDetail { get; set; }
    }
}