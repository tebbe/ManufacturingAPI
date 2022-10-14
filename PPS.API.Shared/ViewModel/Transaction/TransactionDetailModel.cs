using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.API.Shared.ViewModel.Transaction
{
    public class TransactionDetailModel
    {
        public int TranId { get; set; }
        public int TranHeadId { get; set; }
        public string TranHead { get; set; }
        public double DrAmount { get; set; }
        public double CrAmount { get; set; }
        public string Note { get; set; }
    }
}