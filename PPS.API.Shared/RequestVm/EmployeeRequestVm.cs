using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;

namespace PPS.API.Shared.RequestVm
{
    public class EmployeeRequestVm : RequestVm
    {
        public int EmployeeId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
    }
}