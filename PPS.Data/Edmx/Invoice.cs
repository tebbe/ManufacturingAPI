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
    
    public partial class Invoice
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Invoice()
        {
            this.CrashedGood = new HashSet<CrashedGood>();
            this.DeliveryQuantity = new HashSet<DeliveryQuantity>();
            this.InvoiceReturn = new HashSet<InvoiceReturn>();
            this.InvoiceDetail = new HashSet<InvoiceDetail>();
            this.InvoiceTransaction = new HashSet<InvoiceTransaction>();
        }
    
        public int Id { get; set; }
        public int InvoiceNo { get; set; }
        public int DemandOrderId { get; set; }
        public System.DateTime InvoiceDate { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string Note { get; set; }
        public Nullable<int> ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedOn { get; set; }
        public Nullable<int> DeliveredBy { get; set; }
        public Nullable<System.DateTime> DeliveredOn { get; set; }
        public Nullable<double> TotalAmount { get; set; }
        public Nullable<double> RegularDiscountInPercentage { get; set; }
        public Nullable<double> RegularDiscountAmount { get; set; }
        public Nullable<double> SpecialDiscountInPercentage { get; set; }
        public Nullable<double> SpecialDiscountAmount { get; set; }
        public Nullable<double> AdditionalDiscountInPercentage { get; set; }
        public Nullable<double> AdditionalDiscountAmount { get; set; }
        public Nullable<double> ExtraDiscountInPercentage { get; set; }
        public Nullable<double> ExtraDiscountAmount { get; set; }
        public Nullable<double> CashBackAmount { get; set; }
        public Nullable<double> TotalDiscountAmount { get; set; }
        public Nullable<double> TotalGrandAmount { get; set; }
        public Nullable<double> TotalDueAmount { get; set; }
        public Nullable<int> TransactionEntryId { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CrashedGood> CrashedGood { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryQuantity> DeliveryQuantity { get; set; }
        public virtual DemandOrder DemandOrder { get; set; }
        public virtual TransactionEntry TransactionEntry { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvoiceReturn> InvoiceReturn { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvoiceDetail> InvoiceDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvoiceTransaction> InvoiceTransaction { get; set; }
    }
}
