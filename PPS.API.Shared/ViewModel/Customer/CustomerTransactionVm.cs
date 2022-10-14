using System;
using System.Collections.Generic;

namespace PPS.API.Shared.ViewModel.Customer
{
    public class CustomerTransactionVm
    {
        public int Id { get; set; }
        public int CashBankAccountHeadId { get; set; }
        public string AccountName { get; set; }
        public string AccountCode { get; set; }
        public DateTime TransactionDate { get; set; }
        public double TransactionAmount { get; set; }
        public string TransactionReference { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ApprovedBy { get; set; }
        public DateTime ApprovedOn { get; set; }
        public bool IsApproved { get; set; }
        public IEnumerable<CustomerTransactionDetailVm> CustomerTransactionDetail { get; set; }
        public string Status { get; set; }
        public double? BankChargeAmount { get; set; }
        public int TotalCount { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public string UpdatedByName { get; set; }
    }
}