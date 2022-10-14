using PPS.API.Shared.ViewModel;
using PPS.Data.Edmx;
using PPS.Data.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PPS.Data.Repositories
{
    public class LeaveRepository:ILeaveRepository
    {
        private readonly PPSDbContext _ppsDbContext;
        public LeaveRepository()
        {
            _ppsDbContext = new PPSDbContext();
        }

        public IQueryable<EmployeeLeave> GetEmployeeLeaveList()
        {
            return _ppsDbContext.EmployeeLeave;
        }
        public IQueryable<LeaveCategory> GetLeaveCategory()
        {
            return _ppsDbContext.LeaveCategory;
        }
        public IQueryable<Employee> GetEmployee()
        {
            return _ppsDbContext.Employee;
        }
        public EmployeeLeave SaveEmployeeLeave(EmployeeLeave employeeLeave)
        {
            using(var db=new PPSDbContext())
            {
                db.EmployeeLeave.Add(employeeLeave);
                db.SaveChanges();
                return employeeLeave;
            }
        }
        public EmployeeLeave GetEmployeeLeaveById(int id)
        {
            return _ppsDbContext.EmployeeLeave.Where(m => m.Id == id).FirstOrDefault();
        }
        public IQueryable<EmployeeLeave> GetLeaveListByEmployeeId(int employeeId)
        {
            return _ppsDbContext.EmployeeLeave.Where(m => m.EmployeeId == employeeId && m.LeaveYear==DateTime.Now.Year);
        }
        public IQueryable<EmployeeLeave> GetLeaveListByEmployeeIdAndStatus(int employeeId,int? status)
        {

            return _ppsDbContext.EmployeeLeave.Where(m => m.EmployeeId == employeeId && m.LeaveYear == DateTime.Now.Year && m.IsApproved==status);
        }

        public EmployeeLeave UpdateEmployeeLeave(EmployeeLeave employeeLeave)
        {
            using (var db = new PPSDbContext())
            {
                db.EmployeeLeave.Attach(employeeLeave);
                db.Entry(employeeLeave).State = EntityState.Modified;
                db.SaveChanges();
                return employeeLeave;
            }
        }

        public EmployeeLeave ApproveOrRejectEmployeeLeave(EmployeeLeave employeeLeave)
        {
            using (var db = new PPSDbContext())
            {
                db.EmployeeLeave.Attach(employeeLeave);
                db.Entry(employeeLeave).State = EntityState.Modified;
                db.SaveChanges();
                return employeeLeave;
            }
        }
    }
}