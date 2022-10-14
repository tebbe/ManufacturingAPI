using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PPS.API.Shared.ViewModel;
using PPS.API.Shared.ViewModel.Employee;
using PPS.Data.Edmx;

namespace PPS.Data.RepositoryInterfaces
{
    public interface ILeaveRepository
    {
        EmployeeLeave SaveEmployeeLeave(EmployeeLeave employeeLeave);
        EmployeeLeave GetEmployeeLeaveById(int id);
        IQueryable<EmployeeLeave> GetLeaveListByEmployeeId(int employeeId);
        EmployeeLeave UpdateEmployeeLeave(EmployeeLeave employeeLeave);
        IQueryable<EmployeeLeave> GetEmployeeLeaveList();
        IQueryable<LeaveCategory> GetLeaveCategory();
        IQueryable<Employee> GetEmployee();
        IQueryable<EmployeeLeave>GetLeaveListByEmployeeIdAndStatus(int id, int? status);
        EmployeeLeave ApproveOrRejectEmployeeLeave(EmployeeLeave employeeLeave);
        
    }
}