using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.API.Shared.RequestVm
{
    public class RequestVm
    {
        public int FiscalYear { get; set; }
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public DateTime DatedOn { get; set; }
    }
}