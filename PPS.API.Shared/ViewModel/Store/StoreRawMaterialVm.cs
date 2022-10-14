using System;

namespace PPS.API.Shared.ViewModel.Store
{
    public class StoreRawMaterialVm
    {
        public int Id { get; set; }
        public int PurchaseOrderId { get; set; }
        public int RawMaterialTypeId { get; set; }
        public string RawMaterialTypeName { get; set; }
        public string UnitTypeName { get; set; }
        public int Quantity { get; set; }
        public int ReceivedBy { get; set; }
        public DateTime ReceivedOn { get; set; }
        public int PurchaseOrderStatusId { get; set; }
    }
}