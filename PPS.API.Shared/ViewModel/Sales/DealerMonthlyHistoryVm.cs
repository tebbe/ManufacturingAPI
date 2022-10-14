namespace PPS.API.Shared.ViewModel.Sales
{
    public class DealerMonthlyHistoryVm
    {
        public string DealerName { get; set; }
        public int DearlerCode { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public double Rating { get; set; }
        public double RiskLevel { get; set; }
        public decimal TotalDoAmount { get; set; }
        public decimal TotalPaidAmount { get; set; }
        public decimal TotalDueAmount { get; set; }
    }
}