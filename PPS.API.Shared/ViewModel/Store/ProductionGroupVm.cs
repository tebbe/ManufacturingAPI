using System;
using System.Collections.Generic;
using PPS.API.Shared.ViewModel.Purchase;

namespace PPS.API.Shared.ViewModel.Store
{
    public class ProductionGroupVm
    {
        public int Id { get; set; }
        public string ProductionGroupId { get; set; }
        public bool IsClosed { get; set; }
        public int CreatedBy { get; set; }
        public String CreatedByName { get; set; }
        public DateTime CreatedOn { get; set; }
        public IList<BatchRequisitionVm> BatchRequisitionList { get; set; }
    }
}