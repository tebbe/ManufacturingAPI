using System;
using System.Collections.Generic;

namespace PPS.API.Shared.ViewModel.Customer
{
    public class CustomerStatementVm
    {
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerMobile { get; set; }
        public string CustomerSeCode { get; set; }
        public DateTime StatementStartDate { get; set; }
        public DateTime StatementEndDate { get; set; }
        public double PreviousBalance { get; set; }
        public double AsOfDateBalance { get; set; }
        public IList<CustomerStatementDetailVm> CustomerStatementDetail { get; set; }
    }
}