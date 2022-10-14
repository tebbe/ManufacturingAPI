using System;
using System.Collections.Generic;

namespace PPS.API.Shared.ViewModel.Purchase
{
    public class PurchaseOrderModel
    {
        public int Id { get; set; }
        public int PurchaseOrderNo { get; set; }
        public DateTime PurchaseOrderDate { get; set; }
        public int? SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string Note { get; set; }
        public int? VerifiedBy { get; set; }
        public DateTime VerifiedOn { get; set; }
        public string VerifiedByOn { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime ApprovedOn { get; set; }
        public string ApprovedByOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsCurrentRecord { get; set; }
        public int? PreviousId { get; set; }
        public bool Locked { get; set; }
        public int? RejectedReasonTypeId { get; set; }
        public int? RejectedBy { get; set; }
        public DateTime RejectedOn { get; set; }
        public string RejectedByOn { get; set; }
        public string PaymentType { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
        public int? PriceValidity { get; set; }
        public double TotalAmount { get; set; }
        public int? PurchaseOrderStatusId { get; set; }
        public string POStatusName { get; set; }
        public List<PurchaseOrderDetailModel> PurchaseOrderDetail { get; set; }
        public double PaidAmount { get; set; }
        public double? BalanceAmount { get; set; }
        public bool? IsCashPurchase { get; set; }
        public bool? IsCreditPurchase { get; set; }
        public bool? IsLcPurchase { get; set; }
        public int? CashAccountHeadId { get; set; }
        public double? CashAmount { get; set; }
        public int? BankAccountHeadId { get; set; }
        public double? BankAmount { get; set; }
        public int? SupplierAccountHeadId { get; set; }
        public double? SupplierAmount { get; set; }
        public string LCNo { get; set; }
        public int? LCAccountHeadId { get; set; }
        public double? LCAmount { get; set; }
    }
}