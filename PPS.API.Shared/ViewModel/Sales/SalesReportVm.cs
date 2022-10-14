using System;

namespace PPS.API.Shared.ViewModel.Sales
{
    public class SalesReportVm
    {
        public DateTime DODate { get; set; }
        public string CustomerName { get; set; }
        public string SalesOfficer { get; set; }
        public int DONo { get; set; }
        public double DOAmount { get; set; }
        public double DOPaid { get; set; }
        public double DOBalance { get; set; }
        public int CustomerCode { get; set; }
        public string SalesDivisionName { get; set; }
        public string SalesAreaName { get; set; }
    }
}