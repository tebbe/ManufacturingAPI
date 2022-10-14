using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.API.Shared.ViewModel.Sales
{
    public class InvoiceVm
    {
        public int Id { get; set; }
        public int InvoiceNo { get; set; }
        public int DemandOrderId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int? UpdatedBy { get; set; }
        public string UpdatedByName { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ApprovedBy { get; set; }
        public string ApprovedByName { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string Note { get; set; }
        public string InvoiceStatusName { get; set; }
        public double? TotalAmount { get; set; }
        public double? TotalGrandAmount { get; set; }
        public double? RegularDiscountInPercentage { get; set; }
        public double? RegularDiscountAmount { get; set; }
        public double? SpecialDiscountInPercentage { get; set; }
        public double? SpecialDiscountAmount { get; set; }
        public double? AdditionalDiscountInPercentage { get; set; }
        public double? AdditionalDiscountAmount { get; set; }
        public double? ExtraDiscountInPercentage { get; set; }
        public double? ExtraDiscountAmount { get; set; }
        public double? CashBackAmount { get; set; }
        public double? TotalDiscountAmount { get; set; }
        public List<InvoiceDetailVm> InvoiceDetail { get; set; }
        public int DemandOrderNo { get; set; }
        public int CustomerId { get; set; }
        public int CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerMobile { get; set; }
        public int? DeliveredBy { get; set; }
        public double TotalCustomerBalance { get; set; }
        public double? TotalInvoiceBalanceAmount { get; set; }
        public double? CustomerRemainingBalance { get; set; }
        public double TotalPaidAmount { get; set; }
        public double? TotalDueAmount { get; set; }
        public double? TotalDiscountInPercent { get; set; }
        public List<InvoiceTransactionVm> InvoiceTransaction { get; set; }
        public string DONote { get; set; }
    }
}