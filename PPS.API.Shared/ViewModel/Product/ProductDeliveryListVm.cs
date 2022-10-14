using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.API.Shared.ViewModel.Product
{
    public class ProductDeliveryListVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Code { get; set; }
        public double Ammount { get; set; }
        public string DealerName { get; set; }
        public int ProductId { get; set; }
        public string Color { get; set; }
        public string Thickness { get; set; }
        public decimal? Length { get; set; }
        public int DemandOrderQuantity { get; set; }
        public int InvoiceQuantity { get; set; }
        public int DeliveryQuantity { get; set; }
        public int? DealerCode { get; set; }
    }
}