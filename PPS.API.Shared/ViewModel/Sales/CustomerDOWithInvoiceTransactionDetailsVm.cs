using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.API.Shared.ViewModel.Sales
{
    public class CustomerDOWithInvoiceTransactionDetailsVm
    {
        public double TotalDOAmount { get; set; }
        public double CustomerMatureInvoiceAmount { get; set; }
        public double CustomerImmatureInvoiceAmount { get; set; }
        public double CustomerTotalInvoiceBalance { get; set; }
        public double CustomerTotalMatureDue { get; set; }
        public double CustomerTotalImmatureDue { get; set; }
        public double SingleDOInviceAmount { get; set; }
        public double SingleDoInvoiceTransactionAmount { get; set; }
        public double SingleDOInvoiceDue { get; set; }
        public double CustomerTotalTransactionAmount { get; set; }
    }
}