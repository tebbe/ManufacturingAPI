using PPS.Data.Entities.Ledger;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PPS.Data.Entities.Transaction
{
    public class TransactionDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("AccountHead")]
        public int AccountHeadId { get; set; }
        public virtual AccountHead AccountHead { get; set; }

        [Required]
        public double DrAmount { get; set; }

        [Required]
        public double CrAmount { get; set; }

        

        //public string OppLedgerId { get; set; }

        //public bool Active { get; set; }

        //public int? PreviousId { get; set; }

        [Required]
        [ForeignKey("TransactionEntry")]
        public int TransactionEntryId { get; set; }
        public virtual TransactionEntry TransactionEntry { get; set; }
    }
}