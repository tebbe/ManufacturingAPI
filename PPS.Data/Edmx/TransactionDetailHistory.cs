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
    
    public partial class TransactionDetailHistory
    {
        public int Id { get; set; }
        public int TransactionDetailId { get; set; }
        public int AccountHeadId { get; set; }
        public double DrAmount { get; set; }
        public double CrAmount { get; set; }
        public int TransactionEntryId { get; set; }
        public string Note { get; set; }
    
        public virtual AccountHead AccountHead { get; set; }
        public virtual TransactionEntryHistory TransactionEntryHistory { get; set; }
    }
}
