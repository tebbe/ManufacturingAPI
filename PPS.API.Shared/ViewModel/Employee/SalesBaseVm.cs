using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.API.Shared.ViewModel.Employee
{
    public class SalesBaseVm
    {
        public int Id { get; set; }
        public string SalesBaseName { get; set; }
        public int SalesAreaId { get; set; }
        public string SalesAreaName { get; set; }
    }
}