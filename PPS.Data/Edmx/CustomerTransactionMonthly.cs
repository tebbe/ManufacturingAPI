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
    
    public partial class CustomerTransactionMonthly
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public System.DateTime TransactionMonth { get; set; }
        public decimal TotalDoAmount { get; set; }
        public decimal TotalPaidAmount { get; set; }
        public decimal CarryForwardedBalanceAmount { get; set; }
        public decimal BalanceAmount { get; set; }
    }
}