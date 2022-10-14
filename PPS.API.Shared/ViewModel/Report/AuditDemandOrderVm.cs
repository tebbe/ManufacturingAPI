namespace PPS.API.Shared.ViewModel.Report
{
    public class AuditDemandOrderVm
    {
        public  double  GeneralDiscount { get; set; }
        public double SpecialDiscount { get; set; }
        public double ExtraDiscount { get; set; }
        public double TotalAmount { get; set; }
        public double GrandTotal { get; set; }

    }
}