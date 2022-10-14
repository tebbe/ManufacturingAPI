using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PPS.Data.Entities.Transaction
{
    public class ControlType : BaseEntity
    {
        [Required]
        [StringLength(50)]
        [Column(TypeName = "VARCHAR")]
        public string Type { get; set; }

        //public virtual ICollection<TransactionEntry> TransactionEntries { get; set; }
    }
}