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
    
    public partial class BatchRequisitionDetail
    {
        public int Id { get; set; }
        public int BatchRequisitionId { get; set; }
        public int RawMaterialTypeId { get; set; }
        public double Quantity { get; set; }
    
        public virtual BatchRequisition BatchRequisition { get; set; }
        public virtual RawMaterialType RawMaterialType { get; set; }
    }
}
