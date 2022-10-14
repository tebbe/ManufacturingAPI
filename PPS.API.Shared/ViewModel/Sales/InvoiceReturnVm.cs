using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.API.Shared.ViewModel.Sales
{
    public class InvoiceReturnVm
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public DateTime ReturnDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Note { get; set; }
        public Nullable<int> ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public double TotalAmount { get; set; }
        public double TotalGrandAmount { get; set; }
        public Nullable<int> TransactionEntryId { get; set; }
        public string CreatedByName { get; set; }
        public string ApprovedByName { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdateOn { get; set; }
        public double? RegularDiscountInPercentage { get; set; }
        public double TotalDiscountAmount { get; set; }
        public int? CompanyId { get; set; }
        public List<InvoiceReturnDetailVm> InvoiceReturnDetail { get; set; }

    }
}