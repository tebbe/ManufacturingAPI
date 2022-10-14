using System;
using System.Collections.Generic;

namespace PPS.API.Shared.ViewModel.Store
{
    public class FinishedGoodVm
    {
        public int Id { get; set; }
        public DateTime ProductionDate { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductionGroupId { get; set; }
        public string ProductionGroupIdName { get; set; }
        public int Quantity { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ApprovedBy { get; set; }
        public string ApprovedByName { get; set; }
        public DateTime ApprovedOn { get; set; }
        public string FPStatusName { get; set; }
        public bool IsApproved { get; set; }
        public IList<FinishedGoodVm> FinishedGoodSubList { get; set; }
        public int TotalCount { get; set; }
    }
}