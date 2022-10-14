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
    
    public partial class Customer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Customer()
        {
            this.CrashedGood = new HashSet<CrashedGood>();
            this.CustomerAttachment = new HashSet<CustomerAttachment>();
            this.CustomerSalesCredit = new HashSet<CustomerSalesCredit>();
            this.CustomerSalesCreditHistory = new HashSet<CustomerSalesCreditHistory>();
            this.CustomerTransactionDetail = new HashSet<CustomerTransactionDetail>();
            this.DemandOrder = new HashSet<DemandOrder>();
        }
    
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public int CustomerCode { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerMobile { get; set; }
        public string CustomerPhone { get; set; }
        public string OwnerName { get; set; }
        public string OwnerMobile { get; set; }
        public string OwnerPhone { get; set; }
        public Nullable<System.DateTime> OwnerBirthDate { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPersonMobile { get; set; }
        public string PrimaryContactNo { get; set; }
        public string Village { get; set; }
        public Nullable<int> PostOfficeId { get; set; }
        public string Email { get; set; }
        public Nullable<int> AreaId { get; set; }
        public Nullable<int> EmployeeId { get; set; }
        public Nullable<int> CustomerTypeId { get; set; }
        public Nullable<int> AccountHeadId { get; set; }
        public Nullable<int> CustomerStatusId { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
    
        public virtual AccountHead AccountHead { get; set; }
        public virtual Area Area { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CrashedGood> CrashedGood { get; set; }
        public virtual CustomerStatus CustomerStatus { get; set; }
        public virtual CustomerType CustomerType { get; set; }
        public virtual PostOffice PostOffice { get; set; }
        public virtual Employee Employee { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustomerAttachment> CustomerAttachment { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustomerSalesCredit> CustomerSalesCredit { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustomerSalesCreditHistory> CustomerSalesCreditHistory { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustomerTransactionDetail> CustomerTransactionDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DemandOrder> DemandOrder { get; set; }
    }
}