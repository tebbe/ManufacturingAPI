using PPS.API.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.API.Shared.ViewModel.Filter
{
    public class FilterVm
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public SortDirectionEnum SortDirection { get; set; }
        public string SortColumn { get; set; }
        public string FilterColumn { get; set; }
        public string FilterCriteria { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? CustomerId { get; set; }
    }
}