using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.API.Shared.ViewModel.Employee
{
    public class EmployeeVm
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime? DateOfJoin { get; set; }
        public DateTime? JobConfirmationDate { get; set; }
        public string Designation { get; set; }
        public string DeptName { get; set; }
        public string BloodGroup { get; set; }
        public string SalesDivision { get; set; }
        public string SalesBase { get; set; }
        public string Mobile { get; set; }
        public int EmployeeCode { get; set; }
        public int? ManagerId { get; set; }
        public string ManagerName { get; set; } 
        public decimal? SalesTarget { get; set; }
        public decimal? TeamTarget { get; set; }
        public int? SalesDivisionId { get; set; }
        public int? SalesAreaId { get; set; }
        public int? SalesBaseId { get; set; }
        public string SalesArea { get; set; }
        public int EmployeeId { get; set; }
        public bool IsActive { get; set; }
        public int? DepartmentId { get; set; }
        public int? CompanyId { get; set; }
        public int? DesignationId { get; set; }
        public string Phone { get; set; }
        public int? PostOfficeId { get; set; }
        public string Address { get; set; }
        public string ImageId { get; set; }
        public string CompanyName { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public List<EmployeeSalesLocationVm> SalesLocation { get; set; }
        public int? EmployeeTypeId { get; set; }
    }
}