using PPS.Service.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PPS.Data.Dtos;
using PPS.Data.RepositoryInterfaces;
using PPS.Data.Repositories;
using PPS.API.Shared.ViewModel.Account;
using PPS.API.Shared.ViewModel.Sales;
using System.Threading.Tasks;
using PPS.API.Shared.RequestVm;
using PPS.API.Shared.ViewModel.User;
using PPS.API.Shared.ViewModel.Employee;
using System.Data.Entity.Core.Objects;
using PPS.Data.Edmx;

namespace PPS.Service.Services
{
    public class EmployeeService : IEmployeeInterface
    {

        private readonly IEmployeeRepository _employeeRepository;
        private PPSDbContext _ppsDbContext;
        public EmployeeService()
        {
            _employeeRepository = new EmployeeRepository();
            _ppsDbContext = new PPSDbContext();
        }

        public List<EmployeeVm> GetEmployee(int employeeId)
        {
            return _employeeRepository.GetEmployee(employeeId);
        }
        public List<EmployeeTypeVm> GetEmployeeType()
        {
            return _employeeRepository.GetEmployeeType().Select(d=>new EmployeeTypeVm {
                Id=d.Id,
                TypeName=d.TypeName,
                Description=d.Description
            }).ToList();
        }
        public List<EmployeeVm> GetSalesEmployeeWithSalesTargetByMonth(EmployeeRequestVm employeeRequestVm)
        {
            return _employeeRepository.GetSalesEmployeeWithSalesTargetByMonth(employeeRequestVm);
        }

        public List<EmployeeVm> GetEmployee()
        {
            return _employeeRepository.GetEmployee();
        }

        public EmployeeDetailVm GetEmployeeHierarchy(int employeeId)
        {
            return _employeeRepository.GetEmployeeHierarchy(employeeId);
        }

        public List<EmployeeVm> GetActiveEmployeeList()
        {

            var employeeList = _employeeRepository.GetEmployeeList()
                .Where(m => m.IsActive == true)
                .Select(d => new EmployeeVm
                {
                    Id = d.Id,
                    EmployeeCode = d.EmployeeCode,
                    FullName = d.FirstName + " " + d.LastName,
                    DeptName = d.Department.DepartmentName,
                    Designation = d.Designation.DesignationName,
                    Email = d.Email,
                    Mobile = d.Mobile,
                    ManagerName = d.Employee2 == null ? "" : d.Employee2.FirstName + " " + d.Employee2.LastName,
                    BloodGroup = d.BloodGroup,
                    SalesDivision = d.SalesDivision == null ? "" : d.SalesDivision.SalesDivisionName,
                    SalesArea = d.SalesArea == null ? "" : d.SalesArea.SalesAreaName,
                    SalesBase = d.SalesBase == null ? "" : d.SalesBase.SalesBaseName
                }).ToList();
            return employeeList;
        }

        public List<EmployeeVm> GetInactiveEmployeeList()
        {
            var employeeList = _employeeRepository.GetEmployeeList()
                .Where(m => m.IsActive == false)
               .Select(d => new EmployeeVm
               {
                   Id = d.Id,
                   EmployeeCode = d.EmployeeCode,
                   FullName = d.FirstName + " " + d.LastName,
                   DeptName = d.Department.DepartmentName,
                   Designation = d.Designation.DesignationName,
                   Email = d.Email,
                   Mobile = d.Mobile,
                   ManagerName = d.Employee2 == null ? "" : d.Employee2.FirstName + " " + d.Employee2.LastName,
                   BloodGroup = d.BloodGroup,
                   SalesDivision = d.SalesDivision == null ? "" : d.SalesDivision.SalesDivisionName,
                   SalesArea = d.SalesArea == null ? "" : d.SalesArea.SalesAreaName,
                   SalesBase = d.SalesBase == null ? "" : d.SalesBase.SalesBaseName
               }).ToList();
            return employeeList;
        }
        public List<SalesDivisionVm> GetSalesDivision()
        {
            var salesDivision = _employeeRepository.GetSalesDivision().Select(d => new SalesDivisionVm
            {
                Id = d.Id,
                SalesDivisionName = d.SalesDivisionName,
            }).OrderBy(m => m.SalesDivisionName).ToList();
            return salesDivision;
        }
        public List<SalesAreaVm> GetSalesAreaByDevisionId(int id)
        {
            var salesArea = _employeeRepository.GetSalesArea(id).Select(d => new SalesAreaVm
            {
                Id = d.Id,
                SalesAreaName = d.SalesAreaName,
                SalesDivisionId = d.SalesDivisionId,
            }).OrderBy(m => m.SalesAreaName).ToList();
            return salesArea;
        }
        public List<SalesBaseVm> GetSalesBaseByAreaId(int id)
        {
            var salesBase = _employeeRepository.GetSalesBase(id).Select(d => new SalesBaseVm
            {
                Id = d.Id,
                SalesBaseName = d.SalesBaseName,
                SalesAreaId = d.SalesAreaId
            }).OrderBy(m => m.SalesBaseName).ToList();
            return salesBase;
        }
        public List<EmployeeVm> GetManager()
        {
            var managerList = _employeeRepository.GetEmployeeList()
                .Where(m => m.IsActive == true)
                .Select(d => new EmployeeVm
                {
                    Id = d.Id,
                    ManagerName = d.FirstName == null ? "" : d.FirstName + "" + d.LastName
                }).ToList();
            return managerList;
        }
        public List<DepartmentVm> GetDepartment()
        {
            var department = _employeeRepository.GetDepartment().Select(d => new DepartmentVm
            {
                Id = d.Id,
                DepartmentName = d.DepartmentName
            }).OrderBy(m => m.DepartmentName).ToList();
            return department;
        }
        public List<DesignationVm> GetDesignation()
        {
            var designation = _employeeRepository.GetDesignation().Select(d => new DesignationVm
            {
                Id = d.Id,
                DesignationName = d.DesignationName
            }).OrderBy(m => m.DesignationName).ToList();
            return designation;
        }
        public Employee AddNewEmployee(EmployeeVm employeeVm)
        {
            var hasEmployeeCode = _employeeRepository.GetEmployeeList().Where(m => m.EmployeeCode == employeeVm.EmployeeCode).FirstOrDefault();
            if (hasEmployeeCode != null)
            {
                throw new KeyNotFoundException($"Employee code: {employeeVm.EmployeeCode} has existed.");
            }
            var employee = new Employee
            {
                EmployeeCode = employeeVm.EmployeeCode,
                FirstName = employeeVm.FirstName,
                LastName = employeeVm.LastName,
                Address = employeeVm.Address,
                DateOfJoin = employeeVm.DateOfJoin,
                PostOfficeId = employeeVm.PostOfficeId,
                Email = employeeVm.Email,
                Mobile = employeeVm.Mobile,
                Phone = employeeVm.Phone,
                JobConfirmationDate=employeeVm.JobConfirmationDate,
                DepartmentId = employeeVm.DepartmentId,
                EmployeeTypeId=employeeVm.EmployeeTypeId,
                BloodGroup = employeeVm.BloodGroup,
                ImageId = employeeVm.ImageId,
                DesignationId = employeeVm.DesignationId,
                ManagerId = employeeVm.ManagerId,
                IsActive = employeeVm.IsActive,
                SalesDivisionId = employeeVm.SalesDivisionId,
                SalesAreaId = employeeVm.SalesAreaId,
                SalesBaseId = employeeVm.SalesBaseId,
                CompanyId = employeeVm.CompanyId,

            };
            List<EmployeeSalesLocation> salesLocation = new List<EmployeeSalesLocation>();
            foreach (var item in employeeVm.SalesLocation)
            {
                var sLocation = new EmployeeSalesLocation
                {
                    DivisionId = item.DivisionId,
                    AreaId = item.AreaId,
                    BaseId = item.BaseId,
                    CreatedBy = employeeVm.CreatedBy,
                    CreatedOn = employeeVm.CreatedOn
                };
                salesLocation.Add(sLocation);
            }

            return _employeeRepository.AddNewEmployee(employee, salesLocation);
        }
        public EmployeeVm GetEmployeeById(int Id)
        {
            var employee = _employeeRepository.GetEmployeeById(Id);
            EmployeeVm employeeVm = new EmployeeVm
            {
                Id = employee.Id,
                EmployeeCode = employee.EmployeeCode,
                EmployeeId = employee.Id,
                CompanyId = employee.CompanyId,
                CompanyName = employee.CompanyId != null ? employee.Company.Name : "",
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                FullName = employee.FirstName + " " + employee.LastName,
                DeptName = employee.Department.DepartmentName,
                DepartmentId = employee.DepartmentId,
                Designation = employee.Designation.DesignationName,
                DesignationId = employee.DesignationId,
                EmployeeTypeId=employee.EmployeeTypeId,
                Email = employee.Email,
                Mobile = employee.Mobile,
                JobConfirmationDate=employee.JobConfirmationDate,
                Phone = employee.Phone,
                DateOfJoin=employee.DateOfJoin,
                Address = employee.Address,
                IsActive = employee.IsActive,
                ManagerName = employee.Employee2 == null ? "" : employee.Employee2.FirstName + " " + employee.Employee2.LastName,
                ManagerId = employee.ManagerId,
                BloodGroup = employee.BloodGroup,
                SalesDivision = employee.SalesDivision == null ? "" : employee.SalesDivision.SalesDivisionName,
                SalesDivisionId = employee.SalesDivisionId,
                SalesArea = employee.SalesArea == null ? "" : employee.SalesArea.SalesAreaName,
                SalesAreaId = employee.SalesAreaId,
                SalesBase = employee.SalesBase == null ? "" : employee.SalesBase.SalesBaseName,
                SalesBaseId = employee.SalesBaseId,
                SalesLocation = employee.EmployeeSalesLocation.Select(d => new EmployeeSalesLocationVm
                {
                    DivisionName=d.DivisionId>0?d.SalesDivision.SalesDivisionName:"N/A",
                    AreaName=d.AreaId>0?d.SalesArea.SalesAreaName:"N/A",
                    BaseName=d.BaseId>0?d.SalesBase.SalesBaseName:"N/A",
                }).ToList()

            };

            return employeeVm;
        }
        public Employee EmployeeUpdate(EmployeeVm employeeEdit)
        {
            var model = _ppsDbContext.Employee.FirstOrDefault(m => m.Id == employeeEdit.Id);
            if (model == null)
            {
                throw new KeyNotFoundException($"Employee Id: {employeeEdit.Id} does not exist.");
            }
            var empEdit = new Employee
            {
                Id= model.Id,
                EmployeeCode = employeeEdit.EmployeeCode,
                FirstName = employeeEdit.FirstName,
                LastName = employeeEdit.LastName,
                Address = employeeEdit.Address,
                PostOfficeId = employeeEdit.PostOfficeId,
                Email = employeeEdit.Email,
                Mobile = employeeEdit.Mobile,
                DateOfJoin=employeeEdit.DateOfJoin,
                Phone = employeeEdit.Phone,
                JobConfirmationDate=employeeEdit.JobConfirmationDate,
                EmployeeTypeId=employeeEdit.EmployeeTypeId,
                DepartmentId = employeeEdit.DepartmentId,
                BloodGroup = employeeEdit.BloodGroup,
                ImageId = employeeEdit.ImageId,
                DesignationId = employeeEdit.DesignationId,
                ManagerId = employeeEdit.ManagerId,
                IsActive = employeeEdit.IsActive,
                SalesDivisionId = employeeEdit.SalesDivisionId,
                SalesAreaId = employeeEdit.SalesAreaId,
                SalesBaseId = employeeEdit.SalesBaseId,
                CompanyId = employeeEdit.CompanyId
            };
            var employeeHistory = new EmployeeHistory
            {
                EmployeeId = model.Id,
                EmployeeCode = model.EmployeeCode,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Address = model.Address,
                PostOfficeId = model.PostOfficeId,
                Email = model.Email,
                Mobile = model.Mobile,
                Phone = model.Phone,
                DateOfJoin=model.DateOfJoin,
                DepartmentId = model.DepartmentId,
                BloodGroup = model.BloodGroup,
                ImageId = model.ImageId,
                DesignationId = model.DesignationId,
                ManagerId = model.ManagerId,
                IsActive = model.IsActive,
                SalesDivisionId = model.SalesDivisionId,
                SalesAreaId = model.SalesAreaId,
                SalesBaseId = model.SalesBaseId,
                CompanyId = model.CompanyId
            };
            List<EmployeeSalesLocationVm> salesLocation = new List<EmployeeSalesLocationVm>();
            foreach (var item in employeeEdit.SalesLocation)
            {
                if (item.Id >0)
                {
                    var addsalesLocation = new EmployeeSalesLocationVm
                    {
                        Id=item.Id,
                        ActionTypeId=item.ActionTypeId,
                        DivisionId = item.DivisionId,
                        AreaId = item.AreaId,
                        BaseId = item.BaseId,
                        UpdatedBy = employeeEdit.UpdatedBy,
                        UpdateOn = employeeEdit.UpdatedOn,
                    };
                    salesLocation.Add(addsalesLocation);
                }
                else
                {
                    var sLocation = new EmployeeSalesLocationVm
                    {
                        ActionTypeId=item.ActionTypeId,
                        DivisionId = item.DivisionId,
                        AreaId = item.AreaId,
                        BaseId = item.BaseId,
                        CreatedBy = employeeEdit.CreatedBy,
                        CreatedOn = employeeEdit.CreatedOn
                    };
                    salesLocation.Add(sLocation);
                }
                
                
            }

            var repository = _employeeRepository.EmployeeUpdate(empEdit, employeeHistory,salesLocation);
            return repository;
        }
        public List<EmployeeVm> GetEmployeeHistory(int id)
        {
            var empHistory = _employeeRepository.GetEmployeeHistory(id).Select(d => new EmployeeVm
            {
                Id=d.Id,
                EmployeeCode = d.EmployeeCode,
                EmployeeId = d.EmployeeId,
                FullName = d.FirstName + " " + d.LastName,
                DeptName = d.Department.DepartmentName,
                Designation = d.Designation.DesignationName,
                Email = d.Email,
                Mobile = d.Mobile,
                DateOfJoin=d.DateOfJoin,
                ManagerName = d.ManagerId == null ? "" : d.Employee.FirstName + " " + d.Employee.LastName,
                BloodGroup = d.BloodGroup,
                SalesDivision = d.SalesDivision == null ? "" : d.SalesDivision.SalesDivisionName,
                SalesArea = d.SalesArea == null ? "" : d.SalesArea.SalesAreaName,
                SalesBase = d.SalesBase == null ? "" : d.SalesBase.SalesBaseName,
                SalesLocation = d.EmployeeSalesLocationHistory.Select(m => new EmployeeSalesLocationVm
                {
                    DivisionName = m.SalesDivisionId > 0 ? m.SalesDivision.SalesDivisionName : "N/A",
                    AreaName = m.SalesAreaId > 0 ? m.SalesArea.SalesAreaName : "N/A",
                    BaseName = m.SalesBaseId > 0 ? m.SalesBase.SalesBaseName : "N/A",
                    CreatedOn=m.CreatedOn
                }).ToList()

            }).OrderBy(o => o.EmployeeCode).ToList();
            return empHistory;
        }
        public List<SalesAreaVm> GetSalesAreaList()
        {
            var areaList = _employeeRepository.GetSalesAreaList().Select(d => new SalesAreaVm
            {
                Id = d.Id,
                SalesAreaName = d.SalesAreaName,
                SalesDivisionName = d.SalesDivision.SalesDivisionName,
            }).OrderBy(m => m.SalesAreaName).ToList();
            return areaList;
        }

        public List<SalesBaseVm> GetSalesBaseList()
        {
            var baseList = _employeeRepository.GetSalesBaseList().Select(d => new SalesBaseVm
            {
                Id = d.Id,
                SalesBaseName = d.SalesBaseName,
                SalesAreaName = d.SalesArea.SalesAreaName
            }).OrderBy(m => m.SalesBaseName).ToList();
            return baseList;
        }
        public SalesArea AddSalesArea(SalesArea salesArea)
        {
            var model = new SalesArea
            {
                SalesDivisionId = salesArea.SalesDivisionId,
                SalesAreaName = salesArea.SalesAreaName
            };
            var area = _employeeRepository.AddSalesArea(model);
            return area;
        }
        public SalesBase AddSalesBase(SalesBase salesBase)
        {
            var model = new SalesBase
            {
                SalesBaseName = salesBase.SalesBaseName,
                SalesAreaId = salesBase.SalesAreaId
            };
            var sBase = _employeeRepository.AddSalesBase(model);
            return sBase;
        }

        public List<SalesAreaVm> GetSalesAreaById(int Id)
        {
            var area = _employeeRepository.GetSalesAreaById(Id).Select(d => new SalesAreaVm
            {
                Id = d.Id,
                SalesAreaName = d.SalesAreaName,
                SalesDivisionId = d.SalesDivisionId
            }).OrderBy(m => m.SalesAreaName).ToList();
            return area;

        }
        public List<SalesBaseVm> GetSalesBaseById(int Id)
        {
            var sBase = _employeeRepository.GetSalesBaseById(Id).Select(d => new SalesBaseVm
            {
                Id = d.Id,
                SalesBaseName = d.SalesBaseName,
                SalesAreaId = d.SalesAreaId
            }).OrderBy(m => m.SalesBaseName).ToList();
            return sBase;
        }

        public SalesArea UpdateSalesArea(SalesArea sArea)
        {
            var model = _ppsDbContext.SalesArea.FirstOrDefault(m => m.Id == sArea.Id);
            if (model == null)
            {
                throw new KeyNotFoundException($"Area Id: {sArea.Id} does not exist.");
            }
            var area = new SalesArea
            {
                Id = sArea.Id,
                SalesAreaName = sArea.SalesAreaName,
                SalesDivisionId = sArea.SalesDivisionId
            };
            return _employeeRepository.UpdateSalesArea(area);
        }
        public SalesBase UpdateSalesBase(SalesBase sBase)
        {
            var model = _ppsDbContext.SalesBase.FirstOrDefault(m => m.Id == sBase.Id);
            if (model == null)
            {
                throw new KeyNotFoundException($"Area Id: {sBase.Id} does not exist.");
            }
            var saleBase = new SalesBase
            {
                Id = sBase.Id,
                SalesBaseName = sBase.SalesBaseName,
                SalesAreaId = sBase.SalesAreaId
            };
            return _employeeRepository.UpdateSalesBase(saleBase);
        }

        public List<EmployeeSalesLocationVm> GetSalesLocationByEmployeeId(int Id)
        {
            var empSaleslocation = _employeeRepository.GetSalesLocationByEmployeeId(Id).Select(d => new EmployeeSalesLocationVm
            {
                Id = d.Id,
                EmployeeId = d.EmployeeId,
                DivisionId = d.DivisionId,
                DivisionName = d.DivisionId > 0 ? d.SalesDivision.SalesDivisionName : "N/A",
                AreaId = (int)d.AreaId,
                AreaName = d.AreaId > 0 ? d.SalesArea.SalesAreaName : "N/A",
                BaseId = (int)d.BaseId,
                BaseName = d.BaseId > 0 ? d.SalesBase.SalesBaseName : "N/A",
                CreatedOn=d.CreatedOn
            }).ToList();
            return empSaleslocation;
        }

        
        public List<EmployeeSalesLocationVm> GetSalesLocationHistoryByEmployeeId(int Id)
        {
            var salesLocationByEmployeeId = _employeeRepository.GetSalesLocationHistoryByEmployeeId(Id).Select(d => new EmployeeSalesLocationVm
            {
                EmployeeId = d.EmployeeId,
                EmployeeHistoryId=d.EmployeeHistoryId,
                DivisionName = d.SalesDivisionId > 0 ? d.SalesDivision.SalesDivisionName : "N/A",
                AreaName = d.SalesAreaId > 0 ? d.SalesArea.SalesAreaName : "N/A",
                BaseName = d.SalesBaseId > 0 ? d.SalesBase.SalesBaseName : "N/A",
                CreatedOn = d.CreatedOn
            }).ToList();
            return salesLocationByEmployeeId;
        }
    }
}