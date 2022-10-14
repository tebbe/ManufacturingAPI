using PPS.API.Shared.Enums;
using PPS.API.Shared.Extensions;
using PPS.API.Shared.RequestVm;
using PPS.API.Shared.ViewModel.Employee;
using PPS.API.Shared.ViewModel.Filter;
using PPS.API.Shared.ViewModel.Sales;
using PPS.Data.Edmx;
using PPS.Data.Repositories;
using PPS.Data.RepositoryInterfaces;
using PPS.Service.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace PPS.Service.Services
{
    public class SalesService : ISalesInterface
    {
        static readonly object invLock = new object();
        private ISalesRepository _salesRepository;
        private ICustomerRepository _customerRepository;
        private IEmployeeRepository _employeeRepository;
        private readonly PPSDbContext _PPSDbContext;

        public string UpdateByName { get; private set; }
        public string ApprovedByName { get; private set; }

        public SalesService()
        {
            _salesRepository = new SalesRepository();
            _customerRepository = new CustomerRepository();
            _employeeRepository = new EmployeeRepository();
            _PPSDbContext = new PPSDbContext();
        }
        private Tuple<string, string> GetUserNameById(int userId)
        {
            var userName = "";
            var designation = "";
            var user = _PPSDbContext.User.FirstOrDefault(x => x.Id == userId);
            if (user != null)
            {
                userName = StringExtension.ToFullName(user.FirstName, user.LastName);
                designation = _PPSDbContext.Employee.FirstOrDefault(x => x.Id == user.EmployeeId)?.Designation?
                    .DesignationName;
            }
            var tuple = Tuple.Create(userName, designation);

            return tuple;
        }
        public List<SaleTypeModel> GetSaleType()
        {
            var saleType = _salesRepository.GetSaleType();
            if (saleType == null)
                return null;
            var saleTypeList = new List<SaleTypeModel>();
            foreach (var sType in saleType)
            {
                saleTypeList.Add(sType);
            }
            return saleTypeList;
        }
        public List<DemandOrderTypeModel> GetDemandOrderType()
        {
            var demandOrderType = _salesRepository.GetDemandOrderType();
            if (demandOrderType == null)
                return null;
            var demandOrderTypeList = new List<DemandOrderTypeModel>();
            foreach (var doType in demandOrderType)
            {
                demandOrderTypeList.Add(doType);
            }
            return demandOrderTypeList;
        }
        public List<DiscountTypeModel> GetDiscountType()
        {
            var discountType = _salesRepository.GetDiscountType();
            if (discountType == null)
                return null;
            var discountTypeList = new List<DiscountTypeModel>();
            foreach (var discType in discountType)
            {
                discountTypeList.Add(discType);
            }
            return discountTypeList;
        }
        public List<ProductModel> GetProductList()
        {
            var product = _salesRepository.GetProductList();
            if (product == null)
                return null;
            var productList = new List<ProductModel>();
            foreach (var prd in product)
            {
                productList.Add(prd);
            }
            return productList;
        }

        public DemandOrderModel SaveDemandOrder(DemandOrderModel demandOrderModel)
        {
            var demandOrderDto = _salesRepository.SaveDemandOrder(demandOrderModel);
            return demandOrderDto;
        }

        public DemandOrderModel UpdateDemandOrder(DemandOrderModel demandOrderModel)
        {
            var demandOrderDto = _salesRepository.UpdateDemandOrder(demandOrderModel);
            return demandOrderDto;
        }

        public IList<DemandOrderVm> GetDemandOrderList(int userId, int paymentStatus)
        {
            return _salesRepository.GetDemandOrderList(userId, paymentStatus);
            //if (doDtoList == null)
            //    return null;
            //var doVmList = new List<DemandOrderVm>();
            //foreach (var d in doDtoList)
            //{
            //    doVmList.Add(d.ToDemandOrderVm());
            //}
            //return doVmList;
        }

        public IList<DemandOrderVm> GetDemandOrderListForFiltering(int userId, int paymentStatus, FilterVm filterVm)
        {
            return _salesRepository.GetDemandOrderListForFiltering(userId, paymentStatus, filterVm);
            //if (doDtoList == null)
            //    return null;
            //var doVmList = new List<DemandOrderVm>();
            //foreach (var d in doDtoList)
            //{
            //    doVmList.Add(d.ToDemandOrderVm());
            //}
            //return doVmList;
        }
        //public List<DemandOrderVm> GetLastNDemandOrder(int n)
        //{
        //    return _salesRepository.GetLastNDemandOrder(n);
        //}
        public DemandOrderVm GetDemandOrderById(int userId, int doId)
        {
            var vm = _salesRepository.GetDemandOrderById(userId, doId);
            var totalBalance = _customerRepository.GetAvailableAmountByCustomer(vm.CustomerId).Result;
            vm.TotalBalanceAmount = totalBalance;
            return vm;
        }

        public IList<PostOfficeModel> GetPostOfficeList()
        {
            return _salesRepository.GetPostOfficeList();
        }

        public IList<AreaModel> GetAreaList()
        {
            return _salesRepository.GetAreaList();
        }

        public async Task<bool> SubmitDO(int doId, int userId)
        {
            return await _salesRepository.SubmitDO(doId, userId);
        }

        public async Task<bool> VerifyDO(int doId, int userId)
        {
            return await _salesRepository.VerifyDO(doId, userId);
        }

        public async Task<bool> DeliveryConfirmedDO(int doId, int userId)
        {
            return await _salesRepository.DeliveryConfirmedDO(doId, userId);
        }

        public async Task<bool> ApproveDO(int doId, int userId)
        {
            return await _salesRepository.ApproveDO(doId, userId);
        }

        public async Task<bool> SaveDemandOrderTransaction(int userId, DemandOrderTransactionVm doTransactionVm)
        {
            var demandOrder = _salesRepository.GetDemandOrderById(userId, doTransactionVm.DemandOrderId);
            var availableAmount = _customerRepository.GetAvailableAmountByCustomer(demandOrder.CustomerId).Result;
            if (doTransactionVm.TransactionAmount > availableAmount)
            {
                throw new Exception("Transaction amount is greater than available balance.");
            }
            return await _salesRepository.SaveDemandOrderTransactionAsync(doTransactionVm);
        }
        public async Task<bool> SaveInvoiceTransaction(int userId, InvoiceTransactionVm invoiceTransactionVm)
        {
            var invoice = _salesRepository.GetInvoiceById(userId, invoiceTransactionVm.InvoiceId);
            var availableAmount = _customerRepository.GetAvailableAmountByCustomer(invoice.CustomerId).Result;
            if (invoiceTransactionVm.TransactionAmount > invoice.CustomerRemainingBalance)
            {
                throw new Exception("Transaction amount is greater than available balance.");
            }
            return await _salesRepository.SaveInvoiceTransaction(invoiceTransactionVm);
        }

        public IList<InvoiceVm> GetInvoiceList(int userId)
        {
            return _salesRepository.GetInvoiceList(userId);
        }

        public List<DemandOrderVm> GetDemandOrderIdListForInvoice(int userId)
        {
            return _salesRepository.GetDemandOrderIdListForInvoice(userId);
        }

        public DemandOrderVm GetDemandOrderByIdFromInvoice(int userId, int doId, int invoiceId)
        {
            var vm = _salesRepository.GetDemandOrderByIdFromInvoice(userId, doId, invoiceId);
            //var totalBalance = _customerRepository.GetAvailableAmountByCustomerId(vm.CustomerId);
            //vm.TotalBalanceAmount = totalBalance;
            return vm;
        }

        public InvoiceVm SaveInvoice(InvoiceVm invoiceVm)
        {
            var invoice = _salesRepository.SaveInvoice(invoiceVm);
            return invoice;
        }

        public InvoiceVm UpdateInvoice(InvoiceVm invoiceVm)
        {
            var invoice = _salesRepository.UpdateInvoice(invoiceVm);
            return invoice;
        }

        public InvoiceVm GetInvoiceById(int userId, int invoiceId)
        {
            var invoice = _salesRepository.GetInvoiceById(userId, invoiceId);
            var totalCustomerBalance = _customerRepository.GetAvailableAmountByCustomer(invoice.CustomerId).Result;
            invoice.TotalCustomerBalance = totalCustomerBalance;
            return invoice;
        }

        public async Task<bool> ApproveInvoice(InvoiceRequestVm invoiceRequestVm)
        {
            return await _salesRepository.ApproveInvoice(invoiceRequestVm);
        }

        public async Task<bool> DeliveryInvoice(int invoiceId, int userId)
        {
            return await _salesRepository.DeliveryInvoice(invoiceId, userId);
        }
        public IList<CustomerTransactionHistoryVm> GetCustomerTransactionHistoryByCustomerId(int userId, int doId)
        {
            var customerTransactionHistory = _salesRepository.GetCustomerTransactionHistoryByCustomerId(userId, doId);
            return customerTransactionHistory;
        }

        public SalesPersonVm GetSalesPersonHistoryByEmployeeId(int userId, int employeeId, int year)
        {
            return _salesRepository.GetSalesPersonHistoryByEmployeeId(userId, employeeId, year);
        }

        public IList<DemandOrderVm> GetDemandOrderEarlyPaymentList(int userId)
        {
            return _salesRepository.GetDemandOrderEarlyPaymentList(userId);
        }

        public IList<DemandOrderVm> GetDemandOrderEarlyPaymentPendingList(int userId)
        {
            return _salesRepository.GetDemandOrderEarlyPaymentPendingList(userId);
        }

        public IList<DemandOrderVm> GetDemandOrderEarlyPaymentPaidList(int userId)
        {
            return _salesRepository.GetDemandOrderEarlyPaymentPaidList(userId);
        }

        public async Task<bool> PayDOEarlyPaymentDiscountToCustomer(int doId, int userId)
        {
            return await _salesRepository.PayDOEarlyPaymentDiscountToCustomer(doId, userId);
        }

        public IList<DemandOrderVm> GetDemandOrderEarlyPaymentPendingTransactionList(int userId)
        {
            return _salesRepository.GetDemandOrderEarlyPaymentPendingTransactionList(userId);
        }

        public IList<DemandOrderVm> GetDemandOrderEarlyPaymentApprovedTransactionList(int userId)
        {
            return _salesRepository.GetDemandOrderEarlyPaymentApprovedTransactionList(userId);
        }

        public async Task<bool> VerifyDOEarlyPaymentDiscountToCustomer(DemandOrderEarlyPaymentRequestVm demandOrderEarlyPaymentRequestVm)
        {
            return await _salesRepository.VerifyDOEarlyPaymentDiscountToCustomer(demandOrderEarlyPaymentRequestVm);
        }

        public IList<CompanySalesTargetVm> GetCompanySalesTargetList(int userId)
        {
            return _salesRepository.GetCompanySalesTargetList(userId);
        }

        public CompanySalesTargetVm SaveCompanySalesTarget(CompanySalesTargetVm companySalesTarget)
        {
            return _salesRepository.SaveCompanySalesTarget(companySalesTarget);
        }
        public CompanySalesTargetVm UpdateCompanySalesTarget(CompanySalesTargetVm companySalesTarget)
        {
            return _salesRepository.UpdateCompanySalesTarget(companySalesTarget);
        }
        public async Task<bool> ApproveCompanySalesTarget(int companySalesTargetId, int userId)
        {
            return await _salesRepository.ApproveCompanySalesTarget(companySalesTargetId, userId);
        }

        public CompanySalesTargetVm GetCompanySalesTargetById(int userId, int companySalesTargetId)
        {
            return _salesRepository.GetCompanySalesTargetById(userId, companySalesTargetId);
        }

        public List<SalesTeamTargetVm> GetSalesTeamTargetList(int userId, int year, int month)
        {
            return _salesRepository.GetSalesTeamTargetList(userId, year, month);
        }

        public List<SalesTeamTargetVm> SaveSalesTeamTarget(List<SalesTeamTargetVm> salesTeamTargetVmList)
        {
            return _salesRepository.SaveSalesTeamTarget(salesTeamTargetVmList);
        }

        public SalesTeamTargetVm UpdateSalesTeamTarget(SalesTeamTargetVm salesTeamTargetVm)
        {
            return _salesRepository.UpdateSalesTeamTarget(salesTeamTargetVm);
        }

        public CompanySalesTargetVm GetSalesTeamTargetById(int userId, int salesTeamTargetId)
        {
            return _salesRepository.GetSalesTeamTargetById(userId, salesTeamTargetId);
        }

        public List<SalesDivisionVm> GetSalesDivisionList(int userId)
        {
            return _salesRepository.GetSalesDivisionList(userId);
        }

        public List<SalesAreaVm> GetSalesAreaList(int userId)
        {
            return _salesRepository.GetSalesAreaList(userId);
        }
        public List<SalesReportVm> GetSalesReportList(DateTime startDate, DateTime endDate,
            int salesDivisionId, int salesAreaId, int employeeId, int customerId)
        {
            return _salesRepository.GetSalesReportList(startDate, endDate, salesDivisionId, salesAreaId, employeeId, customerId);
        }

        public List<ProductionForecastVm> GetProductionForecastList(int year, int month)
        {
            return _salesRepository.GetProductionForecastList(year, month);
        }

        public List<ProductionForecastVm> SaveProductionForecast(List<ProductionForecastVm> productionForecastList)
        {
            return _salesRepository.SaveProductionForecast(productionForecastList);
        }

        public async Task<List<SalesAreaVm>> GetSalesAreaBySalesDivisionId(int salesDivisionId)
        {
            if (salesDivisionId != -1)
            {
                var salesArea = _salesRepository.GetSalesAreaList().Where(x => x.SalesDivisionId == salesDivisionId)
                    .Select(x => new SalesAreaVm
                    {
                        Id = x.Id,
                        SalesAreaName = x.SalesAreaName,
                        SalesDivisionId = x.SalesDivisionId,
                        SalesDivisionName = x.SalesDivision.SalesDivisionName
                    }
                    ).ToList();
                return salesArea;
            }
            else
            {
                var salesArea = _salesRepository.GetSalesAreaList()
                .Select(x => new SalesAreaVm
                {
                    Id = x.Id,
                    SalesAreaName = x.SalesAreaName,
                    SalesDivisionId = x.SalesDivisionId,
                    SalesDivisionName = x.SalesDivision.SalesDivisionName
                }
                ).ToList();
                return salesArea;
            }


        }

        public async Task<List<EmployeeVm>> GetSalesOfficerBySalesDivisionId(int salesDivisionId, int employeeId)
        {
            List<EmployeeVm> employee;
            var departmentId = _salesRepository.GetEmployees().FirstOrDefault(x => x.Id == employeeId)?.DepartmentId;

            if (departmentId == (int)DepartmentEnum.SalesAndMarketing)
            {
                var managedEmployeeIdList = _employeeRepository.GetManagedEmployee(employeeId).Select(x => x.Item1);
                employee = _salesRepository.GetEmployees()
                    .Where(x => managedEmployeeIdList.Contains(x.Id))
                    .Select(x =>
                        new EmployeeVm
                        {
                            Id = x.Id,
                            FirstName = x.FirstName,
                            LastName = x.LastName,
                            FullName = x.FirstName + " " + x.LastName,
                            Designation = x.Designation.DesignationName,
                            EmployeeCode = x.EmployeeCode,
                            ManagerId = x.ManagerId,
                            SalesDivisionId = x.SalesDivisionId ?? 0,
                            SalesAreaId = x.SalesAreaId ?? 0,
                            SalesBaseId = x.SalesBaseId ?? 0

                        }).ToList();
            }
            else
            {
                employee = _salesRepository.GetEmployees().Where(x => x.DepartmentId == (int)DepartmentEnum.SalesAndMarketing).Select(x =>
                      new EmployeeVm
                      {
                          Id = x.Id,
                          FirstName = x.FirstName,
                          LastName = x.LastName,
                          FullName = x.FirstName + " " + x.LastName,
                          Designation = x.Designation.DesignationName,
                          EmployeeCode = x.EmployeeCode,
                          ManagerId = x.ManagerId,
                          SalesDivisionId = x.SalesDivisionId ?? 0,
                          SalesAreaId = x.SalesAreaId ?? 0,
                          SalesBaseId = x.SalesBaseId ?? 0
                      }).ToList();
            }

            if (salesDivisionId != -1)
            {
                var salesOfficer = employee.Where(x => x.SalesDivisionId == salesDivisionId).ToList();
                return salesOfficer;
            }
            return employee;
        }

        public async Task<List<CustomerModel>> GetCustomerBySalesDivisionId(int salesDivisionId)
        {
            if (salesDivisionId != -1)
            {
                var customers = _salesRepository.GetCustomers().Where(x => x.Employee.SalesDivisionId == salesDivisionId)
                    .Select(x => new CustomerModel
                    {
                        Id = x.Id,
                        CustomerName = x.CustomerName,
                        CustomerCode = x.CustomerCode,
                        EmployeeId = x.EmployeeId
                    }).ToList();
                return customers;
            }
            else
            {
                var customers = _salesRepository.GetCustomers()
                .Select(x => new CustomerModel
                {
                    Id = x.Id,
                    CustomerName = x.CustomerName,
                    CustomerCode = x.CustomerCode,
                    EmployeeId = x.EmployeeId
                }).ToList();
                return customers;
            }
        }
        //invoice return process start

        public List<InvoiceReturnVm> InvoiceReturnList()
        {
            return _salesRepository.InvoiceReturnList().Select(d => new InvoiceReturnVm
            {

                Id = d.Id,
                InvoiceId = d.InvoiceId,
                ReturnDate = d.ReturnDate,
                CreatedBy = d.CreatedBy,
                CreatedOn = d.CreatedOn,
                TotalAmount = (double)d.TotalAmount,
                TotalGrandAmount = (double)d.TotalGrandAmount,
                Note = d.Note,
                ApprovedBy = d.ApprovedBy > 0 ? d.ApprovedBy : 0,
                ApprovedOn = d.ApprovedBy > 0 ? d.ApprovedOn : null,
                TransactionEntryId = d.TransactionEntryId > 0 ? d.TransactionEntryId : 0,
            }).ToList();
        }

        public List<InvoiceVm> GetAllInvoiceList()
        {
            return _salesRepository.GetAllInvoiceList().Select(d => new InvoiceVm
            {
                Id = d.Id,
                InvoiceNo = d.InvoiceNo,

            }).ToList();
        }
        public InvoiceReturn SaveReturnInvoice(InvoiceReturnVm invoiceReturnVm)
        {
            lock (invLock)
            {

                InvoiceReturn returnInvoiceEntry = new InvoiceReturn
                {
                    InvoiceId = invoiceReturnVm.InvoiceId,
                    ReturnDate = invoiceReturnVm.ReturnDate,
                    Note = invoiceReturnVm.Note,
                    TotalAmount = invoiceReturnVm.TotalAmount,
                    TotalGrandAmount = invoiceReturnVm.TotalGrandAmount,
                    CreatedBy = invoiceReturnVm.CreatedBy,
                    CreatedOn = invoiceReturnVm.CreatedOn
                };

                foreach (var item in invoiceReturnVm.InvoiceReturnDetail)
                {
                    var productDetails = _PPSDbContext.Product.FirstOrDefault(m => m.Id == item.ProductId);
                    if (productDetails != null)
                    {
                        returnInvoiceEntry.InvoiceReturnDetail.Add(
                         new InvoiceReturnDetail
                         {
                             InvoiceReturnId = returnInvoiceEntry.Id,
                             ProductId = item.ProductId,
                             Quantity = item.ReturnQuantity,
                             TotalAmount = (decimal)((item.ReturnQuantity) * productDetails.UnitPrice)
                         });
                    }

                }
                var data = _salesRepository.SaveReturnInvoice(returnInvoiceEntry);
                return data;
            };
        }

        public InvoiceReturnVm GetInvoiceReturnById(int id)
        {
            var returnInvoice = _salesRepository.GetInvoiceReturnById(id);
            var user1 = _PPSDbContext.User.Where(m => m.Id == returnInvoice.CreatedBy).FirstOrDefault();
            var user2 = _PPSDbContext.User.Where(m => m.Id == returnInvoice.ApprovedBy).FirstOrDefault();
            InvoiceReturnVm returnVm = new InvoiceReturnVm
            {
                Id = returnInvoice.Id,
                InvoiceId = returnInvoice.InvoiceId,
                ReturnDate = returnInvoice.ReturnDate,
                Note = returnInvoice.Note,
                RegularDiscountInPercentage = returnInvoice.Invoice.RegularDiscountInPercentage,
                TotalAmount = (double)returnInvoice.TotalAmount,
                TotalDiscountAmount = CalculateValue((double)returnInvoice.TotalAmount, (double)returnInvoice.Invoice.RegularDiscountInPercentage),
                TotalGrandAmount = (double)returnInvoice.TotalGrandAmount,
                CreatedByName = returnInvoice.CreatedBy > 0 ? user1.FirstName + " " + user1.LastName : "N/A",
                CreatedOn = returnInvoice.CreatedOn,
                ApprovedBy = returnInvoice.ApprovedBy,
                //ApprovedByName = returnInvoice.ApprovedBy > 0 ? user2.FirstName + " " + user2.LastName : "N/A",
                ApprovedOn = returnInvoice.ApprovedOn,
                InvoiceReturnDetail = returnInvoice.InvoiceReturnDetail.Where(m => m.InvoiceReturnId == returnInvoice.Id).Select(p => new InvoiceReturnDetailVm
                {
                    Id = p.Id,
                    ProductId = p.ProductId,
                    ProductName = p.Product.Name + " (" + p.Product?.Color + ")",
                    ReturnQuantity = p.Quantity,
                    DeliveryQuantity = p.InvoiceReturn.Invoice.InvoiceDetail.Where(m => m.InvoiceId == returnInvoice.InvoiceId && m.ProductId == p.ProductId).Select(m => m.Quantity),
                    UnitePrice = p.Product.UnitPrice,
                    TotalPrice = (double)p.TotalAmount

                }).ToList()

            };
            return returnVm;
        }
        public double CalculateValue(double amount, double parcentage)
        {
            var percent = parcentage / 100;
            var totalAmount = Math.Round(amount * percent);
            return totalAmount;
        }
        public InvoiceReturn UpdateReturnInvoice(InvoiceReturnVm invoiceReturnVm)
        {
            var invoiceReturn = _PPSDbContext.InvoiceReturn.Where(m => m.Id == invoiceReturnVm.Id).FirstOrDefault();
            if (invoiceReturn == null)
            {
                throw new KeyNotFoundException($"Invoice Return Id: {invoiceReturnVm.Id} does not exist.");
            }
            InvoiceReturn returnInvoice = new InvoiceReturn
            {
                Id = invoiceReturn.Id,
                InvoiceId = invoiceReturn.InvoiceId,
                ReturnDate = invoiceReturn.ReturnDate,
                CreatedBy = invoiceReturn.CreatedBy,
                CreatedOn = invoiceReturn.CreatedOn,
                Note = invoiceReturn.Note,
                TotalAmount = invoiceReturnVm.TotalAmount,
                TotalGrandAmount = invoiceReturnVm.TotalGrandAmount,
                UpdateBy = invoiceReturnVm.UpdatedBy,
                UpdateOn = invoiceReturnVm.UpdateOn,
            };
            _PPSDbContext.InvoiceReturnDetail.RemoveRange(invoiceReturn.InvoiceReturnDetail);

            foreach (var item in invoiceReturnVm.InvoiceReturnDetail)
            {
                var returnDetails = _PPSDbContext.InvoiceReturnDetail.Where(m => m.InvoiceReturnId == invoiceReturn.Id && m.ProductId == item.ProductId).FirstOrDefault();
                if (returnDetails == null)
                {
                    throw new KeyNotFoundException($"Invoice Return details Id: {returnDetails.Id} does not exist.");
                }
                InvoiceReturnDetail detail = new InvoiceReturnDetail
                {
                    InvoiceReturnId = invoiceReturn.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    TotalAmount = (decimal)item.TotalPrice
                };
                _PPSDbContext.InvoiceReturnDetail.Add(detail);
            }
            _PPSDbContext.SaveChanges();
            return _salesRepository.UpdateReturnInvoice(returnInvoice);
        }

        public InvoiceReturn ApproveReturnInvoice(InvoiceReturnVm invoiceReturnVm)
        {
            return _salesRepository.ApproveReturnInvoice(invoiceReturnVm);
        }
        //Invoice return process end

        //Invoice Delivery Challa Start

        public List<DeliveryQuantityVm> InvoiceDeliveryChallanList()
        {
            var deliveryList = _salesRepository.InvoiceDeliveryChallanList().Select(p => new DeliveryQuantityVm
            {
                Id = p.Id,
                InvoiceId = p.InvoiceId,
                InvoiceNo = p.Invoice.InvoiceNo,
                ChallanDate = p.ChallanDate,
                CreatedBy = p.CreatedBy,
                CreatedByName = p.Invoice.DemandOrder.User.FirstName + " " + p.Invoice.DemandOrder.User.LastName,
                CreatedOn = p.CreatedOn,
                ApprovedBy = p.ApprovedBy,
            }).ToList();
            return deliveryList;
        }

        public DeliveryQuantity DeliveryQuantitySave(DeliveryQuantityVm deliveryQuantityVm)
        {
            var invoice = _PPSDbContext.Invoice.Where(m => m.Id == deliveryQuantityVm.InvoiceId).FirstOrDefault();
            var deliveredQuantity = _PPSDbContext.DeliveryQuantity.Where(m => m.InvoiceId == invoice.Id).SelectMany(p => p.DeliveryQuantityDetail).Sum(p => p.Quantity);
            if (invoice == null)
            {
                throw new Exception("Invoice Id " + deliveryQuantityVm.InvoiceId + " not found");
            }
            if (deliveredQuantity >= invoice.InvoiceDetail.Sum(p => p.Quantity))
            {
                throw new Exception("Invoice Quantity already delivered");
            }
            DeliveryQuantity model = new DeliveryQuantity
            {
                InvoiceId = deliveryQuantityVm.InvoiceId,
                ChallanDate = deliveryQuantityVm.ChallanDate,
                CreatedBy = deliveryQuantityVm.CreatedBy,
                CreatedOn = deliveryQuantityVm.CreatedOn,
                Note = deliveryQuantityVm.Note
            };
            foreach (var deliveryDetail in deliveryQuantityVm.DeliveryQuantityDetail)
            {
                model.DeliveryQuantityDetail.Add(new DeliveryQuantityDetail
                {
                    DeliveryQuantityId = model.Id,
                    ProductId = deliveryDetail.ProductId,
                    Quantity = deliveryDetail.Quantity

                });
            }
            return _salesRepository.DeliveryQuantitySave(model);

        }
        public DeliveryQuantityVm GetDeliveryQuantityById(int id)
        {

            DeliveryQuantityVm data = new DeliveryQuantityVm();
            List<DeliveryQuantityDetailVm> dataList = new List<DeliveryQuantityDetailVm>();
            var deliveryChallan = _salesRepository.GetDeliveryQuantityById(id);
            var invoiceDetails = _PPSDbContext.InvoiceDetail.Where(m => m.InvoiceId == deliveryChallan.InvoiceId).ToList();
            data.Id = deliveryChallan.Id;
            data.InvoiceId = deliveryChallan.InvoiceId;
            data.ChallanDate = deliveryChallan.ChallanDate;
            data.InvoiceNo = deliveryChallan.Invoice.InvoiceNo;
            data.CreatedBy = deliveryChallan.CreatedBy;
            data.CreatedByName = deliveryChallan.CreatedBy > 0 ? GetUserNameById((int)deliveryChallan.CreatedBy).Item1 : "N/A";
            data.CreatedOn = deliveryChallan.CreatedOn;
            data.UpdateBy = deliveryChallan.UpdateBy;
            UpdateByName = deliveryChallan.UpdateBy > 0 ? GetUserNameById((int)deliveryChallan.UpdateBy).Item1 : "N/A";
            data.UpdatedOn = deliveryChallan.UpdatedOn;
            data.ApprovedBy = deliveryChallan.ApprovedBy;
            ApprovedByName = deliveryChallan.ApprovedBy > 0 ? GetUserNameById((int)deliveryChallan.ApprovedBy).Item1 : "N/A";
            data.ApprovedOn = deliveryChallan.ApprovedOn;
            data.Note = deliveryChallan.Note;
            data.CustomerName = deliveryChallan.Invoice.DemandOrder.Customer.CustomerName;
            data.CustomerPhone = deliveryChallan.Invoice.DemandOrder.Customer.CustomerPhone;
            data.CustomerAddress = deliveryChallan.Invoice.DemandOrder.Customer.CustomerAddress;


            foreach (var item in deliveryChallan.DeliveryQuantityDetail)
            {
                if (invoiceDetails.Where(m => m.ProductId == item.ProductId).Any())
                {
                    DeliveryQuantityDetailVm dataVm = new DeliveryQuantityDetailVm
                    {

                        ProductName = item.Product.Name + " (" + item.Product.Color + ")",
                        Quantity = item.Quantity,

                    };
                    dataList.Add(dataVm);
                }
            }
            data.DeliveryQuantityDetail = dataList;
            return data;
        }
        public DeliveryQuantity DeliveryQuantityUpdate(DeliveryQuantityVm deliveryQuantityVm)
        {
            var deliveryChallan = _PPSDbContext.DeliveryQuantity.Where(m => m.Id == deliveryQuantityVm.Id).FirstOrDefault();
            if (deliveryChallan == null)
            {
                throw new Exception("Delivery Challan Id " + deliveryQuantityVm.Id + " not found");
            }
            DeliveryQuantity model = new DeliveryQuantity
            {
                Id = deliveryChallan.Id,
                InvoiceId = deliveryChallan.InvoiceId,
                CreatedBy = deliveryChallan.CreatedBy,
                CreatedOn = deliveryChallan.CreatedOn,
                ChallanDate = deliveryQuantityVm.ChallanDate,
                UpdateBy = deliveryQuantityVm.UpdateBy,
                UpdatedOn = deliveryQuantityVm.UpdatedOn,
                Note = deliveryQuantityVm.Note,
            };

            if (deliveryQuantityVm.DeliveryQuantityDetail.Count() > 0)
            {
                _PPSDbContext.DeliveryQuantityDetail.RemoveRange(deliveryChallan.DeliveryQuantityDetail);
                foreach (var data in deliveryQuantityVm.DeliveryQuantityDetail)
                {
                    DeliveryQuantityDetail VModel = new DeliveryQuantityDetail
                    {

                        DeliveryQuantityId = deliveryChallan.Id,
                        ProductId = data.ProductId,
                        Quantity = data.Quantity
                    };
                    _PPSDbContext.DeliveryQuantityDetail.Add(VModel);
                }
                _PPSDbContext.SaveChanges();
            }

            return _salesRepository.DeliveryQuantityUpdate(model);
        }
        public DeliveryQuantity ApproveDeliveryQuantityById(DeliveryQuantityVm deliveryQuantity)
        {
            var deliveryApproved = _PPSDbContext.DeliveryQuantity.Where(m => m.Id == deliveryQuantity.Id).FirstOrDefault();
            if (deliveryApproved == null)
            {
                throw new Exception("Delivery Challan Id " + deliveryQuantity.Id + " not found");
            }
            DeliveryQuantity data = new DeliveryQuantity
            {
                Id = deliveryApproved.Id,
                InvoiceId = deliveryApproved.InvoiceId,
                ChallanDate = deliveryApproved.ChallanDate,
                CreatedBy = deliveryApproved.CreatedBy,
                CreatedOn = deliveryApproved.CreatedOn,
                UpdateBy = deliveryApproved.UpdateBy > 0 ? deliveryApproved.UpdateBy : null,
                UpdatedOn = deliveryApproved.UpdateBy > 0 ? deliveryApproved.UpdatedOn : null,
                ApprovedBy = deliveryQuantity.ApprovedBy,
                ApprovedOn = deliveryQuantity.ApprovedOn,
                Note = deliveryApproved.Note
            };

            return _salesRepository.ApproveDeliveryQuantityById(data);
        }

        public UndeliveryQuantityVm GetUndeliveryQuantityById(int id)
        {
            UndeliveryQuantityVm data = new UndeliveryQuantityVm();
            List<UndeliveryQuantityDetailVm> dataList = new List<UndeliveryQuantityDetailVm>();

            var deliveryChallan = _salesRepository.GetUndeliveryQuantityById(id);
            var invoiceDetails = _PPSDbContext.InvoiceDetail.Where(m => m.InvoiceId == deliveryChallan.InvoiceId).ToList();
            data.InvoiceNo = deliveryChallan.Invoice.InvoiceNo;
            data.CreatedBy = deliveryChallan.CreatedBy;
            data.CreatedByName = deliveryChallan.CreatedBy > 0 ? GetUserNameById((int)deliveryChallan.CreatedBy).Item1 : "N/A";
            data.CreatedOn = deliveryChallan.CreatedOn;
            data.Note = deliveryChallan.Note;
            data.CustomerName = deliveryChallan.Invoice.DemandOrder.Customer.CustomerName;
            data.CustomerPhone = deliveryChallan.Invoice.DemandOrder.Customer.CustomerPhone;
            data.CustomerAddress = deliveryChallan.Invoice.DemandOrder.Customer.CustomerAddress;
            foreach (var item in deliveryChallan.DeliveryQuantityDetail)
            {
                if (invoiceDetails.Where(m => m.ProductId == item.ProductId).Any())
                {
                    UndeliveryQuantityDetailVm dataVm = new UndeliveryQuantityDetailVm
                    {

                        ProductName = item.Product.Name + " (" + item.Product.Color + ")",
                        Quantity = invoiceDetails.Where(m => m.ProductId == item.ProductId).Sum(m => m.Quantity) - item.Quantity,

                    };
                    dataList.Add(dataVm);
                }

            }
            data.UndeliveryQuantityDetailVm = dataList;
            return data;

        }

        public DeliveryChallanInvoiceVm InvoiceDetailsByIdForDeliveryChallan(int id)
        {
            var invoiceDetail = _salesRepository.InvoiceDetailsByIdForDeliveryChallan(id);

            if (invoiceDetail == null)
            {
                throw new Exception("Invoice Not Found");
            }
            DeliveryChallanInvoiceVm deliveryInvoiceVm = new DeliveryChallanInvoiceVm();
            List<DeliveryChallanInvoiceDetailVm> dataList = new List<DeliveryChallanInvoiceDetailVm>();

            deliveryInvoiceVm.InvoiceId = invoiceDetail.Id;
            deliveryInvoiceVm.CustomerCode = invoiceDetail.DemandOrder.Customer.CustomerCode;
            deliveryInvoiceVm.CustomerName = invoiceDetail.DemandOrder.Customer.CustomerName;
            deliveryInvoiceVm.EmployeeName = invoiceDetail.DemandOrder.Employee.FirstName + " " + invoiceDetail.DemandOrder.Employee.LastName;
            deliveryInvoiceVm.EmployeeCode = invoiceDetail.DemandOrder.Employee.EmployeeCode;

            foreach (var item in invoiceDetail.InvoiceDetail)
            {
                var challanQuantity = _PPSDbContext.DeliveryQuantity.Where(m => m.InvoiceId == item.InvoiceId).SelectMany(m => m.DeliveryQuantityDetail.Where(p => p.ProductId == item.ProductId)).Sum(p => p.Quantity);
                DeliveryChallanInvoiceDetailVm data = new DeliveryChallanInvoiceDetailVm
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product.Name + " (" + item.Product.Color + ")",
                    InvoiceQuantity = item.Quantity,
                    AvailableQuantity = item.Quantity - challanQuantity
                };

                dataList.Add(data);
            };
            deliveryInvoiceVm.DeliveryChallanInvoiceDetailList = dataList;
            return deliveryInvoiceVm;
        }
        //Invoice Delivery Challan End
        // Total sales report service

        public List<TotalSalesReportVm> GetTotalSalesReportList(DateTime startDate, DateTime endDate)
        {
            List<TotalSalesReportVm> salesVm = new List<TotalSalesReportVm>();
            if (startDate == null && endDate == null)
            {
                throw new ArgumentNullException("Date is null");
            }

            var totalSalesReport = _salesRepository.GetTotalSalesReportList(startDate, endDate);
            var salesReport = (from entry in totalSalesReport
                               group entry by DbFunctions.TruncateTime(entry.DeliveredOn) into saleDetails
                               select new TotalSalesReportVm
                               {
                                   SalesDate = (DateTime)saleDetails.FirstOrDefault().DeliveredOn,
                                   TotalAmount = (double)saleDetails.Sum(m => m.TotalGrandAmount)
                               }).ToList();

            return salesReport;

        }


    }
}