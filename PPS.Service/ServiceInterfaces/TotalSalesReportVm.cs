using System;

namespace PPS.Service.ServiceInterfaces
{
    public class TotalSalesReportVm
    {
        public int Id { get; set; }
        public DateTime SalesDate { get; set; }
        public double Amount { get; set; }
        public double TotalAmount { get; set; }

    }
}