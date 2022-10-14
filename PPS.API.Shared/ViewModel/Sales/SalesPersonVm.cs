using System.Collections.Generic;

namespace PPS.API.Shared.ViewModel.Sales
{
    public class SalesPersonVm
    {
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Area { get; set; }
        public double CurrentSalesTarget { get; set; }
        public int SalesCode { get; set; }
        public double CurrentCollection { get; set; }

        public List<SalesPersonVm> SalesPersonList { get; set; }
        public List<SalesMonthlyHistoryVm> SalesMonthlyHistoryList { get; set; }
        public List<DealerMonthlyHistoryVm> DealerMonthlyHistoryList { get; set; }
        public double DueBalance { get; set; }
        public double TotalDoAmount { get; set; }
        public double TotalDueCollection { get; set; }
    }
}