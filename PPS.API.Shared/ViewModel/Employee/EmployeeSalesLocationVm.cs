using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.API.Shared.ViewModel.Employee
{
    public class EmployeeSalesLocationVm
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int DivisionId { get; set; }
        public int? AreaId { get; set; }
        public int? BaseId { get; set; }
        public int ActionTypeId  { get; set; }
        public string DivisionName { get; set; }
        public string AreaName { get; set; }
        public string BaseName { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdateOn { get; set; }
        public int? EmployeeHistoryId { get; set; }
    }

}