using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.API.Shared.ViewModel.Report
{
    public class JournalModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double DebitTotal { get; set; }
        public double CreditTotal { get; set; }
        public List<JournalEntryModel> JournalEntry { get; set; }
    }
    public class JournalEntryModel
    {
        public string TransactionNo { get; set; }
        public string TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
        public string AccountHead { get; set; }
        public double Debit { get; set; }
        public double Credit { get; set; }
    }
}