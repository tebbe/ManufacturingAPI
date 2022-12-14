using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.API.Shared.ViewModel.Sales
{
    public class InvoiceDetailVm
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int NewAllocatedQuantity { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int AllocatedQuantity { get; set; }
        public int DeliveredQuantity { get; set; }
        public int? AvailableQuantity { get; set; }
        public double UnitPrice { get; set; }
        public double Discount { get; set; }
        public double TotalPrice { get; set; }
        public int PreAllocatedQuantity { get; set; }
    }
}