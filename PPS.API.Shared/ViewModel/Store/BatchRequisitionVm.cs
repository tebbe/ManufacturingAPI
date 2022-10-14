using System;
using System.Collections.Generic;
using PPS.API.Shared.ViewModel.Purchase;

namespace PPS.API.Shared.ViewModel.Store
{
    public class BatchRequisitionVm
    {
        public BatchRequisitionVm()
        {
            BatchRequisitionDetail = new HashSet<BatchRequisitionDetailVm>();
        }

        public int Id { get; set; }
        public int BatchRequisitionNo { get; set; }
        public int ProductionGroupId { get; set; }
        public string ProductionGroupIdName { get; set; }
        public int CreatedBy { get; set; }
        public String CreatedByName { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? DeliveredBy { get; set; }
        public DateTime? DeliveredOn { get; set; }
        public int? ReceivedBy { get; set; }
        public DateTime? ReceivedOn { get; set; }
        public int? SendToProductionBy { get; set; }
        public DateTime? SendToProductionOn { get; set; }
        public string BRStatusName { get; set; }
        public bool IsClosed { get; set; }
        public ICollection<BatchRequisitionDetailVm> BatchRequisitionDetail { get; set; }
        public string DeliveredByOn { get; set; }
        public string ReceivedByOn { get; set; }
        public string SendToProductionByOn { get; set; }
        public DateTime? EstimatedProductionDate { get; set; }
        public DateTime BatchRequisitionDate { get; set; }
        public ICollection<BatchRequisitionProductionEstimationVm> BatchRequisitionProductionEstimation { get; set; }
    }
}