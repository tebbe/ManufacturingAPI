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
    
    public partial class CurrentProductStock
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int TotalQuantity { get; set; }
        public int DeliveredQuantity { get; set; }
        public int AllocatedQuantity { get; set; }
        public int AvailableQuantity { get; set; }
    
        public virtual Product Product { get; set; }
    }
}
