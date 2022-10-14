using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PPS.API.Shared.ViewModel.Transaction;

namespace PPS.Data.Dtos.Transaction
{
    public class TransactionDto
    {
        public TransactionDto()
        {
            TransactionDetail = new List<TransactionDetailDto>();
        }
        public int TranId { get; set; }
        public string TranNo { get; set; }
        public int TranTypeId { get; set; }
        //public int TranStatusId { get; set; }
        public int CompanyId { get; set; }
        public int FiscalYear { get; set; }
        public DateTime TranDate { get; set; }
        public DateTime PostingDate { get; set; }
        //public string TranHead { get; set; }
        public string Particulars { get; set; }
        public bool Active { get; set; }
        public double TranAmount { get; set; }
        //public double DrAmount { get; set; }
        //public double CrAmount { get; set; }
        public string UpdateReason { get; set; }
        //public int CreatedBy { get; set; }
        public bool Accepted { get; set; }
        public string AcceptedByName { get; set; }
        public DateTime? AcceptedDate { get; set; }
        public int? AcceptedById { get; set; }
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
        public DateTime? VerifiedDate { get; set; }
        public string VerifiedByName { get; set; }
        public bool HasHistory { get; set; }
        public ICollection<TransactionDetailDto> TransactionDetail { get; set; }

        public TransactionModel ToTransactionModel()
        {
            var vm = new TransactionModel
            {
                TranId = this.TranId,
                TranNo = this.TranNo,
                TranDate = this.TranDate,
                PostingDate = this.PostingDate,
                TranTypeId = this.TranTypeId,
                CompanyId = this.CompanyId,
                FiscalYear = this.FiscalYear,
                Particulars = this.Particulars,
                TranAmount = this.TranAmount,
                Active = this.Active,
                UpdateReason = this.UpdateReason,
                Accepted = this.Accepted,
                AcceptedByName = this.AcceptedByName,
                AcceptedDate = this.AcceptedDate,
                CreatedById = this.CreatedById,
                CreatedByName = this.CreatedByName,
                CreatedDate = this.CreatedDate,
                VerifiedById = this.VerifiedById,
                VerifiedByName = this.VerifiedByName,
                VerifiedDate = this.VerifiedDate,
                UpdatedById = this.UpdatedById,
                UpdatedByName = this.UpdatedByName,
                UpdatedDate = this.UpdatedDate,
                Deleted = this.Deleted,
                DeletedReason = this.DeletedReason,
                Status = this.Status,
                HasHistory = this.HasHistory
            };

            vm.TransactionDetail = new List<TransactionDetailModel>();

            foreach(var tran in TransactionDetail)
            {
                var vmTranDetail = new TransactionDetailModel
                {
                    TranHeadId = tran.TranHeadId,
                    TranHead = tran.TranHead,
                    DrAmount = tran.DrAmount,
                    CrAmount = tran.CrAmount,
                    Note = tran.Note
                };
                vm.TransactionDetail.Add(vmTranDetail);
            }

            return vm;
        }
    }
}