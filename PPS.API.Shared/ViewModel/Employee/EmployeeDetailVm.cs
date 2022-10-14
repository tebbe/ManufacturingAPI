using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.API.Shared.ViewModel.Employee
{
    public class EmployeeDetailVm
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string DeptName { get; set; }
        public string Designation { get; set; }
        public double SalesTarget { get; set; }
        public string Address { get; set; }
        public int EmployeeCode { get; set; }
        public List<EmployeeDetailVm> Employees { get; set; }
    }
}