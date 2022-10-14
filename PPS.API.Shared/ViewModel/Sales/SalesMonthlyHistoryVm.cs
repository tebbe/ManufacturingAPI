namespace PPS.API.Shared.ViewModel.Sales
{
    public class SalesMonthlyHistoryVm
    {
        public int Year { get; set; }
        public string Month { get; set; }
        public decimal SalesTarget { get; set; }
        public decimal Achievement { get; set; }
        public decimal Percentage { get; set; }

    }
}