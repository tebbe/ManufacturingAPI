using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.Data.Dtos.Transaction
{
    public class TransactionDetailDto
    {
        public int TranHeadId { get; set; }
        public string TranHead { get; set; }
        public double DrAmount { get; set; }
        public double CrAmount { get; set; }
        public string Note { get; set; }
    }
}