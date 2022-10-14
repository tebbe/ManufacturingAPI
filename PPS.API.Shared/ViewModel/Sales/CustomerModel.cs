using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PPS.API.Shared.ViewModel.Customer;

namespace PPS.API.Shared.ViewModel.Sales
{
    public class CustomerModel
    {
        public int Id { get; set; }
        public int? AccountHeadId { get; set; }
        public string CustomerName { get; set; }
        public int CustomerCode { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerMobile { get; set; }
        public string CustomerPhone { get; set; }
        public string OwnerName { get; set; }
        public string OwnerMobile { get; set; }
        public string OwnerPhone { get; set; }
        public DateTime? OwnerBirthDate { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPersonMobile { get; set; }
        public string PrimaryContactNo { get; set; }
        public string Village { get; set; }
        public int? PostOfficeId { get; set; }
        public string PostOffice { get; set; }
        public string Email { get; set; }
        public int? AreaId { get; set; }
        public string Area { get; set; }
        public double TotalPayment { get; set; }
        public double TotalBalance { get; set; }
        public string Status { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CompanyId { get; set; }
        public int FiscalYear { get; set; }
        public decimal MonthlyCredit { get; set; }
        public decimal YearlyCredit { get; set; }
        public decimal SalesCapacityYearly { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public List<CustomerTransactionVm> CustomerTransaction { get; set; }
        public int? EmployeeId { get; set; }
        public int? EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string CustomerTypeName { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}