using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.API.Shared.ViewModel.Report
{
    public class BalanceSheetModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }        

        public List<BalanceSheetDetailModel> Detail { get; set; }

        public List<BalanceSheetDetailAssetModel> AssetsDetail { get; set; }
        public BalanceSheetDetailAssetModel TotalAssets { get; set; }
        public List<BalanceSheetDetailLiabilitiesModel> LiabilitiesDetail { get; set; }
        public BalanceSheetDetailLiabilitiesModel TotalLiabilities { get; set; }
        public BalanceSheetDetailOwnersEquityModel OwnersEquityCapital { get; set; }
        public BalanceSheetDetailOwnersEquityModel OwnersEquityDrawing { get; set; }
        public BalanceSheetDetailOwnersEquityModel NetIncome { get; set; }
        public BalanceSheetDetailOwnersEquityModel TotalOwnersEquity { get; set; }
        public BalanceSheetDetailOwnersEquityModel TotalLiabilitiesAndOwnersEquity { get; set; }
    }
    public class BalanceSheetDetailModel
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

    public class BalanceSheetDetailAssetModel : BalanceSheetDetailModel { }
    public class BalanceSheetDetailLiabilitiesModel : BalanceSheetDetailModel { }
    public class BalanceSheetDetailOwnersEquityModel : BalanceSheetDetailModel { }

}