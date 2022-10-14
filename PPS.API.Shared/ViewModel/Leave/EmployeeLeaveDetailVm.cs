using PPS.API.Shared.ViewModel.Leave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.API.Shared.ViewModel.Leave
{
    public class EmployeeLeaveDetailsVm
    {
        public int Id { get; set; }
        public string LeaveCategoryName { get; set; }
        public int TotalPaidLeave { get; set; }
        public int CasualLeave { get; set; }
        public int EarnLeave { get; set; }
        public int SickLeave { get; set; }
        public int OtherLeave { get; set; }
        public int RemainEarnLeave { get; set; }
        public int RemainSickLeave { get; set; }
        public int RemainOtherLeave { get; set; }
        public int RemainCasualLeave { get; set; }
        public int? IsApproved { get; set; }
        public int TotalUnpaidLeave { get; set; }

    }
}