using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.API.Shared.ViewModel.Report
{
    public class TrailBalanceModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }        

        public List<TrailBalanceDetailModel> Detail { get; set; }
    }
    public class TrailBalanceDetailModel
    {
        public int AccountHeadId { get; set; }
        public string AccountHead { get; set; }
        public double OpenDrAmount { get; set; }
        public double OpenCrAmount { get; set; }
        public double CurrDrAmount { get; set; }
        public double CurrCrAmount { get; set; }
        public double CloseDrAmount { get; set; }
        public double CloseCrAmount { get; set; }
    }
}