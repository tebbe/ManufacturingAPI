using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPS.API.Shared.Enums;
using PPS.API.Shared.ViewModel.Employee;
using PPS.API.Shared.ViewModel.Process;
using PPS.Data.Edmx;
using PPS.Data.Repositories;
using PPS.Data.RepositoryInterfaces;

namespace PPS.Shared.Service.Operation.Service
{
    public class MonthlySalesProcess : IMonthlySalesProcess
    {
        private readonly ISalesRepository _salesRepository;
        private readonly IEmployeeRepository _employeeRepository;
        
        public MonthlySalesProcess()
        {
            _salesRepository = new SalesRepository();
            _employeeRepository = new EmployeeRepository();
        }

        public async Task<List<MonthlySalesProcessVm>> GetMonthlySalesProcess()
        {
            return _salesRepository.GetMonthlyProcessing()
                .Select(x =>
                new MonthlySalesProcessVm
                {
                    Id = x.Id,
                    Month = x.Month,
                    Year = x.Year,
                    CreatedBy = x.User.FirstName + " " + x.User.LastName,
                    CreatedOn = x.CreatedOn,
                    ReprocessedBy = x.ReprocessedBy != null ? x.User1.FirstName + " " + x.User1.LastName : string.Empty,
                    ReprocessedOn = x.ReprocessedOn
                })
                .ToList();
        }

        public async Task<bool> ProcessMonthlyAchievement(int month, int year, int userId, bool reprocess)
        {
            var montlyProcessing = _salesRepository.GetMonthlyProcessing().FirstOrDefault(x => x.Month == month && x.Year == year);
            if(montlyProcessing != null && reprocess == false)
            {
                throw new Exception("Monthly procesing has already been processed for " + month + "/" + year);
            }
            if (montlyProcessing == null)
            {                
                montlyProcessing = new MonthlyProcessing
                {
                    Month = month,
                    Year = year,
                    CreatedBy = userId,
                    CreatedOn = DateTime.Now,
                    ProcessingTypeId = (int)ProcessingTypeEnum.MonthlySalesProcessing
                };
            }
            else
            {
                montlyProcessing.ReprocessedBy = userId;
                montlyProcessing.ReprocessedOn = DateTime.Today;
            }
            var monthlySalesTarget = _salesRepository.GetEmployeeSalesTargetMonthly()
                .Where(x => x.SalesMonth == month && x.SalesYear == year)
                .ToList();

            var monthlySalesByEmplyee = _salesRepository.GetDemandOrderTransaction()
                .Where(x => x.TransactionDate.Month == month && x.TransactionDate.Year == year)
                .GroupBy(g => g.DemandOrder.EmployeeId)
                .Select(y => new EmployeeMonthlySales
                {
                    EmployeeId = y.Key,
                    TotalSales = y.Select(m => m.TransactionAmount).Sum()
                })
                .ToList();

            var headOfSales = 18;
            var salesHierarchy = _employeeRepository.GetEmployeeHierarchy(headOfSales);
            UpdateSalesAchievement(salesHierarchy, monthlySalesTarget, monthlySalesByEmplyee);
            // update achievement for the sales person
            monthlySalesTarget.ForEach(s =>
            {
                if (s.Achievement == null)
                {
                    s.Achievement = 0;
                }
                else if (s.Percentage == null)
                {
                    s.Percentage = 0;
                }
            });

            return true;
            //return await _salesRepository.ProcessMonthlyAchievement(montlyProcessing, monthlySalesTarget, reprocess, userId, year, month);

            //return await salesRepository.ProcessMonthlyAchievement(montlyProcessing, monthlySalesTarget, reprocess);
        }

        private double UpdateSalesAchievement(EmployeeDetailVm employeeDetailVm, List<EmployeeSalesTargetMonthly> monthlySalesTarget, List<EmployeeMonthlySales> employeeMonthlySales)
        {
            if(employeeDetailVm.Employees.Count == 0)
            {
                var monthlySales = employeeMonthlySales.FirstOrDefault(e => e.EmployeeId == employeeDetailVm.Id);
                if(monthlySales == null)
                {
                    return 0;
                }
                return monthlySales.TotalSales;
            }
            var totalSales = 0.0;
            employeeDetailVm.Employees.ForEach(x => {
                var total = UpdateSalesAchievement(x, monthlySalesTarget, employeeMonthlySales);
                totalSales = totalSales + total;
                var childEmp = monthlySalesTarget.FirstOrDefault(e => e.EmployeeId == x.Id);
                if (childEmp != null)
                {
                    var achievement = total;
                    var totalTarget = childEmp.TeamTarget + childEmp.SalesTarget;
                    if (totalTarget > 0)
                    {
                        var tp = (achievement * 100) / (double)totalTarget;
                        childEmp.Achievement = (decimal)achievement;
                        childEmp.Percentage = (decimal)tp;
                    }
                    else
                    {
                        childEmp.Achievement = 0;
                        childEmp.Percentage = 0;
                    }
                }
            });
            var emp = monthlySalesTarget.FirstOrDefault(e => e.EmployeeId == employeeDetailVm.Id);
            if (emp != null)
            {
                var ownMonthlySales = employeeMonthlySales.FirstOrDefault(e => e.EmployeeId == emp.EmployeeId);
                if (ownMonthlySales != null)
                {
                    totalSales = totalSales + ownMonthlySales.TotalSales;
                }
                var achievement = totalSales;
                var totalTarget = emp.TeamTarget + emp.SalesTarget;
                if (totalTarget > 0)
                {
                    var tp = (achievement * 100) / (double)totalTarget;
                    emp.Achievement = (decimal)achievement;
                    emp.Percentage = (decimal)tp;
                }
                else
                {
                    emp.Achievement = 0;
                    emp.Percentage = 0;
                }
            }
            return totalSales;
        }
        private class EmployeeMonthlySales
        {
            public int EmployeeId { get; set; }
            public double TotalSales { get; set; }
        }
    }
}