using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.API.Shared.ViewModel.Purchase
{
    public class PurchaseOrderDetailModel
    {
        public int Id { get; set; }
        public int PurchaseOrderId { get; set; }
        public int RawMaterialTypeId { get; set; }
        public string RawMaterialTypeName { get; set; }
        public string UnitTypeName { get; set; }
        public double? Quantity { get; set; }
        public double? Price { get; set; }
        public double? TotalUnitPrice { get; set; }
        public double? BalanceQuantity { get; set; }
        public double? AcceptedQuantity { get; set; }
        public int AccountHeadId { get; set; }
    }
}