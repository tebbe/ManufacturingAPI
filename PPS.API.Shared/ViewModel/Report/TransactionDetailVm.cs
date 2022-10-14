using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PPS.API.Shared.ViewModel.Account;

namespace PPS.API.Shared.ViewModel.Report
{
    public class TransactionDetailVm
    {
        public int Id { get; set; }
        public int AccountHeadId { get; set; }
        public string AccountHeadName { get; set; }
        public double DrAmount { get; set; }
        public double CrAmount { get; set; }
        public int TransactionEntryId { get; set; }

        public virtual AccountHeadVm AccountHead { get; set; }
        public virtual TransactionEntryVm TransactionEntry { get; set; }
    }
}