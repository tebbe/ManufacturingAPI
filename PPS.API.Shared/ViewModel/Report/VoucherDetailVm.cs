using System;

namespace PPS.API.Shared.ViewModel.Report
{
    public class VoucherDetailVm
    {
        public string AccountHeadName { get; set; }
        public double TransactionAmount { get; set; }
        public double DrAmount { get; set; }
        public double CrAmount { get; set; }
        public string Note { get; set; }
    }
}