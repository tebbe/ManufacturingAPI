using PPS.Data.Entities.GroupOfCompany;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PPS.Data.Entities.Transaction
{
    public class TransactionEntry : BaseEntity
    {
        [Required]
        [StringLength(12)]
        [Column(TypeName = "VARCHAR")]
        public string TransactionNumber { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        public int FiscalYear { get; set; }

        [Required]
        [ForeignKey("TransactionType")]
        public int TransactionTypeId { get; set; }
        public virtual TransactionType TransactionType { get; set; }

        //[Required]
        //[ForeignKey("ControlType")]
        //public int ControlTypeId { get; set; }
        //public virtual ControlType ControlType { get; set; }

        [Required]
        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public int? PostingNumber { get; set; }
        public DateTime? PostingDate { get; set; }

        //[Required]
        //[ForeignKey("TransactionStatus")]
        //public int TransactionStatusId { get; set; }
        //public virtual TransactionStatus TransactionStatus { get; set; }

        [StringLength(250)]
        [Column(TypeName = "VARCHAR")]
        public string Particulars { get; set; }

        [StringLength(250)]
        [Column(TypeName = "VARCHAR")]
        public string UpdateReason { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        public bool Accepted { get; set; }
        public int? AcceptedBy { get; set; }
        public DateTime? AcceptedDate { get; set; }

        [StringLength(250)]
        [Column(TypeName = "VARCHAR")]
        public string DeleteReason { get; set; }

        public int? PreviousId { get; set; }

        public virtual ICollection<TransactionDetail> TransactionDetail { get; set; }
    }
}