using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.API.Shared.ViewModel.Process
{
    public class MonthlySalesProcessVm
    {
        public int Id { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ReprocessedBy { get; set; }
        public DateTime? ReprocessedOn { get; set; }
    }
}