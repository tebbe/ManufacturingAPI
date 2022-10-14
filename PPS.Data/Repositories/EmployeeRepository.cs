using PPS.Data.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using PPS.API.Shared.Enums;
using PPS.API.Shared.ViewModel.Employee;
using PPS.Data.Edmx;
using PPS.API.Shared.Extensions;
using PPS.API.Shared.RequestVm;
using System.Data.Entity;
using PPS.API.Shared.ViewModel.Sales;

namespace PPS.Data.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly PPSDbContext _ppsDbContext;
        public EmployeeRepository()
        {
            _ppsDbContext = new PPSDbContext();
        }

        public List<EmployeeVm> GetEmployee()
        {
            var employee = _ppsDbContext.Employee
                .Where(x => x.IsActive == true)
                .Select(x =>
                    new EmployeeVm
                    {
                        Id = x.Id,
                        FirstName = x.FirstName.Trim(),
                        LastName = x.LastName.Trim(),
                        FullName = x.FirstName + " " + x.LastName,
                        Email = x.Email,
                        Designation = x.Designation.DesignationName,
                        EmployeeCode = x.EmployeeCode
                    }).ToList();

            return employee;
        }
        public IQueryable<EmployeeType> GetEmployeeType()
        {
            return _ppsDbContext.EmployeeType;
        }
        public List<EmployeeVm> GetEmployee(int employeeId)
        {
            List<EmployeeVm> employee;
            var departmentId = _ppsDbContext.Employee.FirstOrDefault(x => x.Id == employeeId)?.DepartmentId;

            if (departmentId == (int)DepartmentEnum.SalesAndMarketing)
            {
                var managedEmployeeIdList = GetManagedEmployee(employeeId).Select(x => x.Item1);
                employee = _ppsDbContext.Employee
                    .Where(x => managedEmployeeIdList.Contains(x.Id))
                    .Select(x =>
                        new EmployeeVm
                        {
                            Id = x.Id,
                            FirstName = x.FirstName,
                            LastName = x.LastName,
                            FullName = x.FirstName + " " + x.LastName,
                            Designation = x.Designation.DesignationName,
                            EmployeeCode = x.EmployeeCode,
                            ManagerId = x.ManagerId,
                            SalesDivisionId = x.SalesDivisionId ?? 0,
                            SalesAreaId = x.SalesAreaId ?? 0,
                            SalesBaseId = x.SalesBaseId ?? 0

                        }).ToList();
            }
            else
            {
                employee = _ppsDbContext.Employee.Where(x => x.DepartmentId == (int)DepartmentEnum.SalesAndMarketing).Select(x =>
                      new EmployeeVm
                      {
                          Id = x.Id,
                          FirstName = x.FirstName,
                          LastName = x.LastName,
                          FullName = x.FirstName + " " + x.LastName,
                          Designation = x.Designation.DesignationName,
                          EmployeeCode = x.EmployeeCode,
                          ManagerId = x.ManagerId,
                          SalesDivisionId = x.SalesDivisionId ?? 0,
                          SalesAreaId = x.SalesAreaId ?? 0,
                          SalesBaseId = x.SalesBaseId ?? 0
                      }).ToList();
            }
            return employee;
        }

        public List<EmployeeVm> GetSalesEmployeeWithSalesTargetByMonth(EmployeeRequestVm employeeRequestVm)
        {
            List<EmployeeVm> employee;
            var departmentId = _ppsDbContext.Employee.FirstOrDefault(x => x.Id == employeeRequestVm.EmployeeId)?.DepartmentId;

            if (departmentId == (int)DepartmentEnum.SalesAndMarketing)
            {
                var managedEmployeeIdList = GetManagedEmployee(employeeRequestVm.EmployeeId).Select(x => x.Item1);
                employee = _ppsDbContext.Employee
                    .Where(x => managedEmployeeIdList.Contains(x.Id))
                    .Select(x =>
                        new EmployeeVm
                        {
                            Id = x.Id,
                            FirstName = x.FirstName,
                            LastName = x.LastName,
                            FullName = x.FirstName + " " + x.LastName,
                            Designation = x.Designation.DesignationName,
                            EmployeeCode = x.EmployeeCode,
                            ManagerId = x.ManagerId,
                        }).ToList();
            }
            else
            {
                employee = _ppsDbContext.Employee.Where(x => x.DepartmentId == (int)DepartmentEnum.SalesAndMarketing).Select(x =>
                    new EmployeeVm
                    {
                        Id = x.Id,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        FullName = x.FirstName + " " + x.LastName,
                        Designation = x.Designation.DesignationName,
                        EmployeeCode = x.EmployeeCode,
                        ManagerId = x.ManagerId,
                    }).ToList();
            }

            employee.ForEach(x =>
            {
                x.SalesTarget = _ppsDbContext.EmployeeSalesTargetMonthly.FirstOrDefault(y =>
                                    x.Id == y.EmployeeId && y.SalesYear == employeeRequestVm.Year &&
                                    y.SalesMonth == employeeRequestVm.Month)?.SalesTarget ?? 0;
                x.TeamTarget = _ppsDbContext.EmployeeSalesTargetMonthly.FirstOrDefault(y =>
                                   x.Id == y.EmployeeId && y.SalesYear == employeeRequestVm.Year &&
                                   y.SalesMonth == employeeRequestVm.Month)?.TeamTarget ?? 0;
            });

            return employee;
        }

        public EmployeeDetailVm GetEmployeeHierarchy(int employeeId)
        {
            var manager = _ppsDbContext.Employee.FirstOrDefault(x => x.Id == employeeId);
            var empHierarchy = CreateEmployeeHierarchy(manager);
            return empHierarchy;
        }
        public List<Tuple<int, int>> GetManagedEmployee(int employeeId)
        {
            var list = new List<Tuple<int, int>>();
            var manager = _ppsDbContext.Employee.FirstOrDefault(x => x.Id == employeeId);
            list = CreateManagedEmployee(manager, list);
            return list;
        }
        private EmployeeDetailVm CreateEmployeeHierarchy(Employee manager)
        {
            if (manager == null || manager.IsActive == false)
            {
                return null;
            }
            var empHierarchy = new EmployeeDetailVm
            {
                Id = manager.Id,
                EmployeeCode = manager.EmployeeCode,
                FullName = StringExtension.ToFullName(manager.FirstName, manager.LastName),
                DeptName = manager.Department?.DepartmentName,
                Designation = manager.Designation?.DesignationName,
                //SalesTarget = (double)(manager.CurrentSalesTarget ?? 0),
                Address = manager.Address + ", PO: " + manager.PostOffice?.PostOfficeName + ", PS: " + manager.PostOffice?.PoliceStation?.PoliceStationName + ", District: " + manager.PostOffice?.PoliceStation?.District?.DistrictName,
                Employees = new List<EmployeeDetailVm>()
            };
            manager.Employee1.ToList().ForEach(emp =>
            {
                var mgr = CreateEmployeeHierarchy(emp);
                empHierarchy.Employees.Add(mgr);
            });

            return empHierarchy;
        }
        private List<Tuple<int, int>> CreateManagedEmployee(Employee manager, List<Tuple<int, int>> list)
        {
            if (manager == null)
            {
                return list;
            }
            list.Add(Tuple.Create(manager.Id, manager.EmployeeCode));
            manager.Employee1.ToList().ForEach(emp =>
            {
                list.Add(Tuple.Create(emp.Id, emp.EmployeeCode));
                CreateManagedEmployee(emp, list);
            });
            return list;
        }

        public IQueryable<Employee> GetEmployeeList()
        {
            return _ppsDbContext.Employee;
        }
        public IQueryable<SalesDivision> GetSalesDivision()
        {
            return _ppsDbContext.SalesDivision;
        }
        public IQueryable<SalesArea> GetSalesArea(int id)
        {
            return _ppsDbContext.SalesArea.Where(m=>m.SalesDivisionId==id);
        }
        public IQueryable<SalesBase> GetSalesBase(int id)
        {
            return _ppsDbContext.SalesBase.Where(m=>m.SalesAreaId==id);
        }
        public IQueryable<Department> GetDepartment()
        {
            return _ppsDbContext.Department;
        }
        public IQueryable<Designation> GetDesignation()
        {
            return _ppsDbContext.Designation;
        }

        public Employee AddNewEmployee(Employee employee,List<EmployeeSalesLocation> sLocation)
        {
            using(var db=new PPSDbContext())
            {
                db.Employee.Add(employee);
                db.SaveChanges();
                if (employee.Id > 0)
                {
                    foreach (var item in sLocation)
                    {
                        item.EmployeeId = employee.Id;
                        db.EmployeeSalesLocation.Add(item);
                        db.SaveChanges();
                    }
                }
                
                return employee;
            }
        }
        public Employee GetEmployeeById(int Id)
        {
            return _ppsDbContext.Employee.FirstOrDefault(m => m.Id==Id);
        }
        public Employee EmployeeUpdate(Employee employeeEdit,EmployeeHistory empHistory, List<EmployeeSalesLocationVm> sLocation)
        {
          
            using (var db=new PPSDbContext())
            {
                db.EmployeeHistory.Add(empHistory);
                db.Employee.Attach(employeeEdit);
                db.Entry(employeeEdit).State = EntityState.Modified;
                db.SaveChanges();
                foreach (var item in sLocation)
                {
                    
                    if (item.ActionTypeId == (int)ActionTypeEnum.AddNew)
                    {
                        var sModel = new EmployeeSalesLocation
                        {
                            EmployeeId = employeeEdit.Id,
                            DivisionId=item.DivisionId,
                            AreaId = item.AreaId,
                            BaseId = item.BaseId,
                            CreatedBy =item.CreatedBy,
                            CreatedOn = item.CreatedOn
                        };
                       
                        db.EmployeeSalesLocation.Add(sModel);
                    }
                    else if (item.ActionTypeId == (int)ActionTypeEnum.Update)
                    {
                        var data = db.EmployeeSalesLocation.FirstOrDefault(m => m.Id == item.Id);
                        if (data != null)
                        {
                            EmployeeSalesLocationHistory salesLocationHistory = new EmployeeSalesLocationHistory
                            {
                                EmployeeHistoryId= empHistory.Id,
                                EmployeeId=data.EmployeeId,
                                SalesDivisionId=data.DivisionId,
                                SalesAreaId=data.AreaId,
                                SalesBaseId=data.BaseId,
                                CreatedBy=item.UpdatedBy,
                                CreatedOn=item.UpdateOn
                            };
                            db.EmployeeSalesLocationHistory.Add(salesLocationHistory);
                            data.DivisionId = item.DivisionId;
                            data.AreaId = item.AreaId;
                            data.BaseId = item.BaseId;
                            data.UpdatedBy = item.UpdatedBy;
                            data.UpdatedOn = item.UpdateOn;
                            db.EmployeeSalesLocation.Attach(data);
                            db.Entry(data).State = EntityState.Modified;
                        }

                    }
                    else if (item.ActionTypeId == (int)ActionTypeEnum.Delete)
                    {
                        var data = db.EmployeeSalesLocation.FirstOrDefault(m => m.Id == item.Id);
                        if (data != null)
                        {
                            EmployeeSalesLocationHistory salesLocationHistory = new EmployeeSalesLocationHistory
                            {
                                EmployeeHistoryId = empHistory.Id,
                                EmployeeId = data.EmployeeId,
                                SalesDivisionId = data.DivisionId,
                                SalesAreaId = data.AreaId,
                                SalesBaseId = data.BaseId,
                                CreatedBy = item.UpdatedBy,
                                CreatedOn = item.UpdateOn
                            };
                            db.EmployeeSalesLocationHistory.Add(salesLocationHistory);
                            db.EmployeeSalesLocation.Remove(data);
                        }
                    }

                }
                db.SaveChanges();
                return  employeeEdit;
            };
        }
        public IQueryable<EmployeeHistory> GetEmployeeHistory(int Id)
        {
            return _ppsDbContext.EmployeeHistory.Where(m=>m.EmployeeId==Id);
        }

        public IQueryable<SalesArea> GetSalesAreaList()
        {
            return _ppsDbContext.SalesArea;
        }
        public IQueryable<SalesBase> GetSalesBaseList()
        {
            return _ppsDbContext.SalesBase;
        }

        public SalesArea AddSalesArea(SalesArea salesArea)
        {
            using(var db=new PPSDbContext())
            {
                db.SalesArea.Add(salesArea);
                db.SaveChanges();
                return salesArea;
            }
        }
        public SalesBase AddSalesBase(SalesBase salesBase)
        {
            using (var db = new PPSDbContext())
            {
                db.SalesBase.Add(salesBase);
                db.SaveChanges();
                return salesBase;
            }
        }
        public IQueryable<SalesArea> GetSalesAreaById(int id)
        {
            return _ppsDbContext.SalesArea.Where(m=>m.Id==id);
        }
        public IQueryable<SalesBase> GetSalesBaseById(int id)
        {
           return  _ppsDbContext.SalesBase.Where(m=>m.Id==id);
        }
        public SalesArea UpdateSalesArea(SalesArea sArea)
        {
            using(var db=new PPSDbContext())
            {
                db.SalesArea.Attach(sArea);
                db.Entry(sArea).State = EntityState.Modified;
                db.SaveChanges();
                return sArea;
            }
        }
        public SalesBase UpdateSalesBase(SalesBase sBase)
        {
            using (var db = new PPSDbContext())
            {
                db.SalesBase.Attach(sBase);
                db.Entry(sBase).State = EntityState.Modified;
                db.SaveChanges();
                return sBase;
            }
        }
        public IQueryable<EmployeeSalesLocation> GetSalesLocationByEmployeeId(int Id)
        {
            return _ppsDbContext.EmployeeSalesLocation.Where(m => m.EmployeeId == Id);
        }
        public IQueryable<EmployeeSalesLocationHistory> GetSalesLocationHistoryByEmployeeId(int Id)
        {
            return _ppsDbContext.EmployeeSalesLocationHistory.Where(m => m.EmployeeId == Id);
        }
       
      
    }
}