using PPS.API.Shared.ViewModel.Employee;
using PPS.API.Shared.ViewModel.Sales;
using PPS.API.Shared.ViewModel.User;
using System.Collections.Generic;
using System.Threading.Tasks;
using PPS.API.Shared.RequestVm;
using PPS.Data.Edmx;

namespace PPS.Service.ServiceInterfaces
{
    public interface IEmployeeInterface
    {
        List<EmployeeVm> GetEmployee();
        List<EmployeeVm> GetEmployee(int employeeId);
        List<EmployeeVm> GetSalesEmployeeWithSalesTargetByMonth(EmployeeRequestVm employeeRequestVm);
        EmployeeDetailVm GetEmployeeHierarchy(int employeeId);
        List<EmployeeVm> GetActiveEmployeeList();
        List<EmployeeVm> GetInactiveEmployeeList();
        List<SalesBaseVm> GetSalesBaseByAreaId(int id);
        List<EmployeeVm>GetManager();
        List<DepartmentVm>GetDepartment();
        List<DesignationVm> GetDesignation();
        EmployeeVm GetEmployeeById(int id);
        List<EmployeeVm> GetEmployeeHistory(int id);
        Employee AddNewEmployee(EmployeeVm employee);
        Employee EmployeeUpdate(EmployeeVm employee);
        List<SalesAreaVm> GetSalesAreaByDevisionId(int id);
        List<SalesDivisionVm> GetSalesDivision();
        List<SalesAreaVm> GetSalesAreaList();
        List<SalesBaseVm> GetSalesBaseList();
        SalesArea AddSalesArea(SalesArea salesArea);
        SalesBase AddSalesBase(SalesBase salesBase);
        List<SalesAreaVm> GetSalesAreaById(int id);
        List<SalesBaseVm> GetSalesBaseById(int id);
        SalesArea UpdateSalesArea(SalesArea salesArea);
        SalesBase UpdateSalesBase(SalesBase salesBase);
        List<EmployeeSalesLocationVm> GetSalesLocationByEmployeeId(int id);
        List<EmployeeSalesLocationVm> GetSalesLocationHistoryByEmployeeId(int id);
        List<EmployeeTypeVm> GetEmployeeType();
    }

}