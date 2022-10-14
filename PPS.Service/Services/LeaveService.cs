using PPS.API.Shared;
using PPS.API.Shared.Constants;
using PPS.API.Shared.Enums;
using PPS.API.Shared.ViewModel;
using PPS.API.Shared.ViewModel.Employee;
using PPS.API.Shared.ViewModel.Leave;
using PPS.Data.Edmx;
using PPS.Data.Repositories;
using PPS.Data.RepositoryInterfaces;
using PPS.Service.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.Service.Services
{
    public class LeaveService : ILeaveInterface
    {
        private readonly PPSDbContext _ppsDbContext;
        private readonly ILeaveRepository _leaveRepository;
        public LeaveService()
        {
            _leaveRepository = new LeaveRepository();
            _ppsDbContext = new PPSDbContext();
        }

        public List<EmployeeLeaveVm> GetEmployeeLeaveList()
        {
            var leaveList = _leaveRepository.GetEmployeeLeaveList().Select(d => new EmployeeLeaveVm
            {
                Id = d.Id,
                EmployeeId = d.EmployeeId,
                EmployeeCode = d.Employee.EmployeeCode,
                FirstName = d.Employee.FirstName,
                LastName = d.Employee.LastName,
                CompanyName = d.Employee.Company.Name,
                DepartmentName = d.Employee.Department.DepartmentName,
                DesignationName = d.Employee.Designation.DesignationName,
                LeaveCategoryId = d.LeaveCategoryId,
                LeaveDays = d.LeaveDays != null ? d.LeaveDays : 0,
                UnpaidLeaveDays=d.UnpaidLeaveDays!=null? d.UnpaidLeaveDays:0,
                ReasonOfLeave = d.ReasonOfLeave,
                FromDate = d.FromDate,
                ToDate = d.ToDate,
                MobileNo = d.MobileNo,
                Address = d.Address,
                DateOfApplication = d.DateOfApplication,
                CreatedOn = d.CreatedOn,
            }).ToList();
            return leaveList;
        }
        public List<LeaveCategoryVm> GetLeaveCategory()
        {
            return _leaveRepository.GetLeaveCategory().Select(d => new LeaveCategoryVm
            {
                Id = d.Id,
                Name = d.Name
            }).ToList();
        }
        public List<EmployeeHierArchyVm> GetEmployeeHierArchy(int employeeId)
        {
            return _leaveRepository.GetEmployee().Where(m => m.ManagerId == employeeId || m.Id == employeeId)
                .Select(d => new EmployeeHierArchyVm
                {
                    Id = d.Id,
                    FullName = d.FirstName + " " + d.LastName + "(" + d.EmployeeCode.ToString() + ")"
                }).ToList();
        }
        public List<EmployeeHierArchyVm> GetEmployeeById(int employeeId)
        {
            return _leaveRepository.GetEmployee().Where(m => m.Id == employeeId)
                .Select(d => new EmployeeHierArchyVm
                {
                    Id = d.Id,
                    FullName = d.FirstName + " " + d.LastName + "(" + d.EmployeeCode.ToString() + ")"
                }).ToList();
        }
        public List<EmployeeLeaveVm> GetEmployeeLeaveDetailsByEmployeeId(int employeeId)
        {

            return _leaveRepository.GetEmployee().Where(m => m.Id == employeeId)
                .Select(d => new EmployeeLeaveVm
                {
                    EmployeeId = d.Id,
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    EmployeeCode = d.EmployeeCode,
                    DepartmentName = d.Department.DepartmentName,
                    CompanyName = d.Company.Name,
                    DesignationName = d.Designation.DesignationName,

                    EmployeeLeaveDetails = (d.EmployeeTypeId == (int)EmployeeTypeEnum.FullTime) ? d.EmployeeLeave.Where(m => m.EmployeeId == d.Id).Select(p => new EmployeeLeaveDetailsVm
                    {
                        EarnLeave = d.EmployeeLeave.Where(x => x.LeaveCategoryId == (int)LeaveCategoryType.EarnLeave).Any() ? d.EmployeeLeave.Where(x => x.LeaveCategoryId == (int)LeaveCategoryType.EarnLeave).Sum(x => (int)x.LeaveDays) : 0,
                        CasualLeave = d.EmployeeLeave.Where(x => x.LeaveCategoryId == (int)LeaveCategoryType.CasualLeave).Any() ? d.EmployeeLeave.Where(x => x.LeaveCategoryId == (int)LeaveCategoryType.CasualLeave).Sum(x => (int)x.LeaveDays) : 0,
                        SickLeave = d.EmployeeLeave.Where(x => x.LeaveCategoryId == (int)LeaveCategoryType.SickLeave).Any() ? d.EmployeeLeave.Where(x => x.LeaveCategoryId == (int)LeaveCategoryType.SickLeave).Sum(x => (int)x.LeaveDays) : 0,
                        OtherLeave = d.EmployeeLeave.Where(x => x.LeaveCategoryId == (int)LeaveCategoryType.OthersLeave).Any() ? d.EmployeeLeave.Where(x => x.LeaveCategoryId == (int)LeaveCategoryType.OthersLeave).Sum(x => (int)x.LeaveDays) : 0,
                        TotalPaidLeave = d.EmployeeLeave.Where(x => x.EmployeeId == d.Id && x.LeaveDays > 0).Any() ? d.EmployeeLeave.Where(x => x.EmployeeId == d.Id && x.LeaveDays > 0).Sum(x => (int)x.LeaveDays) : 0,
                        TotalUnpaidLeave = d.EmployeeLeave.Where(x => x.EmployeeId == d.Id && x.UnpaidLeaveDays > 0).Any() ? d.EmployeeLeave.Where(x => x.EmployeeId == d.Id && x.UnpaidLeaveDays > 0).Sum(x => (int)x.UnpaidLeaveDays) : 0,

                    }).FirstOrDefault() : d.EmployeeLeave.Where(m => m.EmployeeId == d.Id).Select(p => new EmployeeLeaveDetailsVm
                    {
                        EarnLeave = d.EmployeeLeave.Where(x => x.LeaveCategoryId == (int)LeaveCategoryType.EarnLeave).Any() ? d.EmployeeLeave.Where(x => x.LeaveCategoryId == (int)LeaveCategoryType.EarnLeave).Sum(x => (int)x.LeaveDays) : 0,
                        CasualLeave = d.EmployeeLeave.Where(x => x.LeaveCategoryId == (int)LeaveCategoryType.CasualLeave).Any() ? d.EmployeeLeave.Where(x => x.LeaveCategoryId == (int)LeaveCategoryType.CasualLeave).Sum(x => (int)x.LeaveDays) : 0,
                        SickLeave = d.EmployeeLeave.Where(x => x.LeaveCategoryId == (int)LeaveCategoryType.SickLeave).Any() ? d.EmployeeLeave.Where(x => x.LeaveCategoryId == (int)LeaveCategoryType.SickLeave).Sum(x => (int)x.LeaveDays) : 0,
                        OtherLeave = d.EmployeeLeave.Where(x => x.LeaveCategoryId == (int)LeaveCategoryType.OthersLeave).Any() ? d.EmployeeLeave.Where(x => x.LeaveCategoryId == (int)LeaveCategoryType.OthersLeave).Sum(x => (int)x.LeaveDays) : 0,
                        TotalPaidLeave = d.EmployeeLeave.Where(x => x.EmployeeId == d.Id && x.LeaveDays > 0).Any() ? d.EmployeeLeave.Where(x => x.EmployeeId == d.Id && x.LeaveDays > 0).Sum(x => (int)x.LeaveDays) : 0,
                        TotalUnpaidLeave = d.EmployeeLeave.Where(x => x.EmployeeId == d.Id && x.UnpaidLeaveDays > 0).Any() ? d.EmployeeLeave.Where(x => x.EmployeeId == d.Id && x.UnpaidLeaveDays > 0).Sum(x => (int)x.UnpaidLeaveDays) : 0,
                    }).FirstOrDefault(),


                }).ToList();
        }
        public List<EmployeeLeaveVm> GetEmployeeHierArchyLeaveList(int employeeId, int? status)
        {
            List<EmployeeLeaveVm> employeeLeaveList = new List<EmployeeLeaveVm>();
            var employeeList = _leaveRepository.GetEmployee().Where(m => m.ManagerId == employeeId).ToList();
            foreach (var item in employeeList)
            {
                var leaveListByEmployeeId = _leaveRepository.GetLeaveListByEmployeeIdAndStatus(item.Id, status);
                if (leaveListByEmployeeId.Count() > 0)
                {
                    foreach (var leave in leaveListByEmployeeId)
                    {
                        EmployeeLeaveVm employeeLeaveDetails = new EmployeeLeaveVm
                        {
                            Id = leave.Id,
                            FirstName = leave.Employee.FirstName,
                            LastName = leave.Employee.LastName,
                            CompanyName = leave.Employee.Company.Name,
                            DepartmentName = leave.Employee.Department.DepartmentName,
                            DesignationName = leave.Employee.Designation.DesignationName,
                            LeaveDays = leave.LeaveDays != null ? leave.LeaveDays : 0,
                            UnpaidLeaveDays = leave.UnpaidLeaveDays != null ? leave.UnpaidLeaveDays : 0,
                            NatureOfLeaveName = leave.LeaveCategory.Name,
                            FromDate = leave.FromDate,
                            ToDate = leave.ToDate,
                            MobileNo = leave.MobileNo,
                            Address = leave.Address,
                            CreatedOn = leave.CreatedOn,
                            DateOfApplication = leave.DateOfApplication
                        };
                        employeeLeaveList.Add(employeeLeaveDetails);
                    }
                }
            }
            return employeeLeaveList;
        }


        public EmployeeLeave SaveEmployeeLeave(EmployeeLeaveVm employeeLeaveVm)
        {
            var employeeData = _ppsDbContext.Employee.Where(m => m.Id == employeeLeaveVm.EmployeeId).FirstOrDefault();
            if (employeeData == null)
            {
                throw new KeyNotFoundException($"Employee  Id: {employeeData.Id} does not exist.");
            }
            var totalBalance = 0;
            int yearlyLeave = _ppsDbContext.CompanyLeave.Where(m => m.CompanyId == employeeData.CompanyId).Select(m => m.TotalLeave).FirstOrDefault();
            int totalPaidLeave = _ppsDbContext.EmployeeLeave.Where(m => m.EmployeeId == employeeData.Id && m.LeaveYear == DateTime.Now.Year).Sum(m => m.LeaveDays) ?? 0;
            int unpaidLeave = _ppsDbContext.EmployeeLeave.Where(m => m.EmployeeId == employeeData.Id && m.LeaveYear == DateTime.Now.Year).Sum(m => m.UnpaidLeaveDays) ?? 0;
            EmployeeLeave leaveModel = new EmployeeLeave();
            leaveModel.EmployeeId = employeeData.Id;
            leaveModel.LeaveCategoryId = employeeLeaveVm.LeaveCategoryId;
            if (employeeData.EmployeeTypeId == (int)EmployeeTypeEnum.FullTime)
            {
                int hasUnpaidleave = (totalPaidLeave + (int)employeeLeaveVm.LeaveDays) - yearlyLeave;
                int hasPaidLeave = hasUnpaidleave > 0 ? (int)employeeLeaveVm.LeaveDays - hasUnpaidleave : (int)employeeLeaveVm.LeaveDays;

                if (DateTime.Now.Year == employeeData.JobConfirmationDate.Value.Year)
                {
                    var remainingMonth = LeaveSettingConstant.MonthInAYear - (employeeData.JobConfirmationDate.Value.Month + 1);
                    totalBalance = (int)Math.Round(remainingMonth * LeaveSettingConstant.Monthlylimit);
                    int unpaidleave = (totalPaidLeave + (int)employeeLeaveVm.LeaveDays) - totalBalance;
                    int paidLeave = unpaidleave > 0 ? ((int)employeeLeaveVm.LeaveDays - unpaidleave) : (int)employeeLeaveVm.LeaveDays;
                    if (totalPaidLeave < totalBalance)
                    {
                        leaveModel.LeaveDays = paidLeave;
                        leaveModel.UnpaidLeaveDays = unpaidleave > 0 ? (int?)unpaidleave : null;
                    }
                    else
                    {
                        leaveModel.UnpaidLeaveDays = employeeLeaveVm.LeaveDays;
                    }
                }
                else if (totalPaidLeave < yearlyLeave)
                {
                    leaveModel.LeaveDays = hasPaidLeave;
                    leaveModel.UnpaidLeaveDays = hasUnpaidleave > 0 ? (int?)hasUnpaidleave : null;
                }
                else
                {
                    leaveModel.UnpaidLeaveDays = employeeLeaveVm.LeaveDays;
                }
            }
            else if ((employeeData.EmployeeTypeId == (int)EmployeeTypeEnum.Consultant || employeeData.EmployeeTypeId == (int)EmployeeTypeEnum.Trainee) && (unpaidLeave + employeeLeaveVm.LeaveDays) < LeaveSettingConstant.UnpaidLeaveInAYear)
            {
                leaveModel.UnpaidLeaveDays = employeeLeaveVm.LeaveDays;
            }
            else
            {
                throw new InvalidOperationException($"Unpaid  Leave out of limit.");
            }
            leaveModel.IsApproved = employeeLeaveVm.IsApproved;
            leaveModel.FromDate = employeeLeaveVm.FromDate;
            leaveModel.ToDate = employeeLeaveVm.ToDate;
            leaveModel.ReasonOfLeave = employeeLeaveVm.ReasonOfLeave;
            leaveModel.DateOfApplication = employeeLeaveVm.DateOfApplication;
            leaveModel.Address = employeeLeaveVm.Address;
            leaveModel.LeaveYear = employeeLeaveVm.LeaveYear;
            leaveModel.MobileNo = employeeLeaveVm.MobileNo;
            leaveModel.CreatedBy = employeeLeaveVm.CreatedBy;
            leaveModel.CreatedOn = employeeLeaveVm.CreatedOn;
            leaveModel.ApprovedByDepartmentHead = employeeLeaveVm.ApprovedByDepartmentHead;
            leaveModel.ApprovedByHeadOn = employeeLeaveVm.ApprovedByHeadOn;
            leaveModel.ApprovedByMD = employeeLeaveVm.ApprovedByMD;
            leaveModel.ApprovedByAdminOn = employeeLeaveVm.ApprovedByAdminOn;
            var addLeave = _leaveRepository.SaveEmployeeLeave(leaveModel);
            return addLeave;
        }
        public EmployeeLeaveVm GetEmployeeLeaveById(int Id)
        {
            var leaveDetails = _leaveRepository.GetEmployeeLeaveById(Id);

            EmployeeLeaveVm employeeLeaveDetails = new EmployeeLeaveVm
            {
                Id = leaveDetails.Id,
                EmployeeId = leaveDetails.EmployeeId,
                FirstName = leaveDetails.Employee.FirstName,
                LastName = leaveDetails.Employee.LastName,
                CompanyName = leaveDetails.Employee.Company.Name,
                DepartmentName = leaveDetails.Employee.Department.DepartmentName,
                DesignationName = leaveDetails.Employee.Designation.DesignationName,
                LeaveCategoryId = leaveDetails.LeaveCategoryId,
                NatureOfLeaveName = _ppsDbContext.LeaveCategory.Where(m => m.Id == leaveDetails.LeaveCategoryId).Select(m => m.Name).FirstOrDefault(),
                LeaveDays = leaveDetails.LeaveDays != null ? leaveDetails.LeaveDays : 0,
                UnpaidLeaveDays = leaveDetails.UnpaidLeaveDays != null ? leaveDetails.UnpaidLeaveDays : 0,
                ReasonOfLeave = leaveDetails.ReasonOfLeave,
                FromDate = leaveDetails.FromDate,
                ToDate = leaveDetails.ToDate,
                MobileNo = leaveDetails.MobileNo,
                Address = leaveDetails.Address,
                CreatedOn = leaveDetails.CreatedOn,
                ApprovedByDepartmentHead = leaveDetails.ApprovedByDepartmentHead == null ? false : (bool)leaveDetails.ApprovedByDepartmentHead,
                ApprovedByMD = leaveDetails.ApprovedByMD == null ? false : (bool)leaveDetails.ApprovedByMD,
                DateOfApplication = leaveDetails.DateOfApplication,
                IsApproved = leaveDetails.IsApproved
            };
            return employeeLeaveDetails;
        }
        public List<EmployeeLeaveVm> GetLeaveListByEmployeeId(int employeeId)
        {
            var leaveList = _leaveRepository.GetLeaveListByEmployeeId(employeeId).Select(d => new EmployeeLeaveVm
            {
                Id = d.Id,
                EmployeeId = d.EmployeeId,
                FirstName = d.Employee.FirstName,
                LastName = d.Employee.LastName,
                CompanyName = d.Employee.Company.Name,
                DepartmentName = d.Employee.Department.DepartmentName,
                DesignationName = d.Employee.Designation.DesignationName,
                NatureOfLeaveName = d.LeaveCategory.Name,
                LeaveDays = d.LeaveDays != null ? d.LeaveDays : 0,
                UnpaidLeaveDays = d.UnpaidLeaveDays != null ? d.UnpaidLeaveDays : 0,
                CreatedOn = d.CreatedOn,
                DateOfApplication = d.DateOfApplication
            }).ToList();
            return leaveList;
        }

        public EmployeeLeave UpdateEmployeeLeave(EmployeeLeaveVm employeeLeaveVm)
        {
            var leaveDetails = _ppsDbContext.EmployeeLeave.Where(m => m.Id == employeeLeaveVm.Id).FirstOrDefault();
            var employeeData = _ppsDbContext.Employee.Where(m => m.Id == leaveDetails.EmployeeId).FirstOrDefault();
            if (employeeData == null)
            {
                throw new KeyNotFoundException($"Employee Id: {employeeData.Id} does not exist.");
            }
            var totalBalance = 0;
            int yearlyLeave = _ppsDbContext.CompanyLeave.Where(m => m.CompanyId == employeeData.CompanyId).Select(m => m.TotalLeave).FirstOrDefault();
            int totalPaidLeave = _ppsDbContext.EmployeeLeave.Where(m => m.EmployeeId == employeeData.Id && m.LeaveYear == DateTime.Now.Year).Sum(m => m.LeaveDays) ?? 0;
            int unpaidLeave = _ppsDbContext.EmployeeLeave.Where(m => m.EmployeeId == employeeData.Id && m.LeaveYear == DateTime.Now.Year).Sum(m => m.UnpaidLeaveDays) ?? 0;
            EmployeeLeave model = new EmployeeLeave();

            model.Id = leaveDetails.Id;
            model.EmployeeId = leaveDetails.EmployeeId;
            model.LeaveCategoryId = employeeLeaveVm.LeaveCategoryId;
            if (employeeData.EmployeeTypeId == (int)EmployeeTypeEnum.FullTime)
            {
                int hasUnpaidleave = (totalPaidLeave + (int)employeeLeaveVm.LeaveDays) - yearlyLeave;
                int hasPaidLeave = hasUnpaidleave > 0 ? (int)employeeLeaveVm.LeaveDays - hasUnpaidleave : (int)employeeLeaveVm.LeaveDays;

                if (DateTime.Now.Year == employeeData.JobConfirmationDate.Value.Year)
                {
                    var remainingMonth = LeaveSettingConstant.MonthInAYear - employeeData.JobConfirmationDate.Value.Month;
                    totalBalance = totalBalance = (int)Math.Round(remainingMonth * LeaveSettingConstant.Monthlylimit);
                    int unpaidleave = (totalPaidLeave + (int)employeeLeaveVm.LeaveDays) - totalBalance;
                    int paidLeave = unpaidleave > 0 ? ((int)employeeLeaveVm.LeaveDays - unpaidleave) : (int)employeeLeaveVm.LeaveDays;
                    if (totalPaidLeave < totalBalance)
                    {
                        model.LeaveDays = paidLeave;
                        model.UnpaidLeaveDays = unpaidleave > 0 ? (int?)unpaidleave : null;
                    }
                    else
                    {
                        model.UnpaidLeaveDays = employeeLeaveVm.LeaveDays;
                    }
                }
                else if (totalPaidLeave < yearlyLeave)
                {
                    model.LeaveDays = hasPaidLeave;
                    model.UnpaidLeaveDays = hasUnpaidleave > 0 ? (int?)hasUnpaidleave : null;
                }
                else
                {
                    model.UnpaidLeaveDays = employeeLeaveVm.LeaveDays;
                }
            }
            else if ((employeeData.EmployeeTypeId == (int)EmployeeTypeEnum.Trainee|| employeeData.EmployeeTypeId == (int)EmployeeTypeEnum.Consultant) && (unpaidLeave + employeeLeaveVm.LeaveDays) < LeaveSettingConstant.UnpaidLeaveInAYear)
            {
                model.UnpaidLeaveDays = employeeLeaveVm.LeaveDays;
            }
            else
            {
                throw new InvalidOperationException($"Unpaid  Leave out of limit.");
            }
            model.FromDate = employeeLeaveVm.FromDate;
            model.ToDate = employeeLeaveVm.ToDate;
            model.ReasonOfLeave = employeeLeaveVm.ReasonOfLeave;
            model.DateOfApplication = employeeLeaveVm.DateOfApplication;
            model.Address = employeeLeaveVm.Address;
            model.LeaveYear = leaveDetails.LeaveYear;
            model.MobileNo = employeeLeaveVm.MobileNo;
            model.CreatedBy = leaveDetails.CreatedBy;
            model.CreatedOn = leaveDetails.CreatedOn;
            model.UpdatedBy = employeeLeaveVm.UpdatedBy;
            model.UpdatedOn = employeeLeaveVm.UpdatedOn;
            model.ApprovedByDepartmentHead = leaveDetails.ApprovedByDepartmentHead == null ? false : leaveDetails.ApprovedByDepartmentHead;
            model.ApprovedByHeadOn = leaveDetails.ApprovedByHeadOn;
            model.ApprovedByMD = leaveDetails.ApprovedByMD == null ? false : leaveDetails.ApprovedByMD;
            model.ApprovedByAdminOn = leaveDetails.ApprovedByAdminOn;

            var editLeave = _leaveRepository.UpdateEmployeeLeave(model);
            return editLeave;
        }

        public EmployeeLeave ApproveOrRejectEmployeeLeave(EmployeeLeaveVm employeeLeaveVm)
        {
            EmployeeLeave model = new EmployeeLeave();
            var leaveDetails = _ppsDbContext.EmployeeLeave.Where(m => m.Id == employeeLeaveVm.Id).FirstOrDefault();
            if (leaveDetails != null)
            {

                model.Id = leaveDetails.Id;
                model.EmployeeId = leaveDetails.EmployeeId;
                model.LeaveCategoryId = leaveDetails.LeaveCategoryId;
                model.LeaveDays = leaveDetails.LeaveDays;
                model.UnpaidLeaveDays = leaveDetails.UnpaidLeaveDays;
                model.FromDate = leaveDetails.FromDate;
                model.ToDate = leaveDetails.ToDate;
                model.ReasonOfLeave = leaveDetails.ReasonOfLeave;
                model.DateOfApplication = leaveDetails.DateOfApplication;
                model.Address = leaveDetails.Address;
                model.LeaveYear = leaveDetails.LeaveYear;
                model.MobileNo = leaveDetails.MobileNo;
                model.CreatedBy = leaveDetails.CreatedBy;
                model.CreatedOn = leaveDetails.CreatedOn;
                model.UpdatedBy = leaveDetails.UpdatedBy;
                model.UpdatedOn = leaveDetails.UpdatedOn;
                if (employeeLeaveVm.IsApproved == 1)
                {
                    model.ApprovedByDepartmentHead = employeeLeaveVm.ApprovedByDepartmentHead == true ? true : leaveDetails.ApprovedByDepartmentHead;
                    model.ApprovedByHeadOn = employeeLeaveVm.ApprovedByDepartmentHead == true ? DateTime.Now.Date : leaveDetails.ApprovedByHeadOn;
                    model.ApprovedByMD = employeeLeaveVm.ApprovedByMD == true ? true : leaveDetails.ApprovedByMD;
                    model.ApprovedByAdminOn = employeeLeaveVm.ApprovedByMD == true ? DateTime.Now.Date : leaveDetails.ApprovedByHeadOn;
                    model.IsApproved = (employeeLeaveVm.ApprovedByMD == true) ? employeeLeaveVm.IsApproved : leaveDetails.IsApproved;
                }
                else if (employeeLeaveVm.IsApproved == -1)
                {
                    model.ApprovedByDepartmentHead = employeeLeaveVm.ApprovedByDepartmentHead == false ? false : leaveDetails.ApprovedByDepartmentHead;
                    model.ApprovedByHeadOn = employeeLeaveVm.ApprovedByDepartmentHead == false ? DateTime.Now.Date : leaveDetails.ApprovedByHeadOn;
                    model.ApprovedByMD = leaveDetails.ApprovedByDepartmentHead == true ? employeeLeaveVm.ApprovedByMD : leaveDetails.ApprovedByMD;
                    model.ApprovedByAdminOn = leaveDetails.ApprovedByDepartmentHead == true ? DateTime.Now.Date : leaveDetails.ApprovedByHeadOn;
                    model.IsApproved = (employeeLeaveVm.ApprovedByDepartmentHead == false || employeeLeaveVm.ApprovedByMD == false) ? employeeLeaveVm.IsApproved : leaveDetails.IsApproved;
                }

            }
            var leaveStatus = _leaveRepository.ApproveOrRejectEmployeeLeave(model);
            return leaveStatus;
        }
    }
}