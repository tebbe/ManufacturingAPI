using PPS.API.Shared.ViewModel.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PPS.API.Shared.RequestVm;
using PPS.Data.Edmx;
using PPS.API.Shared.ViewModel.Sales;

namespace PPS.Data.RepositoryInterfaces
{
    public interface IEmployeeRepository
    {
        List<EmployeeVm> GetEmployee();
        IQueryable<EmployeeType> GetEmployeeType();
        List<EmployeeVm> GetEmployee(int employeeId);
        List<EmployeeVm> GetSalesEmployeeWithSalesTargetByMonth(EmployeeRequestVm employeeRequestVm);
        EmployeeDetailVm GetEmployeeHierarchy(int employeeId);
        List<Tuple<int, int>> GetManagedEmployee(int employeeId);
        IQueryable<Employee> GetEmployeeList();
        IQueryable<SalesBase> GetSalesBase(int id);
        IQueryable<Department> GetDepartment();
        IQueryable<Designation> GetDesignation();
        Employee GetEmployeeById(int Id);
        Employee AddNewEmployee(Employee employee,List<EmployeeSalesLocation> sLocation);
        Employee EmployeeUpdate(Employee employee, EmployeeHistory employeeHistory, List<EmployeeSalesLocationVm> sLocation);
        
        IQueryable<EmployeeHistory> GetEmployeeHistory(int id);
        IQueryable<SalesArea> GetSalesArea(int id);
        IQueryable<SalesDivision> GetSalesDivision();
        IQueryable<SalesArea> GetSalesAreaList();
        IQueryable<SalesBase> GetSalesBaseList();
        SalesArea AddSalesArea(SalesArea saleArea);
        SalesBase AddSalesBase(SalesBase saleArea);
        IQueryable<SalesArea> GetSalesAreaById(int id);
        IQueryable<SalesBase> GetSalesBaseById(int id);
        SalesArea UpdateSalesArea(SalesArea area);
        SalesBase UpdateSalesBase(SalesBase saleBase);
        IQueryable<EmployeeSalesLocation> GetSalesLocationByEmployeeId(int id);
        IQueryable<EmployeeSalesLocationHistory> GetSalesLocationHistoryByEmployeeId(int id);
       
       
    }
}
