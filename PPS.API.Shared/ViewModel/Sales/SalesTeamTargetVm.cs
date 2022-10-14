using System;
using System.Collections.Generic;

namespace PPS.API.Shared.ViewModel.Sales
{
    public class SalesTeamTargetVm
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public decimal SalesTarget { get; set; }
        public decimal TeamTarget { get; set; }
        public decimal Achievement { get; set; }
        public decimal Percentage { get; set; }
        public int SalesYear { get; set; }
        public int SalesMonth { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime ApprovedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsApproved { get; set; }
        public string CreatedByName { get; set; }
        public string ApprovedByName { get; set; }
        public string ApprovedByOn { get; set; }
        public string Status { get; set; }
        public string EmployeeName { get; set; }
        public string TargetDate { get; set; }
    }    
}