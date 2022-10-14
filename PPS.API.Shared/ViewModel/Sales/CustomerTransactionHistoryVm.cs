using System;

namespace PPS.API.Shared.ViewModel.Sales
{
    public class CustomerTransactionHistoryVm
    {
        public int Id { get; set; }
        public int DemandOrderNo { get; set; }
        public DateTime DemandOrderDate { get; set; }
        public double DOAmount { get; set; }
        public double DOPaidAmount { get; set; }
        public double DOBalanceAmount { get; set; }
        public double DOInvoiceAmount { get; set; }
        public double DOInvoiceBalance { get; set; }
        public string SaleType { get; set; }
    }
}