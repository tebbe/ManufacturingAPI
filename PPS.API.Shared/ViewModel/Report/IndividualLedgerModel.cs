using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.API.Shared.ViewModel.Report
{
    public class IndividualLedgerModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int AccountHeadId { get; set; }
        public double OpeningBalanceDr { get; set; }
        public double OpeningBalanceCr { get; set; }
        public double CurrTransTotalDr { get; set; }
        public double CurrTransTotalCr { get; set; }
        public double CurrentBalanceDr { get; set; }
        public double CurrentBalanceCr { get; set; }
        public double ClosingBalanceDr { get; set; }
        public double ClosingBalanceCr { get; set; }
        public List<IndividualLedgerDetailModel> Detail { get; set; }
    }

    public class IndividualLedgerDetailModel
    {
        public string TransactionNo { get; set; }
        public string TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
        public double DrAmount { get; set; }
        public double CrAmount { get; set; }
        public string AccountHeadName { get; set; }
        public string Particular { get; set; }
    }
}