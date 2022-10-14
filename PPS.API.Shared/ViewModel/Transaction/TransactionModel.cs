using System;
using System.Collections.Generic;

namespace PPS.API.Shared.ViewModel.Transaction
{
    public class TransactionModel
    {
        public TransactionModel()
        {
            TransactionDetail = new HashSet<TransactionDetailModel>();
        }
        public int TranId { get; set; }
        public string TranNo { get; set; }
        public DateTime TranDate { get; set; }
        public DateTime PostingDate { get; set; }
        public string Particulars { get; set; }
        public int TranTypeId { get; set; }
        public int CompanyId { get; set; }
        public int FiscalYear { get; set; }
        public bool Active { get; set; }
        public double TranAmount { get; set; }
        public string UpdateReason { get; set; }
        public bool Accepted { get; set; }
        public int? AcceptedById { get; set; }        
        public string AcceptedByName { get; set; }
        public DateTime? AcceptedDate { get; set; }
        public int CreatedById { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdatedById { get; set; }
        public string UpdatedByName { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool Deleted { get; set; }
        public string DeletedReason { get; set; }
        public string Status { get; set; }
        public int? VerifiedById { get; set; }
        public string VerifiedByName { get; set; }
        public DateTime? VerifiedDate { get; set; }
        public ICollection<TransactionDetailModel> TransactionDetail { get; set; }
        public bool HasHistory { get; set; }
    }
    
}