using System;
using PPS.API.Shared.ViewModel.Sales;
using System.Collections.Generic;
using System.Threading.Tasks;
using PPS.API.Shared.RequestVm;
using PPS.API.Shared.ViewModel.Employee;
using PPS.API.Shared.ViewModel.User;
using PPS.Data.Edmx;
using PPS.API.Shared.ViewModel.Document;
using PPS.API.Shared.ViewModel.Filter;

namespace PPS.Service.ServiceInterfaces
{
    public interface ISalesInterface
    {
        List<SaleTypeModel> GetSaleType();
        List<DemandOrderTypeModel> GetDemandOrderType();
        List<DiscountTypeModel> GetDiscountType();
        List<ProductModel> GetProductList();
        DemandOrderModel SaveDemandOrder(DemandOrderModel demandOrderModel);
        DemandOrderModel UpdateDemandOrder(DemandOrderModel demandOrderModel);
        IList<DemandOrderVm> GetDemandOrderList(int userId, int paymentStatus);
        IList<DemandOrderVm> GetDemandOrderListForFiltering(int id, int paymentStatus, FilterVm filterVm);
        Task<bool> SubmitDO(int doId, int userId);
        DemandOrderVm GetDemandOrderById(int userId, int doId);
        IList<PostOfficeModel> GetPostOfficeList();
        IList<AreaModel> GetAreaList();
        Task<bool> VerifyDO(int doId, int userId);
        Task<bool> DeliveryConfirmedDO(int doId, int userId);
        Task<bool> ApproveDO(int doId, int userId);
        Task<bool> SaveDemandOrderTransaction(int userId, DemandOrderTransactionVm doTransactionVm);
        IList<InvoiceVm> GetInvoiceList(int userId);
        List<DemandOrderVm> GetDemandOrderIdListForInvoice(int userId);
        DemandOrderVm GetDemandOrderByIdFromInvoice(int userId, int doId, int invoiceId);
        InvoiceVm SaveInvoice(InvoiceVm invoiceVm);
        InvoiceVm UpdateInvoice(InvoiceVm invoiceVm);
        InvoiceVm GetInvoiceById(int userId, int invoiceId);
        Task<bool> ApproveInvoice(InvoiceRequestVm invoiceRequestVm);
        Task<bool> DeliveryInvoice(int invoiceId, int userId);
        IList<CustomerTransactionHistoryVm> GetCustomerTransactionHistoryByCustomerId(int userId, int doId);
        SalesPersonVm GetSalesPersonHistoryByEmployeeId(int userId, int employeeId, int year);
        IList<DemandOrderVm> GetDemandOrderEarlyPaymentList(int userId);
        IList<DemandOrderVm> GetDemandOrderEarlyPaymentPendingList(int userId);
        IList<DemandOrderVm> GetDemandOrderEarlyPaymentPaidList(int userId);
        Task<bool> PayDOEarlyPaymentDiscountToCustomer(int doId, int userId);
        IList<DemandOrderVm> GetDemandOrderEarlyPaymentPendingTransactionList(int userId);
        IList<DemandOrderVm> GetDemandOrderEarlyPaymentApprovedTransactionList(int userId);
        Task<bool> VerifyDOEarlyPaymentDiscountToCustomer(DemandOrderEarlyPaymentRequestVm demandOrderEarlyPaymentRequestVm);
        IList<CompanySalesTargetVm> GetCompanySalesTargetList(int userId);
        CompanySalesTargetVm SaveCompanySalesTarget(CompanySalesTargetVm companySalesTarget);
        CompanySalesTargetVm UpdateCompanySalesTarget(CompanySalesTargetVm companySalesTarget);
        Task<bool> ApproveCompanySalesTarget(int companySalesTargetId, int userId);
        CompanySalesTargetVm GetCompanySalesTargetById(int userId, int companySalesTargetId);
        List<SalesTeamTargetVm> GetSalesTeamTargetList(int userId, int year, int month);
        List<SalesTeamTargetVm> SaveSalesTeamTarget(List<SalesTeamTargetVm> salesTeamTargetVmList);
        SalesTeamTargetVm UpdateSalesTeamTarget(SalesTeamTargetVm salesTeamTargetVm);
        CompanySalesTargetVm GetSalesTeamTargetById(int userId, int salesTeamTargetId);
        List<SalesDivisionVm> GetSalesDivisionList(int userId);
        List<SalesAreaVm> GetSalesAreaList(int userId);
        List<SalesReportVm> GetSalesReportList(DateTime startDate, DateTime endDate, int salesDivisionId,
            int salesAreaId, int employeeId, int customerId);
        List<ProductionForecastVm> GetProductionForecastList(int year, int month);
        List<ProductionForecastVm> SaveProductionForecast(List<ProductionForecastVm> productionForecastList);
        Task<List<SalesAreaVm>> GetSalesAreaBySalesDivisionId(int salesDivisionId);
        Task<List<EmployeeVm>> GetSalesOfficerBySalesDivisionId(int salesDivisionId, int employeeId);
        Task<List<CustomerModel>> GetCustomerBySalesDivisionId(int salesDivisionId);
        Task<bool> SaveInvoiceTransaction(int userId, InvoiceTransactionVm invoiceTransactionVm);
        List<InvoiceReturnVm> InvoiceReturnList();
        List<InvoiceVm> GetAllInvoiceList();
        InvoiceReturn SaveReturnInvoice(InvoiceReturnVm invoiceVm);
        InvoiceReturnVm GetInvoiceReturnById(int id);
        InvoiceReturn UpdateReturnInvoice(InvoiceReturnVm invoiceReturnEditVm);
        InvoiceReturn ApproveReturnInvoice(InvoiceReturnVm invoiceReturnVm);
        List<DeliveryQuantityVm> InvoiceDeliveryChallanList();
        DeliveryQuantity DeliveryQuantitySave(DeliveryQuantityVm deliveryQuantityVm);
        DeliveryQuantityVm GetDeliveryQuantityById(int id);
        DeliveryQuantity DeliveryQuantityUpdate(DeliveryQuantityVm deliveryQuantityVm);
        DeliveryQuantity ApproveDeliveryQuantityById(DeliveryQuantityVm dataVm);
        UndeliveryQuantityVm GetUndeliveryQuantityById(int id);
        DeliveryChallanInvoiceVm InvoiceDetailsByIdForDeliveryChallan(int id);
        List<TotalSalesReportVm> GetTotalSalesReportList(DateTime startDate, DateTime endDate);
    }
}