using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.API.Shared.ViewModel.Account
{
    public class AcceptRejectTransactionVm
    {
        public string TransactionNo { get; set; }
        public int TransactionTypeId { get; set; }
        //public int ActionTypeId { get; set; }
        public int RejectReasonTypeId { get; set; }
        public int UserId { get; set; }
        public DateTime DatedOn { get; set; }
    }
}