using System;

namespace PPS.API.Shared.ViewModel.Purchase
{
    public class PurchaseOrderTransactionVm
    {
        public int Id { get; set; }
        public int PurchaseOrderId { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string SupplierCode { get; set; }
        public int SupplierAccountHeadId { get; set; }
        public int CashBankAccountHeadId { get; set; }
        public string AccountName { get; set; }
        public string AccountCode { get; set; }
        public DateTime TransactionDate { get; set; }
        public double TransactionAmount { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ApprovedBy { get; set; }
        public DateTime ApprovedOn { get; set; }
        public bool IsApproved { get; set; }
        public int PurchaseOrderNo { get; set; }
    }
}