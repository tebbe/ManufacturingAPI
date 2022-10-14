using System;

namespace PPS.API.Shared.ViewModel.Customer
{
    public class CustomerTransactionDetailVm
    {
        public int Id { get; set; }
        public int CustomerTransactionId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int CustomerAccountHeadId { get; set; }
        public int? BookNo { get; set; }
        public int? BookSerialNo { get; set; }
        public double TransactionAmount { get; set; }
        public int? CustomerCode { get; set; }
    }
}