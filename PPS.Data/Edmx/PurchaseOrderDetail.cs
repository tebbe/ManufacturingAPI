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
    
    public partial class PurchaseOrderDetail
    {
        public int Id { get; set; }
        public int PurchaseOrderId { get; set; }
        public int RawMaterialTypeId { get; set; }
        public Nullable<double> Quantity { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<int> AccountHeadId { get; set; }
    
        public virtual PurchaseOrder PurchaseOrder { get; set; }
        public virtual RawMaterialType RawMaterialType { get; set; }
    }
}
