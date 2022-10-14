using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.API.Shared.ViewModel.Sales
{
    public class DeliveryChallanInvoiceDetailVm
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int? DeliveryQuantity { get; set; }
        public int InvoiceQuantity { get; set; }
        public int? UndeliveredQuantity { get; set; }
        public int? AvailableQuantity { get; set; }
    }
}