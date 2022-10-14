using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PPS.API.Shared.ViewModel.Account;

namespace PPS.API.Shared.ViewModel.Report
{
    public class LedgerModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }        

        public List<LedgerDetailModel> Detail { get; set; }
    }
    public class LedgerDetailModel
    {
        public int AccountHeadId { get; set; }
        public string AccountHead { get; set; }
        public DateTime LedgerDate { get; set; }
        public double OpenDrAmount { get; set; }
        public double OpenCrAmount { get; set; }
        public double DrAmount { get; set; }
        public double CrAmount { get; set; }

        public virtual AccountHeadVm AccountHeadVm { get; set; }
    }
}