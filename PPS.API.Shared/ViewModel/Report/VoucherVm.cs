using PPS.API.Shared.ViewModel.Company;
using System;
using System.Collections.Generic;

namespace PPS.API.Shared.ViewModel.Report
{
    public class VoucherVm
    {
        public string TransactionNo { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public double TransactionAmount { get; set; }
        public string Particulars { get; set; }
        public string CreatedByName { get; set; }
        public string VerifiedByName { get; set; }
        public string CreatedByDesignation { get; set; }
        public string VerifiedByDesignation { get; set; }
        public IList<VoucherDetailVm> VoucherDetail { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateReason { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public DateTime? VerifiedDate { get; set; }
        public int? VerifiedBy { get; set; }
        public int? ApprovedBy { get; set; }
        public int CreatedBy { get; set; }
    }
}