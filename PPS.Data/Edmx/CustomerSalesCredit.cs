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
    
    public partial class CustomerSalesCredit
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Nullable<decimal> MonthlyCredit { get; set; }
        public Nullable<decimal> YearlyCredit { get; set; }
        public Nullable<System.DateTime> EffectiveDate { get; set; }
        public Nullable<decimal> SalesCapacityYearly { get; set; }
    
        public virtual Customer Customer { get; set; }
    }
}
