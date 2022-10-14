using System;
using System.Collections.Generic;

namespace PPS.API.Shared.ViewModel.Report
{
    public class TransactionEntryVm
    {
        public TransactionEntryVm()
        {
            this.TransactionDetail = new HashSet<TransactionDetailVm>();
        }

        public int Id { get; set; }
        public string TransactionNumber { get; set; }
        public DateTime TransactionDate { get; set; }
        public int FiscalYear { get; set; }
        public int TransactionTypeId { get; set; }
        public int CompanyId { get; set; }
        public DateTime PostingDate { get; set; }
        public bool Active { get; set; }
        public bool? Deleted { get; set; }
        public bool Accepted { get; set; }
        public int? AcceptedById { get; set; }
        public DateTime? AcceptedDate { get; set; }
        public int? PreviousId { get; set; }
        public string UpdateReason { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdatedById { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Particulars { get; set; }
        public int? RejectedById { get; set; }
        public int? RejectedReasonTypeId { get; set; }
        public DateTime? RejectedDate { get; set; }
        public bool IsSystemGenerated { get; set; }

        //public virtual Company Company { get; set; }
        public virtual ICollection<TransactionDetailVm> TransactionDetail { get; set; }
        //public virtual TransactionType TransactionType { get; set; }
    }
}