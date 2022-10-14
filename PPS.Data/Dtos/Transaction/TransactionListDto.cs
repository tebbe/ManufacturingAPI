using System;
using PPS.API.Shared.ViewModel.Transaction;

namespace PPS.Data.Dtos.Transaction
{
    public class TransactionListDto
    {        
        public int TranId { get; set; }
        public string TranNo { get; set; }
        public DateTime TranDate { get; set; }
        public DateTime PostingDate { get; set; }
        public string Particulars { get; set; }
        public int TranTypeId { get; set; }
        public string TranType { get; set; }
        public int CompanyId { get; set; }
        public int FiscalYear { get; set; }
        public bool Active { get; set; }
        public double TranAmount { get; set; }
        public int? VerifiedById { get; set; }
        public string VerifiedByName { get; set; }
        public DateTime? VerifiedDate { get; set; }
        public bool Accepted { get; set; }
        public int? AcceptedById { get; set; }        
        public string AcceptedByName { get; set; }
        public DateTime? AcceptedDate { get; set; }
        public int? RejectedById { get; set; }
        public string RejectedByName { get; set; }
        public DateTime? RejectedDate { get; set; }
        public int CreatedById { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdatedById { get; set; }
        public string UpdatedByName { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Status { get; set; }
        public string RejectedReasonTypeName { get; set; }
        public bool HasHistory { get; set; }

        public TransactionListVm ToTransactionVm()
        {
            var vm = new TransactionListVm
            {
                TranId = this.TranId,
                TranNo = this.TranNo,
                TranDate = this.TranDate,
                PostingDate = this.PostingDate,
                Particulars = this.Particulars,
                TranTypeId = this.TranTypeId,
                TranType = this.TranType,
                CompanyId = this.CompanyId,
                FiscalYear = this.FiscalYear,
                Active = this.Active,
                TranAmount = this.TranAmount,
                Accepted = this.Accepted,
                AcceptedById = this.AcceptedById,
                AcceptedByName = this.AcceptedByName,
                AcceptedDate = this.AcceptedDate,
                VerifiedById = this.VerifiedById,
                VerifiedByName = this.VerifiedByName,
                VerifiedDate = this.VerifiedDate,
                RejectedById = this.RejectedById,
                RejectedByName = this.RejectedByName,
                RejectedDate = this.RejectedDate,
                CreatedById = this.CreatedById,
                CreatedByName = this.CreatedByName,
                CreatedDate = this.CreatedDate,
                UpdatedById = this.UpdatedById,
                UpdatedByName = this.UpdatedByName,
                UpdatedDate = this.UpdatedDate,
                RejectedReasonTypeName = this.RejectedReasonTypeName,
                Status = this.Status,
                HasHistory = this.HasHistory
            };

            return vm;
        }
    }
    
}