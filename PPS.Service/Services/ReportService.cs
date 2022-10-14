using PPS.API.Shared.ViewModel.Customer;
using PPS.API.Shared.ViewModel.Product;
using PPS.API.Shared.ViewModel.Report;
using PPS.Data.Edmx;
using PPS.Data.Repositories;
using PPS.Data.RepositoryInterfaces;
using PPS.Service.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PPS.Service.Services
{
    public class ReportService : IReportService
    {
        private IReportRepository _reportRepository;
        private readonly PPSDbContext _ppsDbContext;
        public ReportService()
        {
            _reportRepository = new ReportRepository();
            _ppsDbContext = new PPSDbContext();
        }

        public IndividualLedgerModel GetIndividualLedger(int fiscalYear, int companyId, int headId, DateTime startDate, DateTime endDate)
        {
            return _reportRepository.GetIndividualLedger(fiscalYear, companyId, headId, startDate, endDate);
        }

        public JournalModel GetJournal(int fiscalYear, int companyId, DateTime startDate, DateTime endDate)
        {
            return _reportRepository.GetJournal(fiscalYear, companyId, startDate, endDate);
        }

        public LedgerModel GetLedger(int fiscalYear, int companyId, DateTime startDate, DateTime endDate)
        {
            return _reportRepository.GetLedger(fiscalYear, companyId, startDate, endDate);
        }

        public TrailBalanceModel GetTrailBalance(int fiscalYear, int companyId, DateTime startDate, DateTime endDate)
        {
            return _reportRepository.GetTrailBalance(fiscalYear, companyId, startDate, endDate);
        }

        public ProfitAndLossAccountModel GetProfitAndLossAccount(int fiscalYear, int companyId, DateTime startDate, DateTime endDate)
        {
            return _reportRepository.GetProfitAndLossAccount(fiscalYear, companyId, startDate, endDate);
        }

        public BalanceSheetModel GetBalanceSheet(int fiscalYear, int companyId, DateTime startDate, DateTime endDate)
        {
            return _reportRepository.GetBalanceSheet(fiscalYear, companyId, startDate, endDate);
        }

        public CustomerStatementVm GetCustomerStatement(int companyId, int customerId, DateTime startDate, DateTime endDate)
        {
            return _reportRepository.GetCustomerStatement(companyId, customerId, startDate, endDate);
        }

        public VoucherVm GetVoucherDetail(string tranNo)
        {
            return _reportRepository.GetVoucherDetail(tranNo);
        }

        public List<VoucherVm> GetTransactionHistoryByTransactionNo(int companyId, string tranNo)
        {
            return _reportRepository.GetTransactionHistoryByTransactionNo(companyId, tranNo);
        }
        public List<ProductDeliveryListVm> GetProductReportList(DatePickerVm datePickerVm)
        {
            return _reportRepository.GetProductReportList(datePickerVm);
        }
        public List<DealerAuditReportVm> DealerAuditReport(DatePickerVm datePickerVm)
        {
            List<DealerAuditReportVm> dealerAuditReportList = new List<DealerAuditReportVm>();

            var demandOrder = _reportRepository.DealerAuditReport(datePickerVm).GroupBy(m => m.CustomerId).ToList();

            foreach (var item in demandOrder)
            {
                var doDetail = item.FirstOrDefault().DemandOrderDetail.GroupBy(m => m.ProductId).ToList();
                DealerAuditReportVm dealerAuditReportVm = new DealerAuditReportVm
                {
                    Id = item.Key,
                    CustomerCode = item.FirstOrDefault().Customer.CustomerCode,
                    CustomerName=item.FirstOrDefault().Customer.CustomerName,
                    DemandOrder = new List<AuditDemandOrderVm>()
                    {
                       new AuditDemandOrderVm(){
                           GeneralDiscount=item.ToList().Sum(n=>n.RegularDiscountAmount??0),
                           SpecialDiscount=item.ToList().Sum(n=>n.SpecialDiscountAmount??0),
                           ExtraDiscount=item.ToList().Sum(n=>n.ExtraDiscountAmount??0),
                           TotalAmount=item.ToList().Sum(n=>n.TotalAmount),
                           GrandTotal=item.ToList().Sum(n=>n.TotalGrandAmount)
                        }
                    },
                    ProductList = new List<ProductListVm>()

                };
                foreach (var doD in doDetail)
                {
                    ProductListVm productList = new ProductListVm
                    {
                        Id = doD.Key,
                        Name = doD.FirstOrDefault().Product.Name,
                        Code=doD.FirstOrDefault().Product.Code,
                        Quantity = doD.FirstOrDefault().Quantity
                    };
                    dealerAuditReportVm.ProductList.Add(productList);
                }
                dealerAuditReportList.Add(dealerAuditReportVm);
            }
            return dealerAuditReportList;
        }
    }
}