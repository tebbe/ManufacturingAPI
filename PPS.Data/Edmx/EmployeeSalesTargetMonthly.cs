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
    
    public partial class EmployeeSalesTargetMonthly
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public decimal SalesTarget { get; set; }
        public decimal TeamTarget { get; set; }
        public Nullable<decimal> Achievement { get; set; }
        public Nullable<decimal> Percentage { get; set; }
        public int SalesYear { get; set; }
        public int SalesMonth { get; set; }
    
        public virtual Employee Employee { get; set; }
    }
}
