using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PPS.API.Shared.ViewModel.Employee;
using PPS.API.Shared.ViewModel.Leave;
using PPS.Data.Edmx;

namespace PPS.Service.ServiceInterfaces
{
    public interface ILeaveInterface
    {
        EmployeeLeave SaveEmployeeLeave(EmployeeLeaveVm employeeLeaveVm);
        EmployeeLeaveVm GetEmployeeLeaveById(int id);
        List<EmployeeLeaveVm> GetLeaveListByEmployeeId(int employeeId);
        EmployeeLeave UpdateEmployeeLeave(EmployeeLeaveVm employeeLeaveVm);
        List<EmployeeLeaveVm> GetEmployeeLeaveList();
        List<LeaveCategoryVm> GetLeaveCategory();
        List<EmployeeHierArchyVm> GetEmployeeHierArchy(int employeeId);
        List<EmployeeHierArchyVm> GetEmployeeById(int employeeId);
        List<EmployeeLeaveVm> GetEmployeeLeaveDetailsByEmployeeId(int employeeId);
        List<EmployeeLeaveVm> GetEmployeeHierArchyLeaveList(int employeeId,int? status);
        EmployeeLeave ApproveOrRejectEmployeeLeave(EmployeeLeaveVm employeeLeaveVm);
    }
}