using System;

namespace PPS.API.Shared.ViewModel.Customer
{
    public class CustomerStatementDetailVm
    {
        public string AccountHead { get; set; }
        public string TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
        public double TransactionAmount { get; set; }
        public double TransactionBalance { get; set; }

    }
}