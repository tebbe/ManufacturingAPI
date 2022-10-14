using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.API.Shared.ViewModel.Report
{
    public class ProfitAndLossAccountModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<ProfitAndLossAccountDetailModel> Detail { get; set; }
        public List<ProfitAndLossAccountRevenueDetailModel> RevenueDetail { get; set; }
        public List<ProfitAndLossAccountExpenseDetailModel> ExpenseDetail { get; set; }
        public TotalRevenue TotalRevenue { get; set; }
        public TotalExpense TotalExpense { get; set; }
        public ClosingRevenue ClosingRevenue { get; set; }
        public ClosingExpense ClosingExpense { get; set; }
        public NetIncome NetIncome { get; set; }
        public List<ProfitAndLossAccountExpenseDetailModel> DirectExpenseDetail { get; set; }
        public List<ProfitAndLossAccountExpenseDetailModel> IndirectExpenseDetail { get; set; }
    }
    public class ProfitAndLossAccountDetailModel
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

    public class ProfitAndLossAccountRevenueDetailModel : ProfitAndLossAccountDetailModel { }

    public class ProfitAndLossAccountExpenseDetailModel : ProfitAndLossAccountDetailModel { }
    public class TotalRevenue : ProfitAndLossAccountDetailModel { }
    public class TotalExpense : ProfitAndLossAccountDetailModel { }
    public class ClosingRevenue : ProfitAndLossAccountDetailModel { }
    public class ClosingExpense : ProfitAndLossAccountDetailModel { }
    public class NetIncome : ProfitAndLossAccountDetailModel { }

}