using PPS.API.Shared.ViewModel.Company;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.API.Shared.ViewModel.Leave
{
    public class EmployeeLeaveVm
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int EmployeeCode { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string  FirstName { get; set; }
        public string LastName { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int DesignationId { get; set; }
        public string DesignationName { get; set; }
        public DateTime DateOfJoin { get; set; }
        public int LeaveCategoryId { get; set; }
        public string NatureOfLeaveName { get; set; }
        public int? LeaveDays { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string ReasonOfLeave { get; set; }
        public string Address { get; set; }
        public string MobileNo { get; set; }
        public DateTime DateOfApplication { get; set; }
        public bool ApprovedByHR { get; set; }
        public bool ApprovedByDepartmentHead { get; set; }
        public bool ApprovedByMD { get; set; }
        public int LeaveYear { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public DateTime? ApprovedByHROn { get; set; }
        public DateTime? ApprovedByHeadOn { get; set; }
        public DateTime? ApprovedByAdminOn { get; set; }
        public int? IsApproved { get; set; }
        public int? UnpaidLeaveDays { get; set; }
        public EmployeeLeaveVm Employee { get; set; }
        public CompanyVm Company { get; set; }
        public EmployeeLeaveDetailsVm EmployeeLeaveDetails { get; set; }
        public CompanyLeaveVm CompanyLeave { get; set; }
    
    }
}