using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.API.Shared.ViewModel.Sales
{
    public class InvoiceReturnDetailVm
    {
        public int Id { get; set; }
        public int InvoiceReturnId { get; set; }
        public int ProductId { get; set; }
        public int ReturnQuantity { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }
        public string ProductName { get; set; }
        public decimal UnitePrice { get; set; }
        public IEnumerable<int> DeliveryQuantity { get; set; }
    }
}