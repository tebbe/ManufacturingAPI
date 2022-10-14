using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.API.Shared.ViewModel.Purchase
{
    public class PurchaseOrderVm
    {
        public int POId { get; set; }
        public int PurchaseOrderNo { get; set; }
        public DateTime PurchaseOrderDate { get; set; }
        public string SupplierName { get; set; }
        public string Note { get; set; }
        public string PaymentType { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
        public int? PriceValidity { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public double? TotalAmount { get; set; }
        public string POStatusName { get; set; }
        public double PaidAmount { get; set; }
        public double? BalanceAmount { get; set; }
    }
}