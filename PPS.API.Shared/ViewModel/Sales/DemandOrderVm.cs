using System;
using System.Collections.Generic;

namespace PPS.API.Shared.ViewModel.Sales
{
    public class DemandOrderVm
    {
        public int Id { get; set; }
        public int DemandOrderNo { get; set; }
        public DateTime DODate { get; set; }
        public int SaleTypeId { get; set; }
        public string SaleTypeName { get; set; }
        public int DemandOrderTypeId { get; set; }
        public string DemandOrderTypeName { get; set; }
        public string ReferenceNo { get; set; }
        public int CustomerId { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CustomerName { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string DOStatusName { get; set; }
        public int DiscountTypeId { get; set; }
        public string DODiscountTypeName { get; set; }
        public int DOPaymentStatusId { get; set; }
        public string DOPaymentStatus { get; set; }
        public bool Submitted { get; set; }
        public string InitiatedByOn { get; set; }
        public string SubmittedByOn { get; set; }
        public string VerifiedByOn { get; set; }
        public string ApprovedByOn { get; set; }
        public string DeliveredByOn { get; set; }
        public double TotalAmount { get; set; }
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
        public double TotalGrandAmount { get; set; }
        public double TotalPaidAmount { get; set; }
        public double TotalDueAmount { get; set; }
        public double TotalBalanceAmount { get; set; }
        public List<DemandOrderDetailVm> DemandOrderDetail { get; set; }
        public List<DemandOrderTransactionVm> DemandOrderTransaction { get; set; }
        public int EmployeeId { get; set; }
        public int EmployeeCode { get; set; }
        public string CreatedByDesignation { get; set; }
        public string VerifiedByName { get; set; }
        public string VerifiedByDesignation { get; set; }
        public int CustomerCode { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerMobile { get; set; }
        public string EmployeeName { get; set; }
        public int MaturityDays { get; set; }
        public int MaturityLabel { get; set; }
        public string DODiscountTransactionStatusName { get; set; }
        public double EarlyPaymentDiscountInPercentage { get; set; }
        public double EarlyPaymentDiscountAmount { get; set; }
        public int ProductTypeGroupId { get; set; }
        public string ProductTypeGroupName { get; set; }
        public DateTime? VerifiedOn { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public int TotalCount { get; set; }
        public double? TotalDoBalanceAmount { get; set; }
        public double? CustomerRemainingBalance { get; set; }
        public List<InvoiceVm> InvoiceList { get; set; }
        public double? TotalInvoiceBalance { get; set; }
        public CustomerDOWithInvoiceTransactionDetailsVm CustomerDoWithInvoiceDetails { get; set; }
        public string Note { get; set; }
    }
}