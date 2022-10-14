using PPS.API.Shared.ViewModel.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using PPS.API.Shared.ViewModel.Customer;
using PPS.API.Shared.ViewModel.Product;
using PPS.API.Shared.ViewModel.Sales;

namespace PPS.Service.ServiceInterfaces
{
    public interface IReportService
    {
        LedgerModel GetLedger(int fiscalYear, int companyId, DateTime startDate, DateTime endDate);
        JournalModel GetJournal(int fiscalYear, int companyId, DateTime startDate, DateTime endDate);
        IndividualLedgerModel GetIndividualLedger(int fiscalYear, int companyId, int headId, DateTime startDate, DateTime endDate);
        TrailBalanceModel GetTrailBalance(int fiscalYear, int companyId, DateTime startDate, DateTime endDate);
        ProfitAndLossAccountModel GetProfitAndLossAccount(int fiscalYear, int companyId, DateTime startDate, DateTime endDate);
        BalanceSheetModel GetBalanceSheet(int fiscalYear, int companyId, DateTime startDate, DateTime endDate);
        CustomerStatementVm GetCustomerStatement(int companyId, int customerId, DateTime startDate, DateTime endDate);
        VoucherVm GetVoucherDetail(string tranNo);
        List<VoucherVm> GetTransactionHistoryByTransactionNo(int companyId, string tranNo);
        List<ProductDeliveryListVm> GetProductReportList(DatePickerVm viewModel);
        List<DealerAuditReportVm> DealerAuditReport(DatePickerVm dataVm);
    }
}