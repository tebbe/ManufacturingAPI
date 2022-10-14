using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.API.Shared.ViewModel.Leave
{
    public class CompanyLeaveVm
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int LeaveCategoryId { get; set; }
        public int MonthlyLimit { get; set; }
        public int TotalYearlyLeave { get; set; }
        public int Year { get; set; }
    }
}