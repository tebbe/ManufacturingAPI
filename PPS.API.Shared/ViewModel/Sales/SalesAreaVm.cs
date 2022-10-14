using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.API.Shared.ViewModel.Sales
{
    public class SalesAreaVm
    {
        public int Id { get; set; }
        public string SalesAreaName { get; set; }
        public string SalesDivisionName { get; set; }
        public int SalesDivisionId { get; set; }
    }
}