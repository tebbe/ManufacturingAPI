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
    
    public partial class SaleType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SaleType()
        {
            this.DemandOrder = new HashSet<DemandOrder>();
        }
    
        public int Id { get; set; }
        public string SaleTypeName { get; set; }
        public int DurationInDays { get; set; }
        public Nullable<int> WarningInDays { get; set; }
        public Nullable<int> EarlyPaymentInDays { get; set; }
        public Nullable<double> EarlyPaymentDiscountInPercentage { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DemandOrder> DemandOrder { get; set; }
    }
}
