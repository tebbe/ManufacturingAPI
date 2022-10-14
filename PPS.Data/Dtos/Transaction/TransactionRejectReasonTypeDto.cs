using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PPS.API.Shared.ViewModel.Account;

namespace PPS.Data.Dtos.Transaction
{
    public class TransactionRejectReasonTypeDto
    {
        public int Id { get; set; }
        public string ReasonText { get; set; }

        public TransactionRejectReasonTypeVm ToTransactionRejectReasonTypeVm()
        {
            var vm = new TransactionRejectReasonTypeVm
            {
                Id = this.Id,
                ReasonText = this.ReasonText
            };

            return vm;
        }
    }
}