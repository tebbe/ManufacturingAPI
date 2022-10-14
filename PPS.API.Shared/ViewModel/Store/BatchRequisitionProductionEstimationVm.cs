using System;

namespace PPS.API.Shared.ViewModel.Store
{
    public class BatchRequisitionProductionEstimationVm
    {
        public int Id { get; set; }
        public int BatchRequisitionId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string ProductCode { get; set; }
    }
}