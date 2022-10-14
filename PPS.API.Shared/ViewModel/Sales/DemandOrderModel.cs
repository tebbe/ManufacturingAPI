using System;
using System.Collections.Generic;

namespace PPS.API.Shared.ViewModel.Sales
{
    public class DemandOrderModel
    {
        public DemandOrderModel()
        {
            DemandOrderDetail = new HashSet<DemandOrderDetailVm>();
        }
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int DemandOrderNo { get; set; }
        public int DemandOrderTypeId { get; set; }
        public string DemandOrderTypeName { get; set; }
        public int DiscountTypeId { get; set; }
        public string DiscountTypeName { get; set; }
        public DateTime DODate { get; set; }
        public int EmployeeId { get; set; }
        public int? VarifiedBy { get; set; }
        public DateTime VarifiedOn { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime ApprovedOn { get; set; }
        public string ReferenceDONo { get; set; }
        public int? SubmittedBy { get; set; }
        public DateTime SubmittedOn { get; set; }
        public int? RejectedBy { get; set; }
        public DateTime RejectedOn { get; set; }
        public int? ReturnedBy { get; set; }
        public DateTime ReturnedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int IsCurrentRecord { get; set; }
        public int PreviousId { get; set; }
        public bool Locked { get; set; }
        public int RejectedReasonTypeId { get; set; }
        public int SaleTypeId { get; set; }
        public string SaleTypeName { get; set; }
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
        public double TotalDiscountAmount { get; set; }
        public double TotalGrandAmount { get; set; }
        public ICollection<DemandOrderDetailVm> DemandOrderDetail { get; set; }
        public ICollection<DemandOrderTransactionVm> DemandOrderTransaction { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int IsInvoiceCompleted { get; set; }
        public string Note { get; set; }
    }    
}