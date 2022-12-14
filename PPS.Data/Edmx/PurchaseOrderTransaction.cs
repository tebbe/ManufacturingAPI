//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PPS.Data.Edmx
{
    using System;
    using System.Collections.Generic;
    
    public partial class PurchaseOrderTransaction
    {
        public int Id { get; set; }
        public int PurchaseOrderId { get; set; }
        public int SupplierId { get; set; }
        public int CashBankAccountHeadId { get; set; }
        public double TransactionAmount { get; set; }
        public System.DateTime TransactionDate { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<int> ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedOn { get; set; }
        public bool IsApproved { get; set; }
    
        public virtual AccountHead AccountHead { get; set; }
        public virtual PurchaseOrder PurchaseOrder { get; set; }
        public virtual Supplier Supplier { get; set; }
    }
}
