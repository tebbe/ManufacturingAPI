using PPS.API.Shared.Enums;
using PPS.API.Shared.Extensions;
using PPS.API.Shared.RequestVm;
using PPS.API.Shared.Utility;
using PPS.API.Shared.ViewModel.Employee;
using PPS.API.Shared.ViewModel.Filter;
using PPS.API.Shared.ViewModel.Sales;
using PPS.Data.Edmx;
using PPS.Data.RepositoryInterfaces;
using PPS.Shared.Service.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace PPS.Data.Repositories
{
    public class SalesRepository : ISalesRepository
    {

        static readonly object invLock = new object();
        private readonly PPSDbContext _ppsDbContext;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public SalesRepository()
        {
            _ppsDbContext = new PPSDbContext();
            _transactionRepository = new TransactionRepository();
            _employeeRepository = new EmployeeRepository();
        }
        public List<SaleTypeModel> GetSaleType()
        {
            var saleType = (from sType in _ppsDbContext.SaleType
                            select new SaleTypeModel
                            {
                                Id = sType.Id,
                                SaleTypeName = sType.SaleTypeName,
                                Duration = sType.DurationInDays
                            }).ToList();

            return saleType.ToList();
        }
        public List<DemandOrderTypeModel> GetDemandOrderType()
        {
            var demandOrderType = (from dOrderType in _ppsDbContext.DemandOrderType
                                   select new DemandOrderTypeModel
                                   {
                                       Id = dOrderType.Id,
                                       DemandOrderTypeName = dOrderType.DemandOrderTypeName
                                   }).ToList();

            return demandOrderType.ToList();
        }
        public List<DiscountTypeModel> GetDiscountType()
        {
            var discountType = (from disType in _ppsDbContext.DiscountType
                                select new DiscountTypeModel
                                {
                                    Id = disType.Id,
                                    DiscountTypeName = disType.DiscountTypeName
                                }).ToList();

            return discountType.ToList();
        }
        public List<ProductModel> GetProductList()
        {
            var prdList = new List<ProductModel>();
            var prds = _ppsDbContext.Product.ToList();
            prds.ForEach(p =>
            {
                StringBuilder str = new StringBuilder();
                var productName = "";
                if (!string.IsNullOrEmpty(p.Color))
                {
                    str.Append(p.Color).Append(", ");
                }
                if (!string.IsNullOrEmpty(p.Thickness))
                {
                    str.Append(p.Thickness).Append(", ");
                }
                if (p.Length != null)
                {
                    str.Append(p.Length + " " + p.UnitType.UnitTypeName).Append(", ");
                }

                if (str.Length > 0)
                {
                    str.Remove(str.Length - 2, 1);
                    productName = "(" + str.ToString().Trim() + ")";
                }

                prdList.Add(new ProductModel
                {
                    Id = p.Id,
                    Name = p.Code != null ? p.Code + " - " + p.Name + productName : p.Name + productName,
                    Code = p.Code,
                    Color = p.Color,
                    ProductStandardTypeId = p.ProductStandardTypeId,
                    Thickness = p.Thickness,
                    Length = p.Length,
                    UnitTypeId = p.UnitTypeId,
                    UnitPrice = p.UnitPrice,
                    ProductTypeId = p.ProductTypeId,
                    ProductTypeGroupId = p.ProductType.ProductTypeGroupId,
                    AccountHeadId = p.AccountHeadId
                });
            });

            return prdList;

            //var products = (from product in _ppsDbContext.Product
            //                select new ProductModel
            //                {
            //                    Id = product.Id,
            //                    Name = product.Name + ((!string.IsNullOrEmpty(product.Color) || !string.IsNullOrEmpty(product.Thickness) || product.Length != null) ? ("(" + (string.IsNullOrEmpty(product.Color) ? "" : product.Color + ", ") + (string.IsNullOrEmpty(product.Thickness) ? "" : product.Thickness + ", ") + product.Length == null ? "" : (product.Length + product.UnitType.UnitTypeName + ")")) : ""),
            //                    Code = product.Code,
            //                    UnitPrice = product.UnitPrice,
            //                    ProductTypeId = product.ProductTypeId
            //                }).ToList();

            //return products.ToList();
        }
        public DemandOrderModel SaveDemandOrder(DemandOrderModel demandOrderEntry)
        {
            var lastDemandOrder = _ppsDbContext.DemandOrder
               .OrderByDescending(x => x.DemandOrderNo)
               .FirstOrDefault();

            var lastNumber = 0;
            if (lastDemandOrder != null)
            {
                lastNumber = lastDemandOrder.DemandOrderNo;
            }
            demandOrderEntry.DemandOrderNo = lastNumber + 1;

            var hasRegularDiscount = WebConfigurationManager.AppSettings["RegularDiscount"] ?? "false";
            var hasSpecialDiscount = WebConfigurationManager.AppSettings["SpecialDiscount"] ?? "false";
            var hasAdditionalDiscount = WebConfigurationManager.AppSettings["AdditionalDiscount"] ?? "false";
            var hasExtraDiscount = WebConfigurationManager.AppSettings["ExtraDiscount"] ?? "false";
            var hasCashBack = WebConfigurationManager.AppSettings["CashBack"] ?? "false";
            var discountAfterDiscount = WebConfigurationManager.AppSettings["DiscountAfterDiscount"] ?? "false";

            var hasDemandOrderCreatedBySalesDoSatusId =
                ConfigUtils.GetSafeAppSettingBoolValue("DemandOrderCreatedBySales") == true
                    ? (int)DemandOrderStatusEnum.Submitted
                    : (int)DemandOrderStatusEnum.Initiated;

            var doEntry = new DemandOrder
            {
                DemandOrderNo = demandOrderEntry.DemandOrderNo,
                CustomerId = demandOrderEntry.CustomerId,
                DemandOrderTypeId = demandOrderEntry.DemandOrderTypeId,
                DODate = demandOrderEntry.DODate,
                ReferenceDONo = demandOrderEntry.ReferenceDONo,
                SaleTypeId = demandOrderEntry.SaleTypeId,
                Note = demandOrderEntry.Note,
                EmployeeId = demandOrderEntry.EmployeeId,
                TotalAmount = demandOrderEntry.TotalAmount,

                RegularDiscountInPercentage = string.Compare(hasRegularDiscount, "true", StringComparison.OrdinalIgnoreCase) == 0 ? demandOrderEntry.RegularDiscountInPercentage : null,
                RegularDiscountAmount = string.Compare(hasRegularDiscount, "true", StringComparison.OrdinalIgnoreCase) == 0 ? demandOrderEntry.RegularDiscountAmount : null,
                SpecialDiscountInPercentage = string.Compare(hasSpecialDiscount, "true", StringComparison.OrdinalIgnoreCase) == 0 ? demandOrderEntry.SpecialDiscountInPercentage : null,
                SpecialDiscountAmount = string.Compare(hasSpecialDiscount, "true", StringComparison.OrdinalIgnoreCase) == 0 ? demandOrderEntry.SpecialDiscountAmount : null,
                AdditionalDiscountInPercentage = string.Compare(hasAdditionalDiscount, "true", StringComparison.OrdinalIgnoreCase) == 0 ? demandOrderEntry.AdditionalDiscountInPercentage : null,
                AdditionalDiscountAmount = string.Compare(hasAdditionalDiscount, "true", StringComparison.OrdinalIgnoreCase) == 0 ? demandOrderEntry.AdditionalDiscountAmount : null,
                ExtraDiscountInPercentage = string.Compare(hasExtraDiscount, "true", StringComparison.OrdinalIgnoreCase) == 0 ? demandOrderEntry.ExtraDiscountInPercentage : null,
                ExtraDiscountAmount = string.Compare(hasExtraDiscount, "true", StringComparison.OrdinalIgnoreCase) == 0 ? demandOrderEntry.ExtraDiscountAmount : null,
                CashBackAmount = string.Compare(hasCashBack, "true", StringComparison.OrdinalIgnoreCase) == 0 ? demandOrderEntry.CashBackAmount : null,

                TotalDiscountAmount = demandOrderEntry.TotalDiscountAmount,
                TotalGrandAmount = demandOrderEntry.TotalGrandAmount,
                CreatedBy = demandOrderEntry.CreatedBy,
                CreatedOn = demandOrderEntry.CreatedOn,
                IsCurrentRecord = true,
                DemandOrderStatusId = hasDemandOrderCreatedBySalesDoSatusId,
                PaymentStatusId = (int)PaymentStatusEnum.Unpaid,
                DemandOrderDetail = new List<DemandOrderDetail>()
            };
            var preProductTypeGroupId = -1;
            foreach (var tempDoEntry in demandOrderEntry.DemandOrderDetail)
            {
                var unitPrice = _ppsDbContext.Product.FirstOrDefault(x => x.Id == tempDoEntry.ProductId)?.UnitPrice ?? 0;
                if (preProductTypeGroupId == -1 || tempDoEntry.ProductTypeGroupId == preProductTypeGroupId)
                {
                    doEntry.DemandOrderDetail.Add(
                        new DemandOrderDetail
                        {
                            DemandOrderId = doEntry.Id,
                            ProductId = tempDoEntry.ProductId,
                            Quantity = tempDoEntry.Quantity,
                            Discount = tempDoEntry.Discount,
                            UnitPrice = (double)unitPrice,
                            TotalPrice = tempDoEntry.TotalPrice
                        });
                }
                else
                {
                    throw new Exception("You can't add different product within a demand order.");
                }
                preProductTypeGroupId = tempDoEntry.ProductTypeGroupId ?? 0;
            }

            DemandOrderCalculation(doEntry);

            _ppsDbContext.DemandOrder.Add(doEntry);
            _ppsDbContext.SaveChanges();
            demandOrderEntry.Id = doEntry.Id;
            return demandOrderEntry;
        }
        public DemandOrderModel UpdateDemandOrder(DemandOrderModel demandOrderEntryVm)
        {
            using (var db = new PPSDbContext())
            {
                var doEntry = db.DemandOrder.FirstOrDefault(x => x.Id == demandOrderEntryVm.Id);
                if (doEntry == null)
                {
                    throw new KeyNotFoundException($"Demand Order Id: {demandOrderEntryVm.Id} does not exist.");
                }
                //if (doEntry.VerifiedBy != null)
                //{
                //    throw new Exception($"Demand Order Id: { demandOrderEntryVm.Id } has already been verified.");
                //}
                if (doEntry.ApprovedBy != null)
                {
                    throw new Exception($"Demand Order Id: { demandOrderEntryVm.Id } has already been approved.");
                }

                var hasRegularDiscount = WebConfigurationManager.AppSettings["RegularDiscount"] ?? "false";
                var hasSpecialDiscount = WebConfigurationManager.AppSettings["SpecialDiscount"] ?? "false";
                var hasAdditionalDiscount = WebConfigurationManager.AppSettings["AdditionalDiscount"] ?? "false";
                var hasExtraDiscount = WebConfigurationManager.AppSettings["ExtraDiscount"] ?? "false";
                var hasCashBack = WebConfigurationManager.AppSettings["CashBack"] ?? "false";

                doEntry.CustomerId = demandOrderEntryVm.CustomerId;
                doEntry.DemandOrderTypeId = demandOrderEntryVm.DemandOrderTypeId;
                doEntry.DODate = demandOrderEntryVm.DODate;
                doEntry.ReferenceDONo = demandOrderEntryVm.ReferenceDONo;
                doEntry.Note = demandOrderEntryVm.Note;
                doEntry.SaleTypeId = demandOrderEntryVm.SaleTypeId;
                doEntry.EmployeeId = demandOrderEntryVm.EmployeeId;
                doEntry.TotalAmount = demandOrderEntryVm.TotalAmount;

                doEntry.RegularDiscountInPercentage = string.Compare(hasRegularDiscount, "true", StringComparison.OrdinalIgnoreCase) == 0 ? demandOrderEntryVm.RegularDiscountInPercentage : null;
                doEntry.RegularDiscountAmount = string.Compare(hasRegularDiscount, "true", StringComparison.OrdinalIgnoreCase) == 0 ? demandOrderEntryVm.RegularDiscountAmount : null;
                doEntry.SpecialDiscountInPercentage = string.Compare(hasSpecialDiscount, "true", StringComparison.OrdinalIgnoreCase) == 0 ? demandOrderEntryVm.SpecialDiscountInPercentage : null;
                doEntry.SpecialDiscountAmount = string.Compare(hasSpecialDiscount, "true", StringComparison.OrdinalIgnoreCase) == 0 ? demandOrderEntryVm.SpecialDiscountAmount : null;
                doEntry.AdditionalDiscountInPercentage = string.Compare(hasAdditionalDiscount, "true", StringComparison.OrdinalIgnoreCase) == 0 ? demandOrderEntryVm.AdditionalDiscountInPercentage : null;
                doEntry.AdditionalDiscountAmount = string.Compare(hasAdditionalDiscount, "true", StringComparison.OrdinalIgnoreCase) == 0 ? demandOrderEntryVm.AdditionalDiscountAmount : null;
                doEntry.ExtraDiscountInPercentage = string.Compare(hasExtraDiscount, "true", StringComparison.OrdinalIgnoreCase) == 0 ? demandOrderEntryVm.ExtraDiscountInPercentage : null;
                doEntry.ExtraDiscountAmount = string.Compare(hasExtraDiscount, "true", StringComparison.OrdinalIgnoreCase) == 0 ? demandOrderEntryVm.ExtraDiscountAmount : null;
                doEntry.CashBackAmount = string.Compare(hasCashBack, "true", StringComparison.OrdinalIgnoreCase) == 0 ? demandOrderEntryVm.CashBackAmount : null;

                doEntry.TotalDiscountAmount = demandOrderEntryVm.TotalDiscountAmount;
                doEntry.TotalGrandAmount = demandOrderEntryVm.TotalGrandAmount;

                var doEntryDetail = doEntry.DemandOrderDetail;
                doEntry.DemandOrderDetail = new List<DemandOrderDetail>();
                var preProductTypeGroupId = -1;
                foreach (var doDetail in demandOrderEntryVm.DemandOrderDetail)
                {
                    var unitPrice = _ppsDbContext.Product.FirstOrDefault(x => x.Id == doDetail.ProductId)?.UnitPrice ?? 0;
                    if (preProductTypeGroupId == -1 || doDetail.ProductTypeGroupId == preProductTypeGroupId)
                    {
                        doEntry.DemandOrderDetail.Add(
                            new DemandOrderDetail
                            {
                                ProductId = doDetail.ProductId,
                                Quantity = doDetail.Quantity,
                                Discount = doDetail.Discount,
                                UnitPrice = (double)unitPrice,
                                TotalPrice = doDetail.TotalPrice
                            });
                    }
                    else
                    {
                        throw new Exception("You can't add different product within a demand order.");
                    }
                    preProductTypeGroupId = doDetail.ProductTypeGroupId ?? 0;
                }

                DemandOrderCalculation(doEntry);

                db.DemandOrderDetail.RemoveRange(doEntryDetail);
                db.DemandOrder.Attach(doEntry);
                db.Entry(doEntry).State = EntityState.Modified;
                db.SaveChanges();
                demandOrderEntryVm.Id = doEntry.Id;
                return demandOrderEntryVm;
            }
        }
        public IList<DemandOrderVm> GetDemandOrderList(int userId, int paymentStatus)
        {
            var user = _ppsDbContext.User.FirstOrDefault(x => x.Id == userId);
            var employee = _ppsDbContext.Employee.FirstOrDefault(x => x.Id == user.EmployeeId);
            List<DemandOrder> doList;
            if (employee != null && employee.DepartmentId == (int)DepartmentEnum.SalesAndMarketing)
            {
                var employeeRepository = new EmployeeRepository();
                var employeeIdList = employeeRepository.GetManagedEmployee(employee.Id).Select(x => x.Item1);
                if (paymentStatus == 0)
                {
                    doList = _ppsDbContext.DemandOrder.Where(x => employeeIdList.Contains(x.EmployeeId)).ToList();
                }
                else
                {
                    doList = _ppsDbContext.DemandOrder.Where(x => x.PaymentStatusId == paymentStatus && employeeIdList.Contains(x.EmployeeId)).ToList();
                }
            }
            else
            {
                if (paymentStatus == 0)
                {
                    doList = _ppsDbContext.DemandOrder.ToList();
                }
                else
                {
                    doList = _ppsDbContext.DemandOrder.Where(x => x.PaymentStatusId == paymentStatus).ToList();
                }
            }

            var doVm = new ConcurrentBag<DemandOrderVm>();
            doList.ForEach(d =>
            {
                var totalPaid = d.DemandOrderTransaction.ToList().Sum(x => x.TransactionAmount);
                var totalDue = d.TotalGrandAmount - totalPaid;

                int maturityDays = (DateTime.Today - d.DODate).Days;
                int maturityLabel = (int)DoMaturityLabelEnum.Normal;
                if (totalDue == 0)
                {
                    maturityDays = 0;
                    maturityLabel = (int)DoMaturityLabelEnum.Paid;
                }
                else
                {
                    if (maturityDays > d.SaleType?.DurationInDays)
                    {
                        maturityLabel = (int)DoMaturityLabelEnum.OverDue;
                    }
                    else if (maturityDays > d.SaleType?.WarningInDays)
                    {
                        maturityLabel = (int)DoMaturityLabelEnum.Warning;
                    }
                }

                doVm.Add(new DemandOrderVm
                {
                    Id = d.Id,
                    DemandOrderNo = d.DemandOrderNo,
                    DODate = d.DODate,
                    SaleTypeName = d.SaleType?.SaleTypeName,
                    DemandOrderTypeName = d.DemandOrderType?.DemandOrderTypeName,
                    MaturityDays = maturityDays,
                    MaturityLabel = maturityLabel,
                    ReferenceNo = d.ReferenceDONo,
                    CustomerName = d.Customer.CustomerName,
                    DOStatusName = d.DemandOrderStatus.Status,
                    TotalGrandAmount = d.TotalGrandAmount,
                    CreatedByName = StringExtension.ToFullName(d.User.FirstName, d.User.LastName),
                    CreatedDate = d.CreatedOn,
                    Submitted = d.SubmittedBy != null,
                    DOPaymentStatusId = GetDemandOrderTransactionStatus(d, totalDue),
                    DOPaymentStatus = d.PaymentStatus.PaymentStatusName,
                    ProductTypeGroupId = d.DemandOrderDetail.First().Product.ProductType.ProductTypeGroupId,
                    ProductTypeGroupName = d.DemandOrderDetail.First().Product.ProductType.ProductTypeGroup.ProductTypeGroupName,
                    InvoiceList = d.Invoice.Where(m => m.DemandOrderId == d.Id).Select(m => new InvoiceVm
                    {
                        Id = m.Id,
                    }).ToList()
                });
            });
            return doVm.ToList();
        }
        //private IQueryable<DemandOrder> OrderByField<DemandOrder>(IQueryable<DemandOrder> q, string SortField, bool Ascending)
        //{
        //    var param = Expression.Parameter(typeof(DemandOrder), "p");
        //    var prop = Expression.Property(param, SortField);
        //    var exp = Expression.Lambda(prop, param);
        //    string method = Ascending ? "OrderBy" : "OrderByDescending";
        //    Type[] types = new Type[] { q.ElementType, exp.Body.Type };
        //    var mce = Expression.Call(typeof(Queryable), method, types, q.Expression, exp);
        //    return q.Provider.CreateQuery<DemandOrder>(mce);
        //}
        public IList<DemandOrderVm> GetDemandOrderListForFiltering(int userId, int paymentStatus, FilterVm filterVm)
        {
            var totalCount = 0;
            var user = _ppsDbContext.User.FirstOrDefault(x => x.Id == userId);
            var employee = _ppsDbContext.Employee.FirstOrDefault(x => x.Id == user.EmployeeId);
            List<DemandOrder> doList;

            var query = _ppsDbContext.DemandOrder.OrderByField(filterVm.SortColumn, filterVm.SortDirection.ToString().Equals(SortDirectionEnum.ASC.ToString()));
            if (filterVm.StartDate != null && filterVm.EndDate != null && filterVm.CustomerId == 0)
            {
                query = query.Where(m => m.CreatedOn >= filterVm.StartDate && m.CreatedOn <= filterVm.EndDate);
            }
            else if (filterVm.StartDate != null && filterVm.EndDate != null && filterVm.CustomerId > 0)
            {
                query = query.Where(m => m.CreatedOn >= filterVm.StartDate && m.CreatedOn <= filterVm.EndDate && m.CustomerId == filterVm.CustomerId);
            }
            else if (filterVm.StartDate == null && filterVm.EndDate == null && filterVm.CustomerId > 0)
            {
                query= query.Where(m=>m.CustomerId == filterVm.CustomerId);
            }
            if (employee != null && employee.DepartmentId == (int)DepartmentEnum.SalesAndMarketing)
            {
                var employeeRepository = new EmployeeRepository();
                var employeeIdList = employeeRepository.GetManagedEmployee(employee.Id).Select(x => x.Item1);

                doList = query.Where(x => employeeIdList.Contains(x.EmployeeId)).Skip((filterVm.PageIndex - 1) * filterVm.PageSize).Take(filterVm.PageSize).ToList();
                totalCount = query.Where(x => employeeIdList.Contains(x.EmployeeId)).Count();

                //if (paymentStatus == 0)
                //{
                //    //doList = _ppsDbContext.DemandOrder.Where(x => employeeIdList.Contains(x.EmployeeId)).ToList();
                //    if (filterVm.SortDirection == SortDirectionEnum.ASC)
                //    {
                //        doList = _ppsDbContext.DemandOrder.Where(x => employeeIdList.Contains(x.EmployeeId)).OrderBy(y => y.Id).Skip((filterVm.PageIndex - 1) * filterVm.PageSize).Take(filterVm.PageSize).ToList();
                //    }
                //    else
                //    {
                //        doList = _ppsDbContext.DemandOrder.Where(x => employeeIdList.Contains(x.EmployeeId)).OrderByDescending(y => y.Id).Skip((filterVm.PageIndex - 1) * filterVm.PageSize).Take(filterVm.PageSize).ToList();
                //    }
                //    //doList = _ppsDbContext.DemandOrder.Where(x => employeeIdList.Contains(x.EmployeeId)).OrderBy(y => y.Id).Skip((filterVm.PageIndex - 1) * filterVm.PageSize).Take(filterVm.PageSize).ToList();
                //    totalCount = _ppsDbContext.DemandOrder.Where(x => employeeIdList.Contains(x.EmployeeId)).Count();
                //}
                //else
                //{
                //    //doList = _ppsDbContext.DemandOrder.Where(x => x.PaymentStatusId == paymentStatus && employeeIdList.Contains(x.EmployeeId)).ToList();
                //    doList = _ppsDbContext.DemandOrder.Where(x => x.PaymentStatusId == paymentStatus && employeeIdList.Contains(x.EmployeeId)).OrderBy(y => y.Id).Skip((filterVm.PageIndex - 1) * filterVm.PageSize).Take(filterVm.PageSize).ToList();
                //    totalCount = _ppsDbContext.DemandOrder.Where(x => x.PaymentStatusId == paymentStatus && employeeIdList.Contains(x.EmployeeId)).Count();
                //}
            }
            else
            {
                //if (paymentStatus == 0)
                //{
                //    if (string.IsNullOrEmpty(filterVm.SortColumn))
                //    {
                //        doList = _ppsDbContext.DemandOrder.OrderByDescending(y => y.DemandOrderNo).Skip((filterVm.PageIndex - 1) * filterVm.PageSize).Take(filterVm.PageSize).ToList();

                //    }
                //    else
                //    {
                //        var query = _ppsDbContext.DemandOrder.OrderByField(filterVm.SortColumn, filterVm.SortDirection.ToString().Equals(SortDirectionEnum.ASC.ToString()));
                //        doList = query.Skip((filterVm.PageIndex - 1) * filterVm.PageSize).Take(filterVm.PageSize).ToList();
                //    }
                //    totalCount = _ppsDbContext.DemandOrder.Count();
                //}
                //else
                //{
                //    //doList = _ppsDbContext.DemandOrder.Where(x => x.PaymentStatusId == paymentStatus).ToList();
                //    doList = _ppsDbContext.DemandOrder.Where(x => x.PaymentStatusId == paymentStatus).OrderBy(y => y.Id).Skip((filterVm.PageIndex - 1) * filterVm.PageSize).Take(filterVm.PageSize).ToList();
                //    totalCount = _ppsDbContext.DemandOrder.Where(x => x.PaymentStatusId == paymentStatus).Count();
                //}
                doList = query.Skip((filterVm.PageIndex - 1) * filterVm.PageSize).Take(filterVm.PageSize).ToList();
                totalCount = query.Count();

            }

            var doVm = new List<DemandOrderVm>();
            foreach (var d in doList)
            {
                var totalPaid = d.DemandOrderTransaction.ToList().Sum(x => x.TransactionAmount);
                var totalDue = d.TotalGrandAmount - totalPaid;

                int maturityDays = (DateTime.Today - d.DODate).Days;
                int maturityLabel = (int)DoMaturityLabelEnum.Normal;
                if (totalDue == 0)
                {
                    maturityDays = 0;
                    maturityLabel = (int)DoMaturityLabelEnum.Paid;
                }
                else
                {
                    if (maturityDays > d.SaleType?.DurationInDays)
                    {
                        maturityDays = maturityDays - d.SaleType?.DurationInDays ?? 0;
                        maturityLabel = (int)DoMaturityLabelEnum.OverDue;
                    }
                    else if (maturityDays > d.SaleType?.WarningInDays)
                    {
                        maturityDays = maturityDays - d.SaleType?.WarningInDays ?? 0;
                        maturityLabel = (int)DoMaturityLabelEnum.Warning;
                    }
                }
                doVm.Add(new DemandOrderVm
                {
                    Id = d.Id,
                    DemandOrderNo = d.DemandOrderNo,
                    DODate = d.DODate,
                    SaleTypeName = d.SaleType?.SaleTypeName,
                    DemandOrderTypeName = d.DemandOrderType?.DemandOrderTypeName,
                    MaturityDays = maturityDays,
                    MaturityLabel = maturityLabel,
                    ReferenceNo = d.ReferenceDONo,
                    Note = d.Note,
                    CustomerName = d.Customer.CustomerName,
                    DOStatusName = d.DemandOrderStatus.Status,
                    TotalGrandAmount = d.TotalGrandAmount,
                    CreatedByName = StringExtension.ToFullName(d.User.FirstName, d.User.LastName),
                    CreatedDate = d.CreatedOn,
                    Submitted = d.SubmittedBy != null,
                    DOPaymentStatusId = GetDemandOrderTransactionStatus(d, totalDue),
                    DOPaymentStatus = d.PaymentStatus.PaymentStatusName,
                    ProductTypeGroupId = d.DemandOrderDetail.First().Product.ProductType.ProductTypeGroupId,
                    ProductTypeGroupName = d.DemandOrderDetail.First().Product.ProductType.ProductTypeGroup.ProductTypeGroupName,
                    TotalCount = totalCount,
                    InvoiceList = d.Invoice.Where(m => m.DemandOrderId == d.Id).Select(p => new InvoiceVm { Id = p.Id, InvoiceNo = p.InvoiceNo, TotalAmount = p.TotalAmount, TotalGrandAmount = p.TotalGrandAmount, InvoiceDate = p.InvoiceDate }).ToList()
                });
            }
            //doList.ForEach(d =>
            //{
            //    var totalPaid = d.DemandOrderTransaction.ToList().Sum(x => x.TransactionAmount);
            //    var totalDue = d.TotalGrandAmount - totalPaid;

            //    int maturityDays = (DateTime.Today - d.DODate).Days;
            //    int maturityLabel = (int)DoMaturityLabelEnum.Normal;
            //    if (maturityDays > d.SaleType?.DurationInDays)
            //    {
            //        maturityLabel = (int)DoMaturityLabelEnum.OverDue;
            //    }
            //    else if (maturityDays > d.SaleType?.WarningInDays)
            //    {
            //        maturityLabel = (int)DoMaturityLabelEnum.Warning;
            //    }

            //    doVm.Add(new DemandOrderVm
            //    {
            //        Id = d.Id,
            //        DemandOrderNo = d.DemandOrderNo,
            //        DODate = d.DODate,
            //        SaleTypeName = d.SaleType?.SaleTypeName,
            //        DemandOrderTypeName = d.DemandOrderType?.DemandOrderTypeName,
            //        MaturityDays = maturityDays,
            //        MaturityLabel = maturityLabel,
            //        ReferenceNo = d.ReferenceDONo,
            //        CustomerName = d.Customer.CustomerName,
            //        DOStatusName = d.DemandOrderStatus.Status,
            //        TotalGrandAmount = d.TotalGrandAmount,
            //        CreatedByName = StringExtension.ToFullName(d.User.FirstName, d.User.LastName),
            //        CreatedDate = d.CreatedOn,
            //        Submitted = d.SubmittedBy != null,
            //        DOPaymentStatusId = GetDemandOrderTransactionStatus(d, totalDue),
            //        DOPaymentStatus = d.PaymentStatus.PaymentStatusName,
            //        ProductTypeGroupId = d.DemandOrderDetail.First().Product.ProductType.ProductTypeGroupId,
            //        ProductTypeGroupName = d.DemandOrderDetail.First().Product.ProductType.ProductTypeGroup.ProductTypeGroupName,
            //        TotalCount = totalCount
            //    });
            //});
            return doVm;
        }
        public async Task<bool> SubmitDO(int doId, int userId)
        {
            var demandOrder = _ppsDbContext.DemandOrder.FirstOrDefault(x => x.Id == doId);
            if (demandOrder == null)
            {
                throw new KeyNotFoundException($"The demand order no {doId} doesn't exist.");
            }
            if (demandOrder.SubmittedBy != null)
            {
                throw new Exception($"The demand order no: {doId} has already been submitted.");
            }
            demandOrder.SubmittedBy = userId;
            demandOrder.SubmittedOn = DateTime.Now;
            demandOrder.DemandOrderStatusId = (int)DemandOrderStatusEnum.Submitted;
            _ppsDbContext.DemandOrder.Attach(demandOrder);
            _ppsDbContext.Entry(demandOrder).State = EntityState.Modified;
            await _ppsDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> VerifyDO(int doId, int userId)
        {
            var demandOrder = _ppsDbContext.DemandOrder.FirstOrDefault(x => x.Id == doId);
            if (demandOrder == null)
            {
                throw new KeyNotFoundException($"The demand order no {doId} doesn't exist.");
            }
            if (demandOrder.VerifiedBy != null)
            {
                throw new Exception($"The demand order no: {doId} has already been verified.");
            }
            demandOrder.VerifiedBy = userId;
            demandOrder.VerifiedOn = DateTime.Now;
            demandOrder.DemandOrderStatusId = (int)DemandOrderStatusEnum.Verified;
            _ppsDbContext.DemandOrder.Attach(demandOrder);
            _ppsDbContext.Entry(demandOrder).State = EntityState.Modified;
            await _ppsDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeliveryConfirmedDO(int doId, int userId)
        {
            var demandOrder = _ppsDbContext.DemandOrder.FirstOrDefault(x => x.Id == doId);
            if (demandOrder == null)
            {
                throw new KeyNotFoundException($"The demand order no {doId} doesn't exist.");
            }

            if (demandOrder.DeliveryConfirmedBy != null)
            {
                throw new Exception($"The demand order no: {doId} has already been confirmed for delivery.");
            }

            demandOrder.DeliveryConfirmedBy = userId;
            demandOrder.DeliveryConfirmedOn = DateTime.Now;
            demandOrder.DemandOrderStatusId = (int)DemandOrderStatusEnum.Delivered;
            _ppsDbContext.DemandOrder.Attach(demandOrder);
            _ppsDbContext.Entry(demandOrder).State = EntityState.Modified;
            await _ppsDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> ApproveDO(int doId, int userId)
        {
            var currentStock = new CurrentProductStockRepository();
            using (var db = new PPSDbContext())
            {
                var demandOrder = db.DemandOrder.FirstOrDefault(x => x.Id == doId);

                if (demandOrder == null)
                {
                    throw new KeyNotFoundException($"The demand order no {doId} doesn't exist.");
                }

                if (demandOrder.ApprovedBy != null)
                {
                    throw new Exception($"The demand order no: {doId} has already been approved.");
                }

                demandOrder.ApprovedBy = userId;
                demandOrder.ApprovedOn = DateTime.Now;
                demandOrder.DemandOrderStatusId = (int)DemandOrderStatusEnum.Approved;
                db.DemandOrder.Attach(demandOrder);
                db.Entry(demandOrder).State = EntityState.Modified;
                demandOrder.DemandOrderDetail.ToList().ForEach(dOrder =>
                {
                    currentStock.AddAllocatedQuantityByProductId(db, dOrder.ProductId, dOrder.Quantity);
                });

                await db.SaveChangesAsync();
            }
            return true;
        }
        public DemandOrderVm GetDemandOrderById(int userId, int doId)
        {
            var demandOrder = _ppsDbContext.DemandOrder.FirstOrDefault(x => x.Id == doId && x.IsCurrentRecord);
            if (demandOrder == null)
            {
                throw new KeyNotFoundException($"The demand order no {doId} doesn't exist.");
            }
            //added by Zahidul

            //Customer total DO with Invoice
            var customerDemandOrderList = _ppsDbContext.DemandOrder.Where(m => m.CustomerId == demandOrder.CustomerId && m.VerifiedBy > 0).Select(m => m.Id).ToList();
            var customerTotalDOAmount = _ppsDbContext.DemandOrder.Where(m => customerDemandOrderList.Contains(m.Id)).Any() ? _ppsDbContext.DemandOrder.Where(m => customerDemandOrderList.Contains(m.Id)).Sum(m => m.TotalGrandAmount) : 0.0;
            var customerTotalInvoiceList = _ppsDbContext.Invoice.Where(m => customerDemandOrderList.Contains(m.DemandOrderId) && m.DeliveredOn != null).ToList();
            var totalDoMatureInvoiceAmount = 0.0;
            var totalDoImMatureInvoiceAmount = 0.0;
            var totalDoMatureInvoiceTransaction = 0.0;
            var totalDoImMatureInvoiceTransaction = 0.0;
            foreach (var invoice in customerTotalInvoiceList)
            {
                var dayDifference = (DateTime.Now.Date - invoice.DeliveredOn.Value.Date).Days;
                if (dayDifference >= 30)
                {
                    var invoiceTransaction = _ppsDbContext.InvoiceTransaction.Where(m => m.InvoiceId == invoice.Id).Any() ? _ppsDbContext.InvoiceTransaction.Where(m => m.InvoiceId == invoice.Id).Sum(m => m.TransactionAmount) : 0.0;
                    totalDoMatureInvoiceTransaction += invoiceTransaction;
                    totalDoMatureInvoiceAmount += Convert.ToInt64(invoice.TotalGrandAmount);
                }
                else
                {
                    var invoiceTransaction = _ppsDbContext.InvoiceTransaction.Where(m => m.InvoiceId == invoice.Id).Any() ? _ppsDbContext.InvoiceTransaction.Where(m => m.InvoiceId == invoice.Id).Sum(m => m.TransactionAmount) : 0.0;
                    totalDoImMatureInvoiceTransaction += invoiceTransaction;
                    totalDoImMatureInvoiceAmount += Convert.ToInt64(invoice.TotalGrandAmount);
                }
            }
            //Customer total Invoice transaction amount
            var invoiceList = _ppsDbContext.Invoice.Where(x => customerDemandOrderList.Contains(x.DemandOrderId) && x.DeliveredBy > 0).Select(m => m.Id);
            var totalInvoiceTransaction = _ppsDbContext.InvoiceTransaction.Where(x => invoiceList.Contains(x.InvoiceId)).Any() ? _ppsDbContext.InvoiceTransaction.Where(x => invoiceList.Contains(x.InvoiceId)).Sum(m => m.TransactionAmount) : 0.0;
            var customerTransactionList = _ppsDbContext.CustomerTransactionDetail.Where(m => m.CustomerId == demandOrder.CustomerId).Select(m => m.CustomerTransactionId).ToList();
            var customerTotalTransactionBalance = _ppsDbContext.CustomerTransaction.Where(m => customerTransactionList.Contains(m.Id)).Any() ? _ppsDbContext.CustomerTransaction.Where(m => customerTransactionList.Contains(m.Id)).Sum(x => x.TransactionAmount) : 0.0;
            var customerRemainingBalance = customerTotalTransactionBalance - (totalDoMatureInvoiceAmount + totalDoImMatureInvoiceAmount);
            //Single DO with Invoice
            var singleDOInvoiceAmount = _ppsDbContext.Invoice.Where(m => m.DemandOrderId == demandOrder.Id && m.DeliveredBy > 0).Any() ? _ppsDbContext.Invoice.Where(m => m.DemandOrderId == demandOrder.Id && m.DeliveredBy > 0).Sum(m => m.TotalGrandAmount) : 0.0;
            var singleDoInvoiceIdList = _ppsDbContext.Invoice.Where(m => m.DemandOrderId == demandOrder.Id).Select(m => m.Id).ToList();
            var singleDoInvoiceTransactionAmount = _ppsDbContext.InvoiceTransaction.Where(m => singleDoInvoiceIdList.Contains(m.InvoiceId)).Any() ? _ppsDbContext.InvoiceTransaction.Where(m => singleDoInvoiceIdList.Contains(m.InvoiceId)).Sum(m => m.TransactionAmount) : 0.0;

            //End

            // var totalDemandOrderTransactionAmmount = _ppsDbContext.DemandOrderTransaction.Where(x => customerDemandOrderList.Contains(x.DemandOrderId)).Any() ? _ppsDbContext.DemandOrderTransaction.Where(x => customerDemandOrderList.Contains(x.DemandOrderId)).Sum(x => x.TransactionAmount) : 0.0;
            var demandOrderDetail = demandOrder.DemandOrderDetail.ToList();
            var demandOrderDetailVm = new List<DemandOrderDetailVm>();
            demandOrderDetail.ForEach(d =>
            {
                demandOrderDetailVm.Add(new DemandOrderDetailVm
                {
                    Id = d.Id,
                    ProductId = d.ProductId,
                    ProductCode = d.Product?.Code,
                    ProductName = d.Product?.Name + " (" + d.Product?.Color + ")",
                    Quantity = d.Quantity,
                    Length = d.Product?.Length ?? 0,
                    UnitTypeName = d.Product?.UnitType?.UnitTypeName ?? "",
                    ProductTypeGroupId = d.Product?.ProductType?.ProductTypeGroupId,
                    UnitPrice = d.UnitPrice,
                    Discount = d.Discount ?? 0,
                    TotalPrice = d.TotalPrice
                });
            });
            CustomerDOWithInvoiceTransactionDetailsVm customerDOWithInvoiceTransactionDetailsVm = new CustomerDOWithInvoiceTransactionDetailsVm
            {
                TotalDOAmount = customerTotalDOAmount,
                CustomerMatureInvoiceAmount = totalDoMatureInvoiceAmount,
                CustomerImmatureInvoiceAmount = totalDoImMatureInvoiceAmount,
                CustomerTotalInvoiceBalance = totalDoMatureInvoiceAmount + totalDoImMatureInvoiceAmount,
                CustomerTotalTransactionAmount = customerTotalTransactionBalance,
                CustomerTotalMatureDue = totalDoMatureInvoiceAmount - totalDoMatureInvoiceTransaction,
                CustomerTotalImmatureDue = totalDoImMatureInvoiceAmount - totalDoImMatureInvoiceTransaction,
                SingleDOInviceAmount = (double)singleDOInvoiceAmount,
                SingleDoInvoiceTransactionAmount = singleDoInvoiceTransactionAmount,
                SingleDOInvoiceDue = (double)(singleDOInvoiceAmount - singleDoInvoiceTransactionAmount),
            };
            //var demandOrderTransactionVm = new ConcurrentBag<DemandOrderTransactionVm>();
            //var totalPaid = 0D;
            //demandOrder.DemandOrderTransaction.ToList().ForEach(tran =>
            //{
            //    totalPaid += tran.TransactionAmount;
            //    demandOrderTransactionVm.Add(new DemandOrderTransactionVm
            //    {
            //        DemandOrderId = doId,
            //        CreatedByName = GetUserName(tran.CreatedBy),
            //        CreatedOn = tran.CreatedOn,
            //        TransactionAmount = tran.TransactionAmount,
            //        TransactionDate = tran.TransactionDate
            //    });
            //});
            //var totalDue = totalInvoiceAmount - totalInvoiceTransaction;

            var demandOrderVm = new DemandOrderVm
            {
                Id = demandOrder.Id,
                DemandOrderNo = demandOrder.DemandOrderNo,
                DODate = demandOrder.DODate,
                DemandOrderTypeId = demandOrder.DemandOrderTypeId,
                DemandOrderTypeName = demandOrder.DemandOrderType?.DemandOrderTypeName,
                DOStatusName = demandOrder.DemandOrderStatus.Status,
                SaleTypeId = demandOrder.SaleTypeId,
                SaleTypeName = demandOrder.SaleType?.SaleTypeName,
                EmployeeId = demandOrder.EmployeeId,
                EmployeeName = demandOrder.Employee.FirstName + " " + demandOrder.Employee.LastName + " (" + demandOrder.Employee.Designation.DesignationName + ")",
                EmployeeCode = demandOrder.Employee.EmployeeCode,
                CustomerId = demandOrder.CustomerId,
                CustomerName = demandOrder.Customer.CustomerName,
                CustomerCode = demandOrder.Customer.CustomerCode,
                CustomerAddress = demandOrder.Customer.CustomerAddress,
                CustomerMobile = demandOrder.Customer.CustomerMobile,
                Note = demandOrder.Note,
                ReferenceNo = demandOrder.ReferenceDONo,
                TotalAmount = demandOrder.TotalAmount,
                CustomerDoWithInvoiceDetails = customerDOWithInvoiceTransactionDetailsVm,

                InvoiceList = demandOrder.Invoice.Where(m => m.DemandOrderId == demandOrder.Id).Select(p => new InvoiceVm
                {
                    Id = p.Id,
                    InvoiceNo = p.InvoiceNo,
                    InvoiceDate = p.InvoiceDate,
                    TotalAmount = p.TotalGrandAmount,
                    //TotalDiscountInPercent = p.RegularDiscountInPercentage==null?0.0 : p.RegularDiscountInPercentage 
                    //    + p.ExtraDiscountInPercentage==null?0.0: p.ExtraDiscountInPercentage 
                    //    + p.AdditionalDiscountInPercentage==null?0.0:p.AdditionalDiscountInPercentage
                    //    + p.SpecialDiscountInPercentage==null?0.0:p.SpecialDiscountInPercentage,
                    //TotalDiscountAmount=p.SpecialDiscountAmount==null?0.0: p.SpecialDiscountAmount
                    //    + p.RegularDiscountAmount==null?0.0:p.RegularDiscountAmount
                    //    +p.ExtraDiscountAmount==null?0.0:p.ExtraDiscountAmount
                    //    +p.AdditionalDiscountAmount==null?0.0:p.AdditionalDiscountAmount,
                    TotalPaidAmount = p.InvoiceTransaction.Where(m => m.InvoiceId == p.Id).Select(m => m.TransactionAmount).SingleOrDefault(),
                    CreatedBy = p.CreatedBy,
                    ApprovedBy = p.ApprovedBy,
                    DeliveredBy = p.DeliveredBy
                }).ToList(),

                RegularDiscountInPercentage = demandOrder.RegularDiscountInPercentage,
                RegularDiscountAmount = demandOrder.RegularDiscountAmount,
                SpecialDiscountInPercentage = demandOrder.SpecialDiscountInPercentage,
                SpecialDiscountAmount = demandOrder.SpecialDiscountAmount,
                AdditionalDiscountInPercentage = demandOrder.AdditionalDiscountInPercentage,
                AdditionalDiscountAmount = demandOrder.AdditionalDiscountAmount,
                ExtraDiscountInPercentage = demandOrder.ExtraDiscountInPercentage,
                ExtraDiscountAmount = demandOrder.ExtraDiscountAmount,
                CashBackAmount = demandOrder.CashBackAmount,
                TotalDiscountAmount = demandOrder.TotalDiscountAmount,
                TotalGrandAmount = demandOrder.TotalGrandAmount,
                //TotalPaidAmount = totalPaid,

                DemandOrderDetail = demandOrderDetailVm,
                //DemandOrderTransaction = demandOrderTransactionVm.ToList(),
                // DOPaymentStatusId = GetDemandOrderTransactionStatus(demandOrder, totalDue),
                CreatedDate = demandOrder.CreatedOn,
                CreatedByName = GetUserNameById(demandOrder.CreatedBy).Item1,
                //TotalDoBalanceAmount = totalDoBalance > 0 ? totalDoBalance : 0.0,
                CustomerRemainingBalance = customerRemainingBalance > 0 ? customerRemainingBalance : 0.0,
                CreatedByDesignation = GetUserNameById(demandOrder.CreatedBy).Item2,
                CreatedOn = demandOrder.CreatedOn,
                VerifiedOn = demandOrder.VerifiedOn,
                ApprovedOn = demandOrder.ApprovedOn,
                VerifiedByName = demandOrder.VerifiedBy != null ? GetUserNameById((int)demandOrder.VerifiedBy).Item1 : null,
                VerifiedByDesignation = demandOrder.VerifiedBy != null ? GetUserNameById((int)demandOrder.VerifiedBy).Item2 : null,
                SubmittedByOn = demandOrder.SubmittedBy != null && demandOrder.SubmittedOn != null
                    ? GetUserByOn((int)demandOrder.SubmittedBy, (DateTime)demandOrder.SubmittedOn)
                    : "-",
                VerifiedByOn = demandOrder.VerifiedBy != null && demandOrder.VerifiedOn != null
                    ? GetUserByOn((int)demandOrder.VerifiedBy, (DateTime)demandOrder.VerifiedOn)
                    : "-",
                ApprovedByOn = demandOrder.ApprovedBy != null && demandOrder.ApprovedOn != null
                    ? GetUserByOn((int)demandOrder.ApprovedBy, (DateTime)demandOrder.ApprovedOn)
                    : "-",
                DeliveredByOn = demandOrder.DeliveryConfirmedBy != null && demandOrder.DeliveryConfirmedOn != null
                    ? GetUserByOn((int)demandOrder.DeliveryConfirmedBy, (DateTime)demandOrder.DeliveryConfirmedOn)
                    : "-"
            };

            return demandOrderVm;
        }
        public IList<PostOfficeModel> GetPostOfficeList()
        {
            var poList = (from po in _ppsDbContext.PostOffice
                          select new PostOfficeModel()
                          {
                              Id = po.Id,
                              PostOfficeName = po.PostOfficeName,
                              PostCode = po.PostCode,
                              PoliceStationId = po.PoliceStationId
                          }).ToList();
            return poList.ToList();
        }
        public IList<AreaModel> GetAreaList()
        {
            var areaList = (from ar in _ppsDbContext.Area
                            select new AreaModel()
                            {
                                Id = ar.Id,
                                AreaName = ar.AreaName
                            }).ToList();
            return areaList.ToList();
        }
        public async Task<bool> SaveDemandOrderTransactionAsync(DemandOrderTransactionVm doTransactionVm)
        {
            using (var db = new PPSDbContext())
            {
                var demandOrder = await db.DemandOrder.FirstOrDefaultAsync(x => x.Id == doTransactionVm.DemandOrderId);
                if (demandOrder == null)
                {
                    throw new Exception("This demand order not found.");
                }
                var totalPaid = demandOrder.DemandOrderTransaction.Sum(x => x.TransactionAmount);
                var totalDoAmount = demandOrder.TotalGrandAmount;
                int paymentStatusId = (int)PaymentStatusEnum.Unpaid;
                if (totalPaid + doTransactionVm.TransactionAmount == totalDoAmount)
                {
                    paymentStatusId = (int)PaymentStatusEnum.Paid;
                    demandOrder.PaymentCompleteDate = DateTime.Now;
                }
                else
                {
                    paymentStatusId = (int)PaymentStatusEnum.PartiallyPaid;
                }

                demandOrder.PaymentStatusId = paymentStatusId;

                var doTran = new DemandOrderTransaction
                {
                    DemandOrderId = doTransactionVm.DemandOrderId,
                    TransactionAmount = doTransactionVm.TransactionAmount,
                    TransactionDate = doTransactionVm.TransactionDate,
                    CreatedBy = doTransactionVm.CreatedBy,
                    CreatedOn = doTransactionVm.CreatedOn
                };
                db.DemandOrderTransaction.Add(doTran);

                db.DemandOrder.Attach(demandOrder);
                db.Entry(demandOrder).State = EntityState.Modified;

                await db.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> SaveInvoiceTransaction(InvoiceTransactionVm invoiceTransactionVm)
        {
            using (var db = new PPSDbContext())
            {
                var invoice = await db.Invoice.FirstOrDefaultAsync(x => x.Id == invoiceTransactionVm.InvoiceId);
                if (invoice == null)
                {
                    throw new Exception("This demand order not found.");
                }
                var totalPaid = invoice.InvoiceTransaction.Sum(x => x.TransactionAmount);
                var totalInvoiceAmount = invoice.TotalGrandAmount;
                //int paymentStatusId = (int)PaymentStatusEnum.Unpaid;
                //if (totalPaid + invoiceTransactionVm.TransactionAmount == totalInvoiceAmount)
                //{
                //    paymentStatusId = (int)PaymentStatusEnum.Paid;

                //}
                //else
                //{
                //    paymentStatusId = (int)PaymentStatusEnum.PartiallyPaid;
                //}

                var invoiceTran = new InvoiceTransaction
                {
                    InvoiceId = invoiceTransactionVm.InvoiceId,
                    TransactionAmount = invoiceTransactionVm.TransactionAmount,
                    TransactionDate = invoiceTransactionVm.TransactionDate,
                    CreatedBy = invoiceTransactionVm.CreatedBy,
                    CreatedOn = invoiceTransactionVm.CreatedOn
                };
                db.InvoiceTransaction.Add(invoiceTran);

                db.Invoice.Attach(invoice);
                db.Entry(invoice).State = EntityState.Modified;

                await db.SaveChangesAsync();
                return true;
            }
        }
        public IList<InvoiceVm> GetInvoiceList(int userId)
        {
            var invoiceList = _ppsDbContext.Invoice.ToList();
            var invoiceVm = new ConcurrentBag<InvoiceVm>();
            invoiceList.ForEach(i =>
            {
                var statusName = "Pending";
                if (i.ApprovedBy != null && i.DeliveredBy == null)
                {
                    statusName = "Approved";
                }
                else if (i.DeliveredBy != null)
                {
                    statusName = "Delivered";
                }

                invoiceVm.Add(new InvoiceVm
                {
                    Id = i.Id,
                    InvoiceNo = i.InvoiceNo,
                    DemandOrderId = i.DemandOrderId,
                    DemandOrderNo = i.DemandOrder.DemandOrderNo,
                    CustomerName = i.DemandOrder.Customer.CustomerName,
                    InvoiceDate = i.InvoiceDate,
                    TotalGrandAmount = i.TotalGrandAmount,
                    Note = i.Note,
                    CreatedByName = GetUserName(i.CreatedBy),
                    InvoiceStatusName = statusName,
                    ApprovedBy = i.ApprovedBy,
                    DeliveredBy = i.DeliveredBy
                });
            });
            return invoiceVm.ToList();
        }
        public List<DemandOrderVm> GetDemandOrderIdListForInvoice(int userId)
        {
            var demandOrderList = _ppsDbContext.DemandOrder.Where(x => x.DemandOrderStatusId == (int)DemandOrderStatusEnum.Approved && x.IsInvoiceCompleted != true)
            .Select(d => new DemandOrderVm
            {
                Id = d.Id,
                DemandOrderNo = d.DemandOrderNo
            }).ToList();
            return demandOrderList;
        }
        public DemandOrderVm GetDemandOrderByIdFromInvoice(int userId, int doId, int invoiceId)
        {
            var demandOrder = _ppsDbContext.DemandOrder.FirstOrDefault(x => x.Id == doId);
            if (demandOrder == null)
            {
                throw new KeyNotFoundException($"This demand order {doId} doesn't exist.");
            }

            var demandOrderDetail = demandOrder.DemandOrderDetail.ToList();

            var invoiceList = demandOrder.Invoice.Where(x => x.DemandOrderId == doId).ToList();

            var invoiceDetail = invoiceList.SelectMany(x => x.InvoiceDetail).ToList();

            var productGroup = invoiceDetail.GroupBy(x => x.Product).Select(g => new { Product = g.Key, Detail = g.ToList() });

            var invoiceDetailVm = new ConcurrentBag<InvoiceDetailVm>();

            productGroup.ToList().ForEach(p =>
            {
                var preAllocQty = 0;
                var allocQty = 0;
                if (invoiceId == -1)
                {
                    preAllocQty = p.Detail.Where(x => x.Invoice.DeliveredBy == null).Sum(g => g.Quantity);
                    allocQty = p.Detail.Where(x => x.Invoice.DeliveredBy == null).Sum(g => g.Quantity);
                }
                else
                {
                    preAllocQty = p.Detail.Where(x => x.Invoice.Id != invoiceId && x.Invoice.DeliveredBy == null).Sum(g => g.Quantity);
                    allocQty = p.Detail.Where(x => x.Invoice.Id == invoiceId && x.Invoice.DeliveredBy == null).Sum(g => g.Quantity);
                }
                invoiceDetailVm.Add(new InvoiceDetailVm
                {
                    ProductId = p.Product.Id,
                    ProductCode = p.Product.Code,
                    ProductName = p.Product.Name + " (" + p.Product?.Color + ")",
                    PreAllocatedQuantity = preAllocQty,
                    AllocatedQuantity = allocQty,
                    DeliveredQuantity = p.Detail.Where(x => x.Invoice.DeliveredBy != null).ToList().Sum(x => x.Quantity)
                });
            });

            var invoiceDetailVmList = invoiceDetailVm.ToList();

            var demandOrderDetailVm = new ConcurrentBag<DemandOrderDetailVm>();
            demandOrderDetail.ForEach(d =>
            {
                var approvedQuantity = d.Quantity;
                var preAllocatedQuantity = invoiceDetailVmList.FirstOrDefault(x => x.ProductId == d.ProductId)?.PreAllocatedQuantity ?? 0;
                var allocatedQuantity = invoiceDetailVmList.FirstOrDefault(x => x.ProductId == d.ProductId)?.AllocatedQuantity ?? 0;
                var deliveredQuantity = invoiceDetailVmList.FirstOrDefault(x => x.ProductId == d.ProductId)?.DeliveredQuantity ?? 0;

                var availableQuantity = approvedQuantity - preAllocatedQuantity - allocatedQuantity - deliveredQuantity;

                demandOrderDetailVm.Add(new DemandOrderDetailVm
                {
                    Id = d.Id,
                    ProductId = d.ProductId,
                    ProductCode = d.Product?.Code,
                    ProductName = d.Product?.Name + " (" + d.Product?.Color + ")",
                    Quantity = approvedQuantity,
                    PreAllocatedQuantity = preAllocatedQuantity,
                    AllocatedQuantity = allocatedQuantity,
                    DeliveredQuantity = deliveredQuantity,
                    AvailableQuantity = availableQuantity,
                    UnitPrice = d.UnitPrice,
                    Discount = d.Discount ?? 0,
                    TotalPrice = d.TotalPrice
                });
            });

            var demandOrderVm = new DemandOrderVm
            {
                Id = demandOrder.Id,
                DemandOrderNo = demandOrder.DemandOrderNo,
                EmployeeId = demandOrder.EmployeeId,
                EmployeeName = demandOrder.Employee.FirstName + " " + demandOrder.Employee.LastName + " (" + demandOrder.Employee.Designation.DesignationName + ")",
                EmployeeCode = demandOrder.Employee.EmployeeCode,
                CustomerId = demandOrder.CustomerId,
                CustomerName = demandOrder.Customer.CustomerName,
                CustomerCode = demandOrder.Customer.CustomerCode,
                CustomerAddress = demandOrder.Customer.CustomerAddress,
                CustomerMobile = demandOrder.Customer.CustomerMobile,
                ReferenceNo = demandOrder.ReferenceDONo,
                TotalAmount = demandOrder.TotalAmount,
                RegularDiscountInPercentage = demandOrder.RegularDiscountInPercentage,
                RegularDiscountAmount = demandOrder.RegularDiscountAmount,
                SpecialDiscountInPercentage = demandOrder.SpecialDiscountInPercentage,
                SpecialDiscountAmount = demandOrder.SpecialDiscountAmount,
                AdditionalDiscountInPercentage = demandOrder.AdditionalDiscountInPercentage,
                AdditionalDiscountAmount = demandOrder.AdditionalDiscountAmount,
                ExtraDiscountInPercentage = demandOrder.ExtraDiscountInPercentage,
                ExtraDiscountAmount = demandOrder.ExtraDiscountAmount,
                CashBackAmount = demandOrder.CashBackAmount,
                TotalDiscountAmount = demandOrder.TotalDiscountAmount,
                TotalGrandAmount = demandOrder.TotalGrandAmount,
                DemandOrderDetail = demandOrderDetailVm.ToList(),
            };

            return demandOrderVm;
        }
        public InvoiceVm SaveInvoice(InvoiceVm invoiceVm)
        {
            var dOQuantity = 0;
            var deliveryInvQuantity = 0;
            var newAllocatedQty = 0;
            var totalInvQuantity = 0;

            lock (invLock)
            {
                invoiceVm.InvoiceDetail.ForEach(x =>
                {
                    dOQuantity += _ppsDbContext.DemandOrderDetail.Where(m => m.DemandOrderId == invoiceVm.DemandOrderId && m.ProductId == x.ProductId).Sum(m => m.Quantity);
                    deliveryInvQuantity += _ppsDbContext.Invoice.Where(m => m.DemandOrderId == invoiceVm.DemandOrderId)
                       .SelectMany(m => m.InvoiceDetail.Where(y => y.ProductId == x.ProductId)).Select(l => l.Quantity).DefaultIfEmpty(0).Sum();
                    newAllocatedQty += invoiceVm.InvoiceDetail.Where(k => k.ProductId == x.ProductId).Sum(k => k.NewAllocatedQuantity);
                    totalInvQuantity = deliveryInvQuantity + newAllocatedQty;

                    if (totalInvQuantity > dOQuantity)
                    {
                        throw new FormatException("Total invoice quantity in Bigger than demand order Quantity");
                    }
                });

                var lastInvoice = _ppsDbContext.Invoice.OrderByDescending(x => x.InvoiceNo).FirstOrDefault();
                var lastNumber = 0;
                if (lastInvoice != null)
                {
                    lastNumber = lastInvoice.InvoiceNo;
                }
                invoiceVm.InvoiceNo = lastNumber + 1;

                var invoiceEntry = new Invoice
                {
                    InvoiceNo = invoiceVm.InvoiceNo,
                    InvoiceDate = invoiceVm.InvoiceDate,
                    DemandOrderId = invoiceVm.DemandOrderId,
                    Note = invoiceVm.Note,
                    TotalAmount = invoiceVm.TotalAmount,
                    RegularDiscountInPercentage = invoiceVm.RegularDiscountInPercentage,
                    RegularDiscountAmount = invoiceVm.RegularDiscountAmount,
                    SpecialDiscountInPercentage = invoiceVm.SpecialDiscountInPercentage,
                    SpecialDiscountAmount = invoiceVm.SpecialDiscountAmount,
                    AdditionalDiscountInPercentage = invoiceVm.AdditionalDiscountInPercentage,
                    AdditionalDiscountAmount = invoiceVm.AdditionalDiscountAmount,
                    ExtraDiscountInPercentage = invoiceVm.ExtraDiscountInPercentage,
                    ExtraDiscountAmount = invoiceVm.ExtraDiscountAmount,
                    CashBackAmount = invoiceVm.CashBackAmount,
                    TotalDiscountAmount = invoiceVm.TotalDiscountAmount,
                    TotalGrandAmount = invoiceVm.TotalGrandAmount,
                    CreatedBy = invoiceVm.CreatedBy,
                    CreatedOn = invoiceVm.CreatedOn
                };

                foreach (var tempInvoiceEntry in invoiceVm.InvoiceDetail)
                {
                    invoiceEntry.InvoiceDetail.Add(
                        new InvoiceDetail
                        {
                            InvoiceId = invoiceEntry.Id,
                            ProductId = tempInvoiceEntry.ProductId,
                            Quantity = tempInvoiceEntry.NewAllocatedQuantity,
                            TotalAmount = (decimal)(tempInvoiceEntry.NewAllocatedQuantity * tempInvoiceEntry.UnitPrice)
                        });
                }

                _ppsDbContext.Invoice.Add(invoiceEntry);

                var doTotalQty = _ppsDbContext.DemandOrder.FirstOrDefault(x => x.Id == invoiceVm.DemandOrderId).DemandOrderDetail.Sum(x => x.Quantity);
                var invList = _ppsDbContext.Invoice.Where(x => x.DemandOrderId == invoiceVm.DemandOrderId);
                var existInvTotalQty = invList.Count() != 0 ? invList.SelectMany(k => k.InvoiceDetail).Sum(n => n.Quantity) : 0;
                var newInvQty = invoiceVm.InvoiceDetail.Sum(x => x.Quantity);
                var totalInvQty = existInvTotalQty + newInvQty;

                if (doTotalQty == totalInvQty)
                {
                    var currentDemandOrder = _ppsDbContext.DemandOrder.FirstOrDefault(x => x.Id == invoiceVm.DemandOrderId);
                    currentDemandOrder.IsInvoiceCompleted = true;
                    _ppsDbContext.DemandOrder.Attach(currentDemandOrder);
                    _ppsDbContext.Entry(currentDemandOrder).State = EntityState.Modified;
                }
                _ppsDbContext.SaveChanges();

                invoiceVm.Id = invoiceEntry.Id;

                return invoiceVm;
            };
        }
        public InvoiceVm UpdateInvoice(InvoiceVm invoiceVm)
        {
            using (var db = new PPSDbContext())
            {
                var dOQuantity = 0;
                var deliveryInvQuantity = 0;
                var newAllocatedQty = 0;
                var totalInvQuantity = 0;

                lock (invLock)
                {
                    var invoiceEntry = db.Invoice.FirstOrDefault(x => x.Id == invoiceVm.Id);

                    if (invoiceEntry == null)
                    {
                        throw new KeyNotFoundException($"Invoice No: {invoiceVm.Id} does not exist.");
                    }
                    if (invoiceEntry.ApprovedBy != null)
                    {
                        throw new Exception($"Invoice No: { invoiceVm.Id } has already been approved.");
                    }
                    if (invoiceEntry.DeliveredBy != null)
                    {
                        throw new Exception($"Invoice No: { invoiceVm.Id } has already been delivered.");
                    }

                    invoiceVm.InvoiceDetail.ForEach(x =>
                    {
                        dOQuantity += _ppsDbContext.DemandOrderDetail.Where(m => m.DemandOrderId == invoiceVm.DemandOrderId && m.ProductId == x.ProductId).Sum(m => m.Quantity);
                        deliveryInvQuantity += _ppsDbContext.Invoice.Where(m => m.DemandOrderId == invoiceVm.DemandOrderId && m.Id != invoiceVm.Id)
                           .SelectMany(m => m.InvoiceDetail.Where(y => y.ProductId == x.ProductId)).Select(l => l.Quantity).DefaultIfEmpty(0).Sum();
                        newAllocatedQty += invoiceVm.InvoiceDetail.Where(k => k.ProductId == x.ProductId).Sum(k => k.NewAllocatedQuantity);
                        totalInvQuantity = deliveryInvQuantity + newAllocatedQty;

                        if (totalInvQuantity > dOQuantity)
                        {
                            throw new FormatException("Total invoice quantity in Bigger than demand order Quantity");
                        }
                    });

                    var hasRegularDiscount = WebConfigurationManager.AppSettings["RegularDiscount"] ?? "false";
                    var hasSpecialDiscount = WebConfigurationManager.AppSettings["SpecialDiscount"] ?? "false";
                    var hasAdditionalDiscount = WebConfigurationManager.AppSettings["AdditionalDiscount"] ?? "false";
                    var hasExtraDiscount = WebConfigurationManager.AppSettings["ExtraDiscount"] ?? "false";
                    var hasCashBack = WebConfigurationManager.AppSettings["CashBack"] ?? "false";

                    invoiceEntry.InvoiceDate = invoiceVm.InvoiceDate;
                    invoiceEntry.Note = invoiceVm.Note;
                    invoiceEntry.TotalAmount = invoiceVm.TotalAmount;

                    invoiceEntry.RegularDiscountInPercentage = string.Compare(hasRegularDiscount, "true", StringComparison.OrdinalIgnoreCase) == 0 ? invoiceVm.RegularDiscountInPercentage : null;
                    invoiceEntry.RegularDiscountAmount = string.Compare(hasRegularDiscount, "true", StringComparison.OrdinalIgnoreCase) == 0 ? invoiceVm.RegularDiscountAmount : null;
                    invoiceEntry.SpecialDiscountInPercentage = string.Compare(hasSpecialDiscount, "true", StringComparison.OrdinalIgnoreCase) == 0 ? invoiceVm.SpecialDiscountInPercentage : null;
                    invoiceEntry.SpecialDiscountAmount = string.Compare(hasSpecialDiscount, "true", StringComparison.OrdinalIgnoreCase) == 0 ? invoiceVm.SpecialDiscountAmount : null;
                    invoiceEntry.AdditionalDiscountInPercentage = string.Compare(hasAdditionalDiscount, "true", StringComparison.OrdinalIgnoreCase) == 0 ? invoiceVm.AdditionalDiscountInPercentage : null;
                    invoiceEntry.AdditionalDiscountAmount = string.Compare(hasAdditionalDiscount, "true", StringComparison.OrdinalIgnoreCase) == 0 ? invoiceVm.AdditionalDiscountAmount : null;
                    invoiceEntry.ExtraDiscountInPercentage = string.Compare(hasExtraDiscount, "true", StringComparison.OrdinalIgnoreCase) == 0 ? invoiceVm.ExtraDiscountInPercentage : null;
                    invoiceEntry.ExtraDiscountAmount = string.Compare(hasExtraDiscount, "true", StringComparison.OrdinalIgnoreCase) == 0 ? invoiceVm.ExtraDiscountAmount : null;
                    invoiceEntry.CashBackAmount = string.Compare(hasCashBack, "true", StringComparison.OrdinalIgnoreCase) == 0 ? invoiceVm.CashBackAmount : null;

                    invoiceEntry.TotalDiscountAmount = invoiceVm.TotalDiscountAmount;
                    invoiceEntry.TotalGrandAmount = invoiceVm.TotalGrandAmount;
                    invoiceEntry.UpdatedBy = invoiceVm.UpdatedBy;
                    invoiceEntry.UpdatedOn = invoiceVm.UpdatedOn;

                    var invoiceEntryDetail = invoiceEntry.InvoiceDetail;
                    invoiceEntry.InvoiceDetail = new List<InvoiceDetail>();

                    foreach (var invDetail in invoiceVm.InvoiceDetail)
                    {
                        var unitPrice = _ppsDbContext.Product.FirstOrDefault(x => x.Id == invDetail.ProductId)?.UnitPrice ?? 0;
                        invoiceEntry.InvoiceDetail.Add(
                                new InvoiceDetail
                                {
                                    InvoiceId = invoiceVm.Id,
                                    ProductId = invDetail.ProductId,
                                    Quantity = invDetail.AllocatedQuantity,
                                    TotalAmount = (decimal)(invDetail.UnitPrice * invDetail.AllocatedQuantity)
                                });

                    }

                    var doTotalQty = _ppsDbContext.DemandOrder.FirstOrDefault(x => x.Id == invoiceVm.DemandOrderId).DemandOrderDetail.Sum(x => x.Quantity);
                    var existInvTotalQty = _ppsDbContext.Invoice.Where(x => x.DemandOrderId == invoiceVm.DemandOrderId && x.Id != invoiceVm.Id).ToList()?.SelectMany(k => k.InvoiceDetail).Sum(m => m.Quantity);
                    var newInvQty = invoiceVm.InvoiceDetail.Sum(x => x.Quantity);
                    var totalInvQty = existInvTotalQty + newInvQty;

                    var currentDemandOrder = _ppsDbContext.DemandOrder.FirstOrDefault(x => x.Id == invoiceVm.DemandOrderId);
                    if (doTotalQty == totalInvQty)
                    {
                        currentDemandOrder.IsInvoiceCompleted = true;
                    }
                    else
                    {
                        currentDemandOrder.IsInvoiceCompleted = false;
                    }
                    _ppsDbContext.DemandOrder.Attach(currentDemandOrder);
                    _ppsDbContext.Entry(currentDemandOrder).State = EntityState.Modified;

                    db.InvoiceDetail.RemoveRange(invoiceEntryDetail);
                    db.Invoice.Attach(invoiceEntry);
                    db.Entry(invoiceEntry).State = EntityState.Modified;
                    db.SaveChanges();
                    return invoiceVm;
                }
            };
        }
        public InvoiceVm GetInvoiceById(int userId, int invoiceId)
        {
            var invoice = _ppsDbContext.Invoice.FirstOrDefault(x => x.Id == invoiceId);
            if (invoice == null)
            {
                throw new KeyNotFoundException($"This invoice no. {invoiceId} doesn't exist.");
            }
            //var invoiceList = _ppsDbContext.Invoice.Where(x => x.DemandOrderId == invoice.DemandOrderId).ToList();

            //added by hasan


            var totalDoList = _ppsDbContext.DemandOrder.Where(m => m.CustomerId == invoice.DemandOrder.CustomerId).Select(m => m.Id).ToList();
            var totalInvoiceAmount = _ppsDbContext.Invoice.Where(m => totalDoList.Contains(m.DemandOrderId)).Select(m => m.TotalGrandAmount).ToList().Sum();

            var totalInvoiceTransactionAmmount = _ppsDbContext.Invoice.Where(m => m.DemandOrder.CustomerId == invoice.DemandOrder.CustomerId).SelectMany(m => m.InvoiceTransaction.Where(x => x.InvoiceId == x.Invoice.Id)).Any() ? _ppsDbContext.Invoice.Where(m => m.DemandOrder.CustomerId == invoice.DemandOrder.CustomerId).SelectMany(m => m.InvoiceTransaction.Where(x => x.InvoiceId == x.Invoice.Id)).ToList().Sum(x => x.TransactionAmount) : 0;

            var customerTotalBalance = _ppsDbContext.CustomerTransaction.Where(x => x.IsApproved == true)
             .SelectMany(x => x.CustomerTransactionDetail.Where(y => y.CustomerId == invoice.DemandOrder.CustomerId).Select(k => k.TransactionAmount)).ToList().Sum();



            var totalInvoiceBalance = totalInvoiceAmount - totalInvoiceTransactionAmmount;
            var customerRemainingBalance = customerTotalBalance - totalInvoiceTransactionAmmount;
            //end
            //var invoices = invoiceList.SelectMany(x => x.InvoiceDetail).ToList();

            //var productGroup = invoices.GroupBy(x => x.Product).Select(g => new { Product = g.Key, Detail = g.ToList() });

            var invoiceDetailVm1 = new ConcurrentBag<InvoiceDetailVm>();
            //if (invoice.DeliveredBy != null)
            //{
            foreach (var item in invoice.InvoiceDetail)
            {
                var invList = _ppsDbContext.Invoice.Where(x => x.DemandOrderId == invoice.DemandOrderId && x.DeliveredBy != null).ToList();
                var totalDelQty = invList.Count() != 0 ? invList.SelectMany(y => y.InvoiceDetail.Where(k => k.ProductId == item.ProductId)).Sum(i => i.Quantity) : 0;
                var totalAllocQty = _ppsDbContext.Invoice.Where(x => x.DemandOrderId == invoice.DemandOrderId && x.Id != invoiceId && x.DeliveredBy == null).ToList()?.SelectMany(y => y.InvoiceDetail.Where(k => k.ProductId == item.ProductId)).Sum(i => i.Quantity) ?? 0;
                invoiceDetailVm1.Add(new InvoiceDetailVm
                {
                    ProductId = item.Product.Id,
                    ProductCode = item.Product.Code,
                    ProductName = item.Product.Name + " (" + item.Product.Color + ")",
                    DeliveredQuantity = totalDelQty,
                    AllocatedQuantity = totalAllocQty
                });
            };
            //}
            //else
            //{
            //    foreach (var item in invoice.InvoiceDetail)
            //    {
            //        invoiceDetailVm1.Add(new InvoiceDetailVm
            //        {
            //            ProductId = item.Product.Id,
            //            ProductCode = item.Product.Code,
            //            ProductName = item.Product.Name + " (" + item.Product.Color + ")",
            //            AllocatedQuantity = item.Quantity
            //        });
            //    };
            //}

            var invoiceDetailVmList = invoiceDetailVm1.ToList();

            var invoiceDetail = invoice.InvoiceDetail.ToList();
            var invoiceDetailVm = new ConcurrentBag<InvoiceDetailVm>();

            var demandOrder = _ppsDbContext.DemandOrder.FirstOrDefault(x => x.Id == invoice.DemandOrderId);
            var demandOrderDetail = demandOrder?.DemandOrderDetail.ToList();

            var regularDiscountInPercentage = demandOrder?.RegularDiscountInPercentage ?? 0;
            var regularDiscountAmount = demandOrder?.RegularDiscountAmount ?? 0;
            var specialDiscountInPercentage = demandOrder?.SpecialDiscountInPercentage ?? 0;
            var specialrDiscountAmount = demandOrder?.SpecialDiscountAmount ?? 0;
            var additionalDiscountInPercentage = demandOrder?.AdditionalDiscountInPercentage ?? 0;
            var additionalDiscountAmount = demandOrder?.AdditionalDiscountAmount ?? 0;
            var extraDiscountInPercentage = demandOrder?.ExtraDiscountInPercentage ?? 0;
            var extraDiscountAmount = demandOrder?.ExtraDiscountAmount ?? 0;
            var cashBackAmount = demandOrder?.CashBackAmount ?? 0;

            var demandOrderDetailVm = new ConcurrentBag<DemandOrderDetailVm>();

            demandOrderDetail?.ForEach(d =>
            {
                demandOrderDetailVm.Add(new DemandOrderDetailVm
                {
                    Id = d.Id,
                    ProductId = d.ProductId,
                    ProductCode = d.Product?.Code,
                    ProductName = d.Product?.Name + " (" + d.Product?.Color + ")",
                    Quantity = d.Quantity,
                    UnitPrice = d.UnitPrice,
                    Discount = d.Discount ?? 0,
                    TotalPrice = d.TotalPrice
                });
            });

            invoiceDetail.ForEach(d =>
            {
                double totalPrice;
                var approvedQuantity = demandOrderDetailVm.FirstOrDefault(x => x.ProductId == d.ProductId)?.Quantity ?? 0;

                var preAllocatedQuantity = invoiceDetailVmList.FirstOrDefault(x => x.ProductId == d.ProductId)?.AllocatedQuantity ?? 0;
                var deliveredQuantity = invoiceDetailVmList.FirstOrDefault(x => x.ProductId == d.ProductId)?.DeliveredQuantity ?? 0;

                var unitPrice = demandOrderDetailVm.FirstOrDefault(x => x.ProductId == d.ProductId)?.UnitPrice ?? 0;
                var discount = demandOrderDetailVm.FirstOrDefault(x => x.ProductId == d.ProductId)?.Discount ?? 0;
                if (discount > 0)
                {
                    totalPrice = unitPrice * d.Quantity * (1 - discount / 100);
                }
                else
                {
                    totalPrice = unitPrice * d.Quantity;
                }
                var allocatedQuantity = d.Quantity;
                var availableQuantity = approvedQuantity - preAllocatedQuantity - deliveredQuantity;

                invoiceDetailVm.Add(new InvoiceDetailVm
                {
                    Id = d.Id,
                    InvoiceId = d.InvoiceId,
                    ProductId = d.ProductId,
                    ProductCode = d.Product?.Code,
                    ProductName = d.Product?.Name + " (" + d.Product?.Color + ")",
                    Quantity = approvedQuantity,
                    PreAllocatedQuantity = preAllocatedQuantity,
                    AllocatedQuantity = allocatedQuantity,
                    DeliveredQuantity = deliveredQuantity,
                    AvailableQuantity = availableQuantity,
                    UnitPrice = unitPrice,
                    Discount = discount,
                    TotalPrice = totalPrice
                });
            });

            var statusName = "Pending";

            if (invoice.ApprovedBy != null && invoice.DeliveredBy == null)
            {
                statusName = "Approved";
            }
            else if (invoice.DeliveredBy != null)
            {
                statusName = "Delivered";
            }
            var invoiceTransactionVm = new ConcurrentBag<InvoiceTransactionVm>();
            var totalPaid = 0D;
            invoice.InvoiceTransaction.ToList().ForEach(tran =>
            {
                totalPaid += tran.TransactionAmount;
                invoiceTransactionVm.Add(new InvoiceTransactionVm
                {
                    InvoiceId = tran.InvoiceId,
                    CreatedByName = GetUserName(tran.CreatedBy),
                    CreatedOn = tran.CreatedOn,
                    TransactionAmount = tran.TransactionAmount,
                    TransactionDate = tran.TransactionDate
                });
            });
            var totalDue = invoice.TotalGrandAmount - totalPaid;
            //TODO Maintain Status Flag
            var invoiceVm = new InvoiceVm
            {
                Id = invoice.Id,
                InvoiceNo = invoice.InvoiceNo,
                InvoiceDate = invoice.InvoiceDate,
                DemandOrderId = invoice.DemandOrderId,
                //DemandOrderNo = _ppsDbContext.DemandOrder.Where(m=>m.Id==invoice.DemandOrderId).Select(d=>d.DemandOrderNo).FirstOrDefault(),
                DemandOrderNo = demandOrder.DemandOrderNo,
                DONote = demandOrder.Note,
                CustomerId = demandOrder.CustomerId,
                CustomerCode = demandOrder.Customer.CustomerCode,
                CustomerName = demandOrder.Customer.CustomerName,
                CustomerAddress = demandOrder.Customer.CustomerAddress,
                CustomerMobile = demandOrder.Customer.CustomerMobile,
                InvoiceStatusName = statusName,
                InvoiceDetail = invoiceDetailVm.ToList(),
                Note = invoice.Note,
                TotalAmount = invoice.TotalAmount,
                RegularDiscountInPercentage = invoice.RegularDiscountInPercentage,
                RegularDiscountAmount = invoice.RegularDiscountAmount,
                SpecialDiscountInPercentage = invoice.SpecialDiscountInPercentage,
                SpecialDiscountAmount = invoice.SpecialDiscountAmount,
                AdditionalDiscountInPercentage = invoice.AdditionalDiscountInPercentage,
                AdditionalDiscountAmount = invoice.AdditionalDiscountAmount,
                ExtraDiscountInPercentage = invoice.ExtraDiscountInPercentage,
                ExtraDiscountAmount = invoice.ExtraDiscountAmount,
                CashBackAmount = invoice.CashBackAmount,
                TotalDiscountAmount = invoice.TotalDiscountAmount,
                TotalGrandAmount = invoice.TotalGrandAmount,
                TotalPaidAmount = totalPaid,
                TotalDueAmount = totalDue,
                InvoiceTransaction = invoiceTransactionVm.ToList(),
                TotalInvoiceBalanceAmount = totalInvoiceBalance > 0 ? totalInvoiceBalance : 0.0,
                CustomerRemainingBalance = customerRemainingBalance > 0 ? customerRemainingBalance : 0,
                ApprovedBy = invoice.ApprovedBy,
                DeliveredBy = invoice.DeliveredBy,
                CreatedByName = GetUserByOn(invoice.CreatedBy, invoice.CreatedOn),
                ApprovedByName = invoice.ApprovedBy != null && invoice.ApprovedOn != null
                    ? GetUserByOn((int)invoice.ApprovedBy, (DateTime)invoice.ApprovedOn)
                    : "-"
            };
            return invoiceVm;
        }
        public async Task<bool> ApproveInvoice(InvoiceRequestVm invoiceRequestVm)
        {
            using (var db = new PPSDbContext())
            {
                var invoice = db.Invoice.FirstOrDefault(x => x.Id == invoiceRequestVm.InvoiceId);
                if (invoice == null)
                {
                    throw new KeyNotFoundException($"This invoice no. {invoiceRequestVm.InvoiceId} doesn't exist.");
                }
                if (invoice.ApprovedBy != null)
                {
                    throw new Exception($"The invoice no: {invoiceRequestVm.InvoiceId} has already been approved.");
                }
                var customerAccountHeadId = invoice.DemandOrder.Customer.AccountHeadId;
                if (customerAccountHeadId == null)
                {
                    throw new KeyNotFoundException("Customer account head doesn't exist.");
                }

                var referenceTable = await db.ReferenceTable.ToListAsync();

                var sysTranDetailList = new List<TransactionDetail>();

                var regularDiscountAccountHeadId = "";

                if (invoice.RegularDiscountAmount != null && invoice.RegularDiscountAmount > 0)
                {
                    regularDiscountAccountHeadId = referenceTable.FirstOrDefault(x => x.ReferenceName == ReferenceTableEnum.RegularDiscountAccountHeadId.ToString())?.ReferenceValue;
                    if (regularDiscountAccountHeadId == null)
                    {
                        throw new KeyNotFoundException("Regular discount account head doesn't exist.");
                    }
                    sysTranDetailList.Add(new TransactionDetail
                    {
                        AccountHeadId = Convert.ToInt32(regularDiscountAccountHeadId),
                        CrAmount = 0,
                        DrAmount = invoice.RegularDiscountAmount ?? 0
                    });
                }

                var specialDiscountAccountHeadId = "";
                if (invoice.SpecialDiscountAmount != null && invoice.SpecialDiscountAmount > 0)
                {
                    specialDiscountAccountHeadId = referenceTable.FirstOrDefault(x => x.ReferenceName == ReferenceTableEnum.SpecialDiscountAccountHeadId.ToString())?.ReferenceValue;
                    if (specialDiscountAccountHeadId == null)
                    {
                        throw new KeyNotFoundException("Special discount account head doesn't exist.");
                    }
                    sysTranDetailList.Add(new TransactionDetail
                    {
                        AccountHeadId = Convert.ToInt32(specialDiscountAccountHeadId),
                        CrAmount = 0,
                        DrAmount = invoice.SpecialDiscountAmount ?? 0
                    });
                }

                var additionalDiscountAccountHeadId = "";
                if (invoice.AdditionalDiscountAmount != null && invoice.AdditionalDiscountAmount > 0)
                {
                    additionalDiscountAccountHeadId = referenceTable.FirstOrDefault(x => x.ReferenceName == ReferenceTableEnum.AdditionalDiscountAccountHeadId.ToString())?.ReferenceValue;
                    if (additionalDiscountAccountHeadId == null)
                    {
                        throw new KeyNotFoundException("Additional discount account head doesn't exist.");
                    }
                    sysTranDetailList.Add(new TransactionDetail
                    {
                        AccountHeadId = Convert.ToInt32(additionalDiscountAccountHeadId),
                        CrAmount = 0,
                        DrAmount = invoice.AdditionalDiscountAmount ?? 0
                    });
                }

                var extraDiscountAccountHeadId = "";

                if (invoice.ExtraDiscountAmount != null && invoice.ExtraDiscountAmount > 0)
                {
                    extraDiscountAccountHeadId = referenceTable.FirstOrDefault(x => x.ReferenceName == ReferenceTableEnum.ExtraDiscountAccountHeadId.ToString())?.ReferenceValue;
                    if (extraDiscountAccountHeadId == null)
                    {
                        throw new KeyNotFoundException("Extra discount account head doesn't exist.");
                    }
                    sysTranDetailList.Add(new TransactionDetail
                    {
                        AccountHeadId = Convert.ToInt32(extraDiscountAccountHeadId),
                        CrAmount = 0,
                        DrAmount = invoice.ExtraDiscountAmount ?? 0
                    });
                }

                var cashBackAccountHeadId = "";
                if (invoice.CashBackAmount != null && invoice.CashBackAmount > 0)
                {
                    cashBackAccountHeadId = referenceTable.FirstOrDefault(x => x.ReferenceName == ReferenceTableEnum.CashBackAccountHeadId.ToString())?.ReferenceValue;
                    if (cashBackAccountHeadId == null)
                    {
                        throw new KeyNotFoundException("Cash back account head doesn't exist.");
                    }
                    sysTranDetailList.Add(new TransactionDetail
                    {
                        AccountHeadId = Convert.ToInt32(cashBackAccountHeadId),
                        CrAmount = 0,
                        DrAmount = invoice.CashBackAmount ?? 0
                    });
                }

                invoice.ApprovedBy = invoiceRequestVm.UserId;
                invoice.ApprovedOn = invoiceRequestVm.DatedOn;

                db.Invoice.Attach(invoice);
                db.Entry(invoice).State = EntityState.Modified;

                // Hit the accounting head for this dealer
                var lastTran = _ppsDbContext.TransactionEntry
                .Where(x => x.TransactionTypeId == (int)TransactionTypeEnum.Sales
                            && x.FiscalYear == invoiceRequestVm.FiscalYear
                            && x.CompanyId == invoiceRequestVm.CompanyId)
                .OrderByDescending(x => x.TransactionNumber.Substring(9, 4))
                .FirstOrDefault();

                var lastNumber = 0;
                if (lastTran != null)
                {
                    lastNumber = int.Parse(lastTran.TransactionNumber.Substring(9, 4));
                }
                var transactionNumber = _transactionRepository.CreateTransactionNumber((int)TransactionTypeEnum.Sales, DateTime.Now, lastNumber + 1);

                // Dealer(Dr), Sales Revenue(Cr)
                var systemTransaction = new TransactionEntry
                {
                    IsSystemGenerated = true,
                    TransactionNumber = transactionNumber,
                    TransactionDate = invoiceRequestVm.DatedOn,
                    FiscalYear = invoiceRequestVm.FiscalYear,
                    TransactionTypeId = (int)TransactionTypeEnum.Sales,
                    CompanyId = invoiceRequestVm.CompanyId,
                    PostingDate = invoiceRequestVm.DatedOn,
                    Active = true,
                    Deleted = false,
                    Accepted = false,
                    CreatedById = invoice.CreatedBy,
                    CreatedDate = invoice.CreatedOn,
                    VerifiedById = invoiceRequestVm.UserId,
                    VerifiedDate = invoiceRequestVm.DatedOn,
                    TransactionDetail = sysTranDetailList
                };

                sysTranDetailList.Add(
                    new TransactionDetail
                    {
                        AccountHeadId = (int)customerAccountHeadId,
                        CrAmount = 0,
                        DrAmount = invoice.TotalGrandAmount ?? 0
                    });

                invoice.InvoiceDetail.ToList().ForEach(x =>
                {
                    sysTranDetailList.Add(new TransactionDetail
                    {
                        AccountHeadId = Convert.ToInt32(x.Product?.AccountHeadId),
                        CrAmount = (double)x.TotalAmount,
                        DrAmount = 0
                    });
                });

                var totalDrAmount = systemTransaction.TransactionDetail.Sum(x => x.DrAmount);
                var totalCrAmount = systemTransaction.TransactionDetail.Sum(x => x.CrAmount);
                if (totalDrAmount == 0 || totalDrAmount != totalCrAmount)
                {
                    throw new Exception("The amount of debit and credit is not matched.");
                }

                invoice.TransactionEntry = systemTransaction;

                db.TransactionEntry.Add(systemTransaction);

                await db.SaveChangesAsync();
            }
            return true;
        }
        public async Task<bool> DeliveryInvoice(int invoiceId, int userId)
        {
            var currentStock = new CurrentProductStockRepository();
            using (var db = new PPSDbContext())
            {
                var invoice = db.Invoice.FirstOrDefault(x => x.Id == invoiceId);
                if (invoice == null)
                {
                    throw new KeyNotFoundException($"This invoice no. {invoiceId} doesn't exist.");
                }
                if (invoice.DeliveredBy != null)
                {
                    throw new Exception($"The invoice no: {invoiceId} has already been delivered.");
                }
                invoice.DeliveredBy = userId;
                invoice.DeliveredOn = DateTime.Now;
                db.Invoice.Attach(invoice);
                db.Entry(invoice).State = EntityState.Modified;

                invoice.InvoiceDetail.ToList().ForEach(inv =>
                {
                    var success = currentStock.AddDeliveredQuantityByProductId(db, inv.ProductId, inv.Quantity);
                    if (success == false)
                    {
                        throw new Exception($"Available stock of product Id {inv.ProductId} is overflow");
                    }
                });
                await db.SaveChangesAsync();
            }
            return true;
        }
        public IList<CustomerTransactionHistoryVm> GetCustomerTransactionHistoryByCustomerId(int userId, int doId)
        {
            var demandOrder = _ppsDbContext.DemandOrder.FirstOrDefault(x => x.Id == doId);
            if (demandOrder == null)
            {
                throw new KeyNotFoundException($"This demand order no. {doId} doesn't exist.");
            }
            var customerId = demandOrder.CustomerId;

            var demandOrderListByCustomer = _ppsDbContext.DemandOrder.Where(x => x.CustomerId == customerId && x.ApprovedBy != null).ToList();

            var dOTransaction = _ppsDbContext.DemandOrderTransaction.GroupBy(x => x.DemandOrderId).Select(g =>
                new CustomerTransactionHistoryVm
                {
                    DemandOrderNo = g.FirstOrDefault().DemandOrderId,
                    DOPaidAmount = g.Sum(x => x.TransactionAmount)
                }).ToList();

            var dOInvoiceTransaction = _ppsDbContext.Invoice.GroupBy(x => x.DemandOrderId).Select(g =>
                new CustomerTransactionHistoryVm
                {
                    DemandOrderNo = g.FirstOrDefault().DemandOrderId,
                    DOInvoiceAmount = (double)g.Sum(x => x.TotalGrandAmount)
                }).ToList();

            var customerTransactionHistoryList = new ConcurrentBag<CustomerTransactionHistoryVm>();
            demandOrderListByCustomer.ForEach(d =>
            {
                var dOPaidAmount = dOTransaction.FirstOrDefault(x => x.DemandOrderNo == d.Id)?.DOPaidAmount ?? 0;
                var dOInvoiceAmount = dOInvoiceTransaction.FirstOrDefault(x => x.DemandOrderNo == d.Id)?.DOInvoiceAmount ?? 0;
                customerTransactionHistoryList.Add(new CustomerTransactionHistoryVm
                {
                    DemandOrderNo = d.Id,
                    DemandOrderDate = d.DODate,
                    DOAmount = d.TotalGrandAmount,
                    DOPaidAmount = dOPaidAmount,
                    DOBalanceAmount = d.TotalGrandAmount - dOPaidAmount,
                    DOInvoiceAmount = dOInvoiceAmount,
                    DOInvoiceBalance = dOInvoiceAmount == 0.0 ? 0 : dOPaidAmount - dOInvoiceAmount,
                    SaleType = d.SaleType.SaleTypeName
                });
            });

            return customerTransactionHistoryList.ToList();
        }
        public SalesPersonVm GetSalesPersonHistoryByEmployeeId(int userId, int employeeId, int year)
        {
            var employeeHierarchy = _employeeRepository.GetEmployeeHierarchy(employeeId);
            return CreateSalesPersonHierarchy(employeeHierarchy, year);
        }
        public IList<DemandOrderVm> GetDemandOrderEarlyPaymentList(int userId)
        {
            int paymentStatus = (int)PaymentStatusEnum.Paid;
            var user = _ppsDbContext.User.FirstOrDefault(x => x.Id == userId);
            var employee = _ppsDbContext.Employee.FirstOrDefault(x => x.Id == user.EmployeeId);
            List<DemandOrder> doList;
            if (employee != null && employee.DepartmentId == (int)DepartmentEnum.SalesAndMarketing)
            {
                var employeeRepository = new EmployeeRepository();
                var employeeIdList = employeeRepository.GetManagedEmployee(employee.Id).Select(x => x.Item1);
                doList = _ppsDbContext.DemandOrder.Where(x => x.PaymentStatusId == paymentStatus && employeeIdList.Contains(x.EmployeeId)).ToList();
            }
            else
            {
                doList = _ppsDbContext.DemandOrder.Where(x => x.PaymentStatusId == paymentStatus).ToList();
            }

            var doVm = new ConcurrentBag<DemandOrderVm>();
            doList.ForEach(d =>
            {
                var totalDOAmount = d.TotalGrandAmount;
                var doEarlyPaymentDiscountPercentage = d.SaleType?.EarlyPaymentDiscountInPercentage ?? 0;
                if (doEarlyPaymentDiscountPercentage == 0)
                {
                    throw new Exception("Early payment discount must be greater than 0");
                }
                var discountAmount = Math.Round(totalDOAmount * doEarlyPaymentDiscountPercentage / 100);

                var totalPaid = d.DemandOrderTransaction.ToList().Sum(x => x.TransactionAmount);
                var totalDue = d.TotalGrandAmount - totalPaid;
                int maturityDays;
                if (d.PaymentStatusId == (int)PaymentStatusEnum.Paid && d.PaymentCompleteDate != null)
                {
                    maturityDays = ((DateTime)d.PaymentCompleteDate - d.DODate).Days;
                }
                else
                {
                    maturityDays = (DateTime.Today - d.DODate).Days;
                }
                int maturityLabel = (int)DoMaturityLabelEnum.Normal;

                if (maturityDays > d.SaleType?.DurationInDays)
                {
                    maturityLabel = (int)DoMaturityLabelEnum.OverDue;
                }
                else if (maturityDays > d.SaleType?.WarningInDays)
                {
                    maturityLabel = (int)DoMaturityLabelEnum.Warning;
                }

                var paidDate = d.PaymentCompleteDate;

                int earlyPaymentDays = ((DateTime)paidDate - d.DODate).Days;
                if (earlyPaymentDays <= d.SaleType?.EarlyPaymentInDays)
                {
                    doVm.Add(new DemandOrderVm
                    {
                        Id = d.Id,
                        DODate = d.DODate,
                        SaleTypeName = d.SaleType?.SaleTypeName,
                        DemandOrderTypeName = d.DemandOrderType?.DemandOrderTypeName,
                        MaturityDays = maturityDays,
                        MaturityLabel = maturityLabel,
                        ReferenceNo = d.ReferenceDONo,
                        CustomerName = d.Customer.CustomerName,
                        DOStatusName = d.DemandOrderStatus.Status,
                        TotalGrandAmount = d.TotalGrandAmount,
                        CreatedByName = StringExtension.ToFullName(d.User.FirstName, d.User.LastName),
                        CreatedDate = d.CreatedOn,
                        Submitted = d.SubmittedBy != null,
                        //DOPaymentStatusId = GetDemandOrderTransactionStatus(d, totalDue),
                        DOPaymentStatusId = d.PaymentStatusId,
                        DOPaymentStatus = d.PaymentStatus.PaymentStatusName,
                        DODiscountTransactionStatusName = GetDemandOrderEarlyPaymentTransactionStatus(d.DemandOrderDiscountTransaction.FirstOrDefault(x => x.DemandOrderDiscountTypeId == (int)DemandOrderDiscountTypeEnum.EarlyPayment)),
                        EarlyPaymentDiscountInPercentage = doEarlyPaymentDiscountPercentage,
                        EarlyPaymentDiscountAmount = discountAmount
                    });
                }
            });
            return doVm.ToList();
        }
        public IList<DemandOrderVm> GetDemandOrderEarlyPaymentPendingList(int userId)
        {
            int paymentStatus = (int)PaymentStatusEnum.Unpaid;
            var user = _ppsDbContext.User.FirstOrDefault(x => x.Id == userId);
            var employee = _ppsDbContext.Employee.FirstOrDefault(x => x.Id == user.EmployeeId);
            List<DemandOrder> doList;
            if (employee != null && employee.DepartmentId == (int)DepartmentEnum.SalesAndMarketing)
            {
                var employeeRepository = new EmployeeRepository();
                var employeeIdList = employeeRepository.GetManagedEmployee(employee.Id).Select(x => x.Item1);
                doList = _ppsDbContext.DemandOrder.Where(x => x.PaymentStatusId == paymentStatus && employeeIdList.Contains(x.EmployeeId)).ToList();
            }
            else
            {
                doList = _ppsDbContext.DemandOrder.Where(x => x.PaymentStatusId == paymentStatus).ToList();
            }

            var doVm = new ConcurrentBag<DemandOrderVm>();
            doList.ForEach(d =>
            {
                var IsDODiscountPaid = d.DemandOrderDiscountTransaction.FirstOrDefault();
                if (IsDODiscountPaid != null)
                {
                    return;
                }

                var totalDOAmount = d.TotalGrandAmount;

                var doEarlyPaymentDiscountPercentage = d.SaleType?.EarlyPaymentDiscountInPercentage ?? 0;
                if (doEarlyPaymentDiscountPercentage == 0)
                {
                    throw new Exception("Early payment discount must be greater than 0");
                }
                var discountAmount = Math.Round(totalDOAmount * doEarlyPaymentDiscountPercentage / 100);

                var totalPaid = d.DemandOrderTransaction.ToList().Sum(x => x.TransactionAmount);
                var totalDue = d.TotalGrandAmount - totalPaid;

                int maturityDays;
                if (d.PaymentStatusId == (int)PaymentStatusEnum.Paid && d.PaymentCompleteDate != null)
                {
                    maturityDays = ((DateTime)d.PaymentCompleteDate - d.DODate).Days;
                }
                else
                {
                    maturityDays = (DateTime.Today - d.DODate).Days;
                }
                int maturityLabel = (int)DoMaturityLabelEnum.Normal;

                if (maturityDays > d.SaleType?.DurationInDays)
                {
                    maturityLabel = (int)DoMaturityLabelEnum.OverDue;
                }
                else if (maturityDays > d.SaleType?.WarningInDays)
                {
                    maturityLabel = (int)DoMaturityLabelEnum.Warning;
                }

                //Need to Check Code
                var paidDate = d.DemandOrderTransaction.OrderBy(x => x.TransactionDate).Last().TransactionDate;

                int earlyPaymentDays = (paidDate - d.DODate).Days;
                if (earlyPaymentDays <= d.SaleType?.EarlyPaymentInDays)
                {
                    doVm.Add(new DemandOrderVm
                    {
                        Id = d.Id,
                        DODate = d.DODate,
                        SaleTypeName = d.SaleType?.SaleTypeName,
                        DemandOrderTypeName = d.DemandOrderType?.DemandOrderTypeName,
                        MaturityDays = maturityDays,
                        MaturityLabel = maturityLabel,
                        ReferenceNo = d.ReferenceDONo,
                        CustomerName = d.Customer.CustomerName,
                        DOStatusName = d.DemandOrderStatus.Status,
                        TotalGrandAmount = d.TotalGrandAmount,
                        CreatedByName = StringExtension.ToFullName(d.User.FirstName, d.User.LastName),
                        CreatedDate = d.CreatedOn,
                        Submitted = d.SubmittedBy != null,
                        //DOPaymentStatusId = GetDemandOrderTransactionStatus(d, totalDue),
                        DOPaymentStatusId = d.PaymentStatusId,
                        DOPaymentStatus = d.PaymentStatus.PaymentStatusName,
                        DODiscountTransactionStatusName = GetDemandOrderEarlyPaymentTransactionStatus(d.DemandOrderDiscountTransaction.FirstOrDefault(x => x.DemandOrderDiscountTypeId == (int)DemandOrderDiscountTypeEnum.EarlyPayment)),
                        EarlyPaymentDiscountInPercentage = doEarlyPaymentDiscountPercentage,
                        EarlyPaymentDiscountAmount = discountAmount
                    });
                }
            });
            return doVm.ToList();
        }
        public IList<DemandOrderVm> GetDemandOrderEarlyPaymentPaidList(int userId)
        {
            int paymentStatus = (int)PaymentStatusEnum.Paid;
            var user = _ppsDbContext.User.FirstOrDefault(x => x.Id == userId);
            var employee = _ppsDbContext.Employee.FirstOrDefault(x => x.Id == user.EmployeeId);
            List<DemandOrder> doList;
            if (employee != null && employee.DepartmentId == (int)DepartmentEnum.SalesAndMarketing)
            {
                var employeeRepository = new EmployeeRepository();
                var employeeIdList = employeeRepository.GetManagedEmployee(employee.Id).Select(x => x.Item1);
                doList = _ppsDbContext.DemandOrder.Where(x => x.PaymentStatusId == paymentStatus && employeeIdList.Contains(x.EmployeeId)).ToList();
            }
            else
            {
                doList = _ppsDbContext.DemandOrder.Where(x => x.PaymentStatusId == paymentStatus).ToList();
            }

            var doVm = new ConcurrentBag<DemandOrderVm>();
            doList.ForEach(d =>
            {
                var IsDODiscountPaid = d.DemandOrderDiscountTransaction.FirstOrDefault();
                if (IsDODiscountPaid == null)
                {
                    return;
                }

                var totalDOAmount = d.TotalGrandAmount;
                var doEarlyPaymentDiscountPercentage = d.SaleType?.EarlyPaymentDiscountInPercentage ?? 0;
                if (doEarlyPaymentDiscountPercentage == 0)
                {
                    throw new Exception("Early payment discount must be greater than 0");
                }
                var discountAmount = Math.Round(totalDOAmount * doEarlyPaymentDiscountPercentage / 100);

                var totalPaid = d.DemandOrderTransaction.ToList().Sum(x => x.TransactionAmount);
                var totalDue = d.TotalGrandAmount - totalPaid;

                int maturityDays;
                if (d.PaymentStatusId == (int)PaymentStatusEnum.Paid && d.PaymentCompleteDate != null)
                {
                    maturityDays = ((DateTime)d.PaymentCompleteDate - d.DODate).Days;
                }
                else
                {
                    maturityDays = (DateTime.Today - d.DODate).Days;
                }
                int maturityLabel = (int)DoMaturityLabelEnum.Normal;

                if (maturityDays > d.SaleType?.DurationInDays)
                {
                    maturityLabel = (int)DoMaturityLabelEnum.OverDue;
                }
                else if (maturityDays > d.SaleType?.WarningInDays)
                {
                    maturityLabel = (int)DoMaturityLabelEnum.Warning;
                }

                var paidDate = d.DemandOrderTransaction.OrderBy(x => x.TransactionDate).Last().TransactionDate;

                int earlyPaymentDays = (paidDate - d.DODate).Days;
                if (earlyPaymentDays <= d.SaleType?.EarlyPaymentInDays)
                {
                    doVm.Add(new DemandOrderVm
                    {
                        Id = d.Id,
                        DODate = d.DODate,
                        SaleTypeName = d.SaleType?.SaleTypeName,
                        DemandOrderTypeName = d.DemandOrderType?.DemandOrderTypeName,
                        MaturityDays = maturityDays,
                        MaturityLabel = maturityLabel,
                        ReferenceNo = d.ReferenceDONo,
                        CustomerName = d.Customer.CustomerName,
                        DOStatusName = d.DemandOrderStatus.Status,
                        TotalGrandAmount = d.TotalGrandAmount,
                        CreatedByName = StringExtension.ToFullName(d.User.FirstName, d.User.LastName),
                        CreatedDate = d.CreatedOn,
                        Submitted = d.SubmittedBy != null,
                        //DOPaymentStatusId = GetDemandOrderTransactionStatus(d, totalDue),
                        DOPaymentStatusId = d.PaymentStatusId,
                        DOPaymentStatus = d.PaymentStatus.PaymentStatusName,
                        DODiscountTransactionStatusName = GetDemandOrderEarlyPaymentTransactionStatus(d.DemandOrderDiscountTransaction.FirstOrDefault(x => x.DemandOrderDiscountTypeId == (int)DemandOrderDiscountTypeEnum.EarlyPayment)),
                        EarlyPaymentDiscountInPercentage = doEarlyPaymentDiscountPercentage,
                        EarlyPaymentDiscountAmount = discountAmount
                    });
                }
            });
            return doVm.ToList();
        }
        public async Task<bool> PayDOEarlyPaymentDiscountToCustomer(int doId, int userId)
        {
            var demandOrder = _ppsDbContext.DemandOrder.FirstOrDefault(x => x.Id == doId);
            if (demandOrder == null)
            {
                throw new KeyNotFoundException($"The demand order no {doId} doesn't exist.");
            }

            var totalDOAmount = demandOrder.TotalGrandAmount;
            var doEarlyPaymentDiscountPercentage = demandOrder.SaleType?.EarlyPaymentDiscountInPercentage ?? 0;
            if (doEarlyPaymentDiscountPercentage == 0)
            {
                throw new Exception("Early payment discount must be greater than 0");
            }
            var discountAmount = Math.Round(totalDOAmount * doEarlyPaymentDiscountPercentage / 100);

            var demandOrderDiscountTransaction = new DemandOrderDiscountTransaction
            {
                DemandOrderId = doId,
                DemandOrderDiscountTypeId = (int)DemandOrderDiscountTypeEnum.EarlyPayment,
                TransactionAmount = discountAmount,
                TransactionDate = DateTime.Today,
                CreatedBy = userId,
                CreatedOn = DateTime.Now
            };

            _ppsDbContext.DemandOrderDiscountTransaction.Add(demandOrderDiscountTransaction);
            await _ppsDbContext.SaveChangesAsync();
            return true;
        }
        public IList<DemandOrderVm> GetDemandOrderEarlyPaymentPendingTransactionList(int userId)
        {
            int paymentStatus = (int)PaymentStatusEnum.Paid;
            var user = _ppsDbContext.User.FirstOrDefault(x => x.Id == userId);
            var employee = _ppsDbContext.Employee.FirstOrDefault(x => x.Id == user.EmployeeId);
            List<DemandOrder> doList;
            if (employee != null && employee.DepartmentId == (int)DepartmentEnum.SalesAndMarketing)
            {
                var employeeRepository = new EmployeeRepository();
                var employeeIdList = employeeRepository.GetManagedEmployee(employee.Id).Select(x => x.Item1);
                doList = _ppsDbContext.DemandOrder.Where(x => x.PaymentStatusId == paymentStatus && employeeIdList.Contains(x.EmployeeId)).ToList();
            }
            else
            {
                doList = _ppsDbContext.DemandOrder.Where(x => x.PaymentStatusId == paymentStatus).ToList();
            }

            var doVm = new ConcurrentBag<DemandOrderVm>();
            doList.ForEach(d =>
            {
                var IsApproved = d.DemandOrderDiscountTransaction.FirstOrDefault().IsApproved;
                if (IsApproved == true)
                {
                    return;
                }

                var totalDOAmount = d.TotalGrandAmount;
                var doEarlyPaymentDiscountPercentage = d.SaleType?.EarlyPaymentDiscountInPercentage ?? 0;
                if (doEarlyPaymentDiscountPercentage == 0)
                {
                    throw new Exception("Early payment discount must be greater than 0");
                }
                var discountAmount = Math.Round(totalDOAmount * doEarlyPaymentDiscountPercentage / 100);

                var totalPaid = d.DemandOrderTransaction.ToList().Sum(x => x.TransactionAmount);
                var totalDue = d.TotalGrandAmount - totalPaid;

                int maturityDays = (DateTime.Today - d.DODate).Days;
                int maturityLabel = (int)DoMaturityLabelEnum.Normal;

                if (maturityDays > d.SaleType?.DurationInDays)
                {
                    maturityLabel = (int)DoMaturityLabelEnum.OverDue;
                }
                else if (maturityDays > d.SaleType?.WarningInDays)
                {
                    maturityLabel = (int)DoMaturityLabelEnum.Warning;
                }

                var paidDate = d.DemandOrderTransaction.OrderBy(x => x.TransactionDate).Last().TransactionDate;

                int earlyPaymentDays = (paidDate - d.DODate).Days;
                if (earlyPaymentDays <= d.SaleType?.EarlyPaymentInDays)
                {
                    doVm.Add(new DemandOrderVm
                    {
                        Id = d.Id,
                        DODate = d.DODate,
                        SaleTypeName = d.SaleType?.SaleTypeName,
                        DemandOrderTypeName = d.DemandOrderType?.DemandOrderTypeName,
                        MaturityDays = maturityDays,
                        MaturityLabel = maturityLabel,
                        ReferenceNo = d.ReferenceDONo,
                        CustomerName = d.Customer.CustomerName,
                        DOStatusName = d.DemandOrderStatus.Status,
                        TotalGrandAmount = d.TotalGrandAmount,
                        CreatedByName = StringExtension.ToFullName(d.User.FirstName, d.User.LastName),
                        CreatedDate = d.CreatedOn,
                        Submitted = d.SubmittedBy != null,
                        //DOPaymentStatusId = GetDemandOrderTransactionStatus(d, totalDue),
                        DOPaymentStatusId = d.PaymentStatusId,
                        DOPaymentStatus = d.PaymentStatus.PaymentStatusName,
                        DODiscountTransactionStatusName = GetDemandOrderEarlyPaymentTransactionStatus(d.DemandOrderDiscountTransaction.FirstOrDefault(x => x.DemandOrderDiscountTypeId == (int)DemandOrderDiscountTypeEnum.EarlyPayment)),
                        EarlyPaymentDiscountInPercentage = doEarlyPaymentDiscountPercentage,
                        EarlyPaymentDiscountAmount = discountAmount
                    });
                }
            });
            return doVm.ToList();
        }
        public IList<DemandOrderVm> GetDemandOrderEarlyPaymentApprovedTransactionList(int userId)
        {
            int paymentStatus = (int)PaymentStatusEnum.Paid;
            var user = _ppsDbContext.User.FirstOrDefault(x => x.Id == userId);
            var employee = _ppsDbContext.Employee.FirstOrDefault(x => x.Id == user.EmployeeId);
            List<DemandOrder> doList;
            if (employee != null && employee.DepartmentId == (int)DepartmentEnum.SalesAndMarketing)
            {
                var employeeRepository = new EmployeeRepository();
                var employeeIdList = employeeRepository.GetManagedEmployee(employee.Id).Select(x => x.Item1);
                doList = _ppsDbContext.DemandOrder.Where(x => x.PaymentStatusId == paymentStatus && employeeIdList.Contains(x.EmployeeId)).ToList();
            }
            else
            {
                doList = _ppsDbContext.DemandOrder.Where(x => x.PaymentStatusId == paymentStatus).ToList();
            }

            var doVm = new ConcurrentBag<DemandOrderVm>();
            doList.ForEach(d =>
            {
                var IsApproved = d.DemandOrderDiscountTransaction.FirstOrDefault().IsApproved;
                if (IsApproved == false)
                {
                    return;
                }

                var totalDOAmount = d.TotalGrandAmount;
                var doEarlyPaymentDiscountPercentage = d.SaleType?.EarlyPaymentDiscountInPercentage ?? 0;
                if (doEarlyPaymentDiscountPercentage == 0)
                {
                    throw new Exception("Early payment discount must be greater than 0");
                }
                var discountAmount = Math.Round(totalDOAmount * doEarlyPaymentDiscountPercentage / 100);

                var totalPaid = d.DemandOrderTransaction.ToList().Sum(x => x.TransactionAmount);
                var totalDue = d.TotalGrandAmount - totalPaid;

                int maturityDays = (DateTime.Today - d.DODate).Days;
                int maturityLabel = (int)DoMaturityLabelEnum.Normal;

                if (maturityDays > d.SaleType?.DurationInDays)
                {
                    maturityLabel = (int)DoMaturityLabelEnum.OverDue;
                }
                else if (maturityDays > d.SaleType?.WarningInDays)
                {
                    maturityLabel = (int)DoMaturityLabelEnum.Warning;
                }

                var paidDate = d.DemandOrderTransaction.OrderBy(x => x.TransactionDate).Last().TransactionDate;

                int earlyPaymentDays = (paidDate - d.DODate).Days;
                if (earlyPaymentDays <= d.SaleType?.EarlyPaymentInDays)
                {
                    doVm.Add(new DemandOrderVm
                    {
                        Id = d.Id,
                        DODate = d.DODate,
                        SaleTypeName = d.SaleType?.SaleTypeName,
                        DemandOrderTypeName = d.DemandOrderType?.DemandOrderTypeName,
                        MaturityDays = maturityDays,
                        MaturityLabel = maturityLabel,
                        ReferenceNo = d.ReferenceDONo,
                        CustomerName = d.Customer.CustomerName,
                        DOStatusName = d.DemandOrderStatus.Status,
                        TotalGrandAmount = d.TotalGrandAmount,
                        CreatedByName = StringExtension.ToFullName(d.User.FirstName, d.User.LastName),
                        CreatedDate = d.CreatedOn,
                        Submitted = d.SubmittedBy != null,
                        DOPaymentStatusId = GetDemandOrderTransactionStatus(d, totalDue),
                        DOPaymentStatus = d.PaymentStatus.PaymentStatusName,
                        DODiscountTransactionStatusName = d.DemandOrderDiscountTransaction.FirstOrDefault().IsApproved == true ? "Approved" : "Pending",
                        EarlyPaymentDiscountInPercentage = doEarlyPaymentDiscountPercentage,
                        EarlyPaymentDiscountAmount = discountAmount,
                    });
                }
            });
            return doVm.ToList();
        }
        public async Task<bool> VerifyDOEarlyPaymentDiscountToCustomer(DemandOrderEarlyPaymentRequestVm demandOrderEarlyPaymentRequestVm)
        {
            using (var db = new PPSDbContext())
            {
                var demandOrderDiscountTransaction = await db.DemandOrderDiscountTransaction.FirstOrDefaultAsync(x => x.DemandOrderId == demandOrderEarlyPaymentRequestVm.DemandOrderId);
                if (demandOrderDiscountTransaction == null)
                {
                    throw new KeyNotFoundException($"The demand order no {demandOrderEarlyPaymentRequestVm.DemandOrderId} doesn't exist.");
                }

                demandOrderDiscountTransaction.VerifiedBy = demandOrderEarlyPaymentRequestVm.UserId;
                demandOrderDiscountTransaction.VerifiedOn = demandOrderEarlyPaymentRequestVm.DatedOn;
                demandOrderDiscountTransaction.IsVerified = true;

                //demandOrderDiscountTransaction.DeletedBy = demandOrderEarlyPaymentRequestVm.UserId;
                //demandOrderDiscountTransaction.DeletedOn = demandOrderEarlyPaymentRequestVm.DatedOn;
                //demandOrderDiscountTransaction.IsDeleted = true;

                var referenceTable = await db.ReferenceTable.ToListAsync();

                var earlyPaymentDiscountAccountHeadId = referenceTable.FirstOrDefault(x => x.ReferenceName == ReferenceTableEnum.EarlyPaymentDiscountAccountHeadId.ToString())?.ReferenceValue;
                if (earlyPaymentDiscountAccountHeadId == null)
                {
                    throw new KeyNotFoundException("Early payment discount account head doesn't exist.");
                }

                // Hit the accounting head for this dealer
                var lastTran = _ppsDbContext.TransactionEntry
                .Where(x => x.TransactionTypeId == (int)TransactionTypeEnum.Journal
                            && x.FiscalYear == demandOrderEarlyPaymentRequestVm.FiscalYear
                            && x.CompanyId == demandOrderEarlyPaymentRequestVm.CompanyId)
                .OrderByDescending(x => x.TransactionNumber.Substring(9, 4))
                .FirstOrDefault();

                var lastNumber = 0;
                if (lastTran != null)
                {
                    lastNumber = int.Parse(lastTran.TransactionNumber.Substring(9, 4));
                }
                var transactionNumber = _transactionRepository.CreateTransactionNumber((int)TransactionTypeEnum.Payment, DateTime.Now, lastNumber + 1);

                var systemTransaction = new TransactionEntry
                {
                    IsSystemGenerated = true,
                    TransactionNumber = transactionNumber,
                    TransactionDate = demandOrderEarlyPaymentRequestVm.DatedOn,
                    FiscalYear = demandOrderEarlyPaymentRequestVm.FiscalYear,
                    TransactionTypeId = (int)TransactionTypeEnum.Journal,
                    CompanyId = demandOrderEarlyPaymentRequestVm.CompanyId,
                    PostingDate = demandOrderEarlyPaymentRequestVm.DatedOn,
                    Active = true,
                    Deleted = false,
                    Accepted = false,
                    CreatedById = demandOrderDiscountTransaction.CreatedBy,
                    CreatedDate = demandOrderDiscountTransaction.CreatedOn,
                    VerifiedById = demandOrderEarlyPaymentRequestVm.UserId,
                    VerifiedDate = demandOrderEarlyPaymentRequestVm.DatedOn,
                    TransactionDetail = new List<TransactionDetail>()
                };

                systemTransaction.TransactionDetail = new List<TransactionDetail>
                {
                    new TransactionDetail
                    {
                        AccountHeadId = int.Parse(earlyPaymentDiscountAccountHeadId),
                        CrAmount = 0,
                        DrAmount = demandOrderDiscountTransaction.TransactionAmount
                    },
                    new TransactionDetail
                    {
                        AccountHeadId = (int) demandOrderDiscountTransaction.DemandOrder.Customer.AccountHeadId,
                        CrAmount = demandOrderDiscountTransaction.TransactionAmount,
                        DrAmount = 0
                    }
                };

                db.TransactionEntry.Add(systemTransaction);

                db.DemandOrderDiscountTransaction.Attach(demandOrderDiscountTransaction);
                db.Entry(demandOrderDiscountTransaction).State = EntityState.Modified;

                await db.SaveChangesAsync();

                demandOrderDiscountTransaction.TransactionEntryId = systemTransaction.Id;

                db.DemandOrderDiscountTransaction.Attach(demandOrderDiscountTransaction);
                db.Entry(demandOrderDiscountTransaction).State = EntityState.Modified;

                await db.SaveChangesAsync();
                return true;
            }
        }
        public IList<CompanySalesTargetVm> GetCompanySalesTargetList(int userId)
        {
            var salesTargetList = _ppsDbContext.CompanySalesTarget.ToList()
                .Select(x => new CompanySalesTargetVm
                {
                    Id = x.Id,
                    SalesTarget = x.SalesTarget,
                    TargetDate = x.SalesYear + "-" + x.SalesMonth + "-01",
                    SalesYear = x.SalesYear,
                    CreatedBy = x.CreatedBy,
                    CreatedByName = GetUserName(x.CreatedBy),
                    CreatedOn = x.CreatedOn,
                    ApprovedBy = x.ApprovedBy,
                    ApprovedByName = x.ApprovedBy != null ? GetUserNameById((int)x.ApprovedBy).Item1 : null,
                    ApprovedByOn = x.ApprovedBy != null && x.ApprovedOn != null ? GetUserByOn((int)x.ApprovedBy, (DateTime)x.ApprovedOn) : "-",
                    Status = x.IsApproved == true ? "Approved" : "Pending"
                }).ToList();
            return salesTargetList;
        }
        public CompanySalesTargetVm SaveCompanySalesTarget(CompanySalesTargetVm companySalesTarget)
        {
            var isSalesTargetExist = _ppsDbContext.CompanySalesTarget.FirstOrDefault(x =>
                x.SalesMonth == companySalesTarget.SalesMonth &&
                x.SalesYear == companySalesTarget.SalesYear);

            if (isSalesTargetExist != null)
            {
                throw new Exception("Sales Target for this month is already allocated.");
            }

            var salesTargetEntry = new CompanySalesTarget
            {
                SalesTarget = companySalesTarget.SalesTarget,
                SalesYear = companySalesTarget.SalesYear,
                SalesMonth = companySalesTarget.SalesMonth,
                CreatedBy = companySalesTarget.CreatedBy,
                CreatedOn = companySalesTarget.CreatedOn,
                IsApproved = false,
            };

            _ppsDbContext.CompanySalesTarget.Add(salesTargetEntry);
            _ppsDbContext.SaveChanges();

            companySalesTarget.Id = salesTargetEntry.Id;
            return companySalesTarget;
        }
        public CompanySalesTargetVm UpdateCompanySalesTarget(CompanySalesTargetVm companySalesTarget)
        {
            using (var db = new PPSDbContext())
            {
                var isSalesTargetExist = db.CompanySalesTarget.FirstOrDefault(x =>
                    x.SalesMonth == companySalesTarget.SalesMonth &&
                    x.SalesYear == companySalesTarget.SalesYear &&
                    x.Id != companySalesTarget.Id);

                if (isSalesTargetExist != null)
                {
                    throw new Exception("Sales Target for this month is already allocated.");
                }

                var salesTargetEntry = db.CompanySalesTarget.FirstOrDefault(x => x.Id == companySalesTarget.Id);
                if (salesTargetEntry == null)
                {
                    throw new KeyNotFoundException($"The company sales target no {companySalesTarget.Id} doesn't exist.");
                }

                if (salesTargetEntry.IsApproved == true)
                {
                    throw new Exception($"The company sales target no: { companySalesTarget.Id } has already been approved.");
                }

                salesTargetEntry.SalesTarget = companySalesTarget.SalesTarget;
                salesTargetEntry.SalesYear = companySalesTarget.SalesYear;
                salesTargetEntry.SalesMonth = companySalesTarget.SalesMonth;
                salesTargetEntry.UpdatedBy = companySalesTarget.UpdatedBy;
                salesTargetEntry.UpdatedOn = companySalesTarget.UpdatedOn;

                db.CompanySalesTarget.Attach(salesTargetEntry);
                db.Entry(salesTargetEntry).State = EntityState.Modified;
                db.SaveChanges();

                companySalesTarget.Id = salesTargetEntry.Id;
                return companySalesTarget;
            }
        }
        public async Task<bool> ApproveCompanySalesTarget(int companySalesTargetId, int userId)
        {
            using (var db = new PPSDbContext())
            {
                var companySalesTarget =
                    db.CompanySalesTarget.FirstOrDefault(x => x.Id == companySalesTargetId);
                if (companySalesTarget == null)
                {
                    throw new KeyNotFoundException(
                        $"The company sales target no {companySalesTargetId} doesn't exist.");
                }

                if (companySalesTarget.IsApproved == true)
                {
                    throw new Exception(
                        $"The company sales target no: {companySalesTargetId} has already been approved.");
                }

                companySalesTarget.ApprovedBy = userId;
                companySalesTarget.ApprovedOn = DateTime.Now;
                companySalesTarget.IsApproved = true;

                var referenceTable = db.ReferenceTable.ToList();

                var headOfSalesEmployeeId = referenceTable.FirstOrDefault(x => x.ReferenceName == ReferenceTableEnum.HeadOfSalesEmployeeId.ToString())?.ReferenceValue;
                if (headOfSalesEmployeeId == null)
                {
                    throw new KeyNotFoundException("Head of Sales employee id doesn't exist.");
                }

                var employeeSalesTargetMonthly = new EmployeeSalesTargetMonthly
                {
                    EmployeeId = int.Parse(headOfSalesEmployeeId),
                    TeamTarget = companySalesTarget.SalesTarget,
                    SalesYear = companySalesTarget.SalesYear,
                    SalesMonth = companySalesTarget.SalesMonth
                };

                db.CompanySalesTarget.Attach(companySalesTarget);
                db.Entry(companySalesTarget).State = EntityState.Modified;

                db.EmployeeSalesTargetMonthly.Add(employeeSalesTargetMonthly);

                await db.SaveChangesAsync();

                return true;
            }
        }
        public CompanySalesTargetVm GetCompanySalesTargetById(int userId, int companySalesTargetId)
        {
            var companySalesTarget = _ppsDbContext.CompanySalesTarget.FirstOrDefault(x => x.Id == companySalesTargetId);
            if (companySalesTarget == null)
            {
                throw new KeyNotFoundException($"The company sales target no {companySalesTargetId} doesn't exist.");
            }
            var companySalesTargetVm = new CompanySalesTargetVm
            {
                Id = companySalesTarget.Id,
                SalesTarget = companySalesTarget.SalesTarget,
                TargetDate = companySalesTarget.SalesYear + "-" + companySalesTarget.SalesMonth + "-01",
                CreatedBy = companySalesTarget.CreatedBy,
                CreatedByName = GetUserName(companySalesTarget.CreatedBy),
                CreatedOn = companySalesTarget.CreatedOn,
                ApprovedBy = companySalesTarget.ApprovedBy,
                ApprovedByName = companySalesTarget.ApprovedBy != null ? GetUserNameById((int)companySalesTarget.ApprovedBy).Item1 : null,
                ApprovedByOn = companySalesTarget.ApprovedBy != null && companySalesTarget.ApprovedOn != null ? GetUserByOn((int)companySalesTarget.ApprovedBy, (DateTime)companySalesTarget.ApprovedOn) : "-",
                Status = companySalesTarget.IsApproved == true ? "Approved" : "Pending"
            };
            return companySalesTargetVm;
        }
        public List<SalesTeamTargetVm> GetSalesTeamTargetList(int userId, int year, int month)
        {
            var salesTeamTargetList = _ppsDbContext.EmployeeSalesTargetMonthly
                    .Where(k => k.SalesYear == year && k.SalesMonth == month)
                .OrderBy(o => o.Employee.Designation.DesignationSerialNo)
                .Select(x => new SalesTeamTargetVm
                {
                    EmployeeId = x.EmployeeId,
                    EmployeeName = x.Employee.FirstName + " " + x.Employee.LastName + " (" +
                               x.Employee.Designation.DesignationName + ")",
                    SalesTarget = x.SalesTarget,
                    TeamTarget = x.TeamTarget,
                    Achievement = x.Achievement ?? 0,
                    Percentage = x.Percentage ?? 0,
                    TargetDate = x.SalesYear + "-" + x.SalesMonth + "-01",
                    SalesYear = x.SalesYear,
                    SalesMonth = x.SalesMonth
                }).ToList();
            return salesTeamTargetList;
        }
        public List<SalesTeamTargetVm> SaveSalesTeamTarget(List<SalesTeamTargetVm> salesTeamTargetVm)
        {
            if (salesTeamTargetVm == null)
            {
                throw new Exception("Please add sales team target");
            }

            var newSalesTeamTargetList = new List<EmployeeSalesTargetMonthly>();

            salesTeamTargetVm.ForEach(s =>
            {
                var existedSalesTarget = _ppsDbContext.EmployeeSalesTargetMonthly.FirstOrDefault(x =>
                    x.EmployeeId == s.EmployeeId && x.SalesYear == s.SalesYear && x.SalesMonth == s.SalesMonth);
                if (existedSalesTarget != null)
                {
                    existedSalesTarget.SalesTarget = s.SalesTarget;
                    existedSalesTarget.TeamTarget = s.TeamTarget;

                    _ppsDbContext.EmployeeSalesTargetMonthly.Attach(existedSalesTarget);
                    _ppsDbContext.Entry(existedSalesTarget).State = EntityState.Modified;
                }
                else
                {
                    newSalesTeamTargetList.Add(new EmployeeSalesTargetMonthly
                    {
                        EmployeeId = s.EmployeeId,
                        SalesTarget = s.SalesTarget,
                        TeamTarget = s.TeamTarget,
                        SalesYear = s.SalesYear,
                        SalesMonth = s.SalesMonth
                    });
                }
            });

            _ppsDbContext.EmployeeSalesTargetMonthly.AddRange(newSalesTeamTargetList);
            _ppsDbContext.SaveChanges();
            return salesTeamTargetVm;
        }
        public SalesTeamTargetVm UpdateSalesTeamTarget(SalesTeamTargetVm salesTeamTargetVm)
        {
            throw new NotImplementedException();
        }
        public CompanySalesTargetVm GetSalesTeamTargetById(int userId, int salesTeamTargetId)
        {
            throw new NotImplementedException();
        }
        public List<SalesDivisionVm> GetSalesDivisionList(int userId)
        {
            var salesDivisionList = _ppsDbContext.SalesDivision
                .Select(x => new SalesDivisionVm
                {
                    Id = x.Id,
                    SalesDivisionName = x.SalesDivisionName
                }).ToList();
            return salesDivisionList;
        }
        public List<SalesAreaVm> GetSalesAreaList(int userId)
        {
            var salesAreaList = _ppsDbContext.SalesArea
                .Select(x => new SalesAreaVm
                {
                    Id = x.Id,
                    SalesAreaName = x.SalesAreaName,
                    SalesDivisionId = x.SalesDivisionId,
                    SalesDivisionName = x.SalesDivision.SalesDivisionName
                }).ToList();
            return salesAreaList;
        }
        //public List<SalesReportVm> GetSalesReportList(DateTime startDate, DateTime endDate,
        //    int salesDivisionId, int salesAreaId, int employeeId, int customerId)
        //{
        //    var salesReportList = new List<SalesReportVm>();

        //    var doList = _ppsDbContext.DemandOrder
        //        .Where(x => DbFunctions.TruncateTime(x.DODate) >= startDate.Date && DbFunctions.TruncateTime(x.DODate) <= endDate.Date);

        //    if (!doList.Any())
        //    {
        //        return salesReportList;
        //    }
        //    if (salesDivisionId != -1)
        //    {
        //        doList = doList.Where(x => x.Employee.SalesDivisionId == salesDivisionId);
        //    }
        //    if (salesAreaId != -1)
        //    {
        //        doList = doList.Where(x => x.Employee.SalesAreaId == salesAreaId);
        //    }
        //    if (employeeId != -1)
        //    {
        //        doList = doList.Where(x => x.EmployeeId == employeeId);
        //    }
        //    if (customerId != -1)
        //    {
        //        doList = doList.Where(x => x.CustomerId == customerId);
        //    }
        //    var filteredDoList = doList.OrderByDescending(x => x.DODate)
        //        .ThenBy(x => x.Employee.SalesDivisionId != null && x.Employee.SalesDivision.SalesDivisionName != null ? x.Employee.SalesDivision.SalesDivisionName : string.Empty)
        //        .ThenBy(x => x.Employee.SalesAreaId != null && x.Employee.SalesArea.SalesAreaName != null ? x.Employee.SalesArea.SalesAreaName : string.Empty)
        //        .ThenBy(x => x.Employee.FirstName)
        //        .ThenBy(x => x.Customer.CustomerName)
        //        .ToList();

        //    filteredDoList.ForEach(x =>
        //    {
        //        var doAmount = x.TotalGrandAmount;
        //        var doPaidAmount = x.DemandOrderTransaction.Sum(y => y.TransactionAmount);
        //        var doBalanceAmount = doAmount - doPaidAmount;

        //        salesReportList.Add(new SalesReportVm
        //        {
        //            DODate = x.DODate,
        //            CustomerName = x.Customer.CustomerName,
        //            CustomerCode = x.Customer.CustomerCode,
        //            SalesDivisionName = x.Employee.SalesDivision?.SalesDivisionName ?? string.Empty,
        //            SalesAreaName = x.Employee.SalesArea?.SalesAreaName ?? string.Empty,
        //            SalesOfficer = x.Employee.FirstName + " " + x.Employee.LastName + " (" + x.Employee.Designation.DesignationName + ")",
        //            DONo = x.DemandOrderNo,
        //            DOAmount = doAmount,
        //            DOPaid = doPaidAmount,
        //            DOBalance = doBalanceAmount
        //        });
        //    });

        //    return salesReportList;
        //}
        public List<SalesReportVm> GetSalesReportList(DateTime startDate, DateTime endDate,
            int salesDivisionId, int salesAreaId, int employeeId, int customerId)
        {
            var salesReportList = new List<SalesReportVm>();

            var doList = _ppsDbContext.DemandOrder
                .Where(x => DbFunctions.TruncateTime(x.DODate) >= startDate.Date && DbFunctions.TruncateTime(x.DODate) <= endDate.Date);

            if (!doList.Any())
            {
                return salesReportList;
            }
            if (salesDivisionId != -1)
            {
                doList = doList.Where(x => x.Employee.SalesDivisionId == salesDivisionId);
            }
            if (salesAreaId != -1)
            {
                doList = doList.Where(x => x.Employee.SalesAreaId == salesAreaId);
            }
            if (employeeId != -1)
            {
                doList = doList.Where(x => x.EmployeeId == employeeId);
            }
            if (customerId != -1)
            {
                doList = doList.Where(x => x.CustomerId == customerId);
            }
            var filteredDoList = doList.OrderByDescending(x => x.DODate)
                .ThenBy(x => x.Employee.SalesDivisionId != null && x.Employee.SalesDivision.SalesDivisionName != null ? x.Employee.SalesDivision.SalesDivisionName : string.Empty)
                .ThenBy(x => x.Employee.SalesAreaId != null && x.Employee.SalesArea.SalesAreaName != null ? x.Employee.SalesArea.SalesAreaName : string.Empty)
                .ThenBy(x => x.Employee.FirstName)
                .ThenBy(x => x.Customer.CustomerName)
                .ToList();

            filteredDoList.ForEach(x =>
            {
                if (x.Invoice.Count() == 0)
                {
                    return;
                }
                var count = 1;
                var doAmount = x.Invoice.Sum(y => y.TotalGrandAmount) ?? 0;
                var doPaidAmount = x.Invoice.Sum(y => y.InvoiceTransaction.Sum(k => k.TransactionAmount));
                var doBalanceAmount = doAmount - doPaidAmount;
                if (count == 1)
                {
                    salesReportList.Add(new SalesReportVm
                    {
                        DODate = x.DODate,
                        CustomerName = x.Customer.CustomerName,
                        CustomerCode = x.Customer.CustomerCode,
                        SalesDivisionName = x.Employee.SalesDivision?.SalesDivisionName ?? string.Empty,
                        SalesAreaName = x.Employee.SalesArea?.SalesAreaName ?? string.Empty,
                        SalesOfficer = x.Employee.FirstName + " " + x.Employee.LastName + " (" + x.Employee.Designation.DesignationName + ")",
                        DONo = x.DemandOrderNo,
                        DOAmount = doAmount,
                        DOPaid = doPaidAmount,
                        DOBalance = doBalanceAmount
                    });
                    count++;
                }
                else
                {
                    salesReportList.Add(new SalesReportVm
                    {
                        DODate = x.DODate,
                        CustomerName = x.Customer.CustomerName,
                        CustomerCode = x.Customer.CustomerCode,
                        SalesDivisionName = x.Employee.SalesDivision?.SalesDivisionName ?? string.Empty,
                        SalesAreaName = x.Employee.SalesArea?.SalesAreaName ?? string.Empty,
                        SalesOfficer = x.Employee.FirstName + " " + x.Employee.LastName + " (" + x.Employee.Designation.DesignationName + ")",
                        DONo = x.DemandOrderNo,
                        DOAmount = doAmount,
                        DOPaid = doPaidAmount,
                        DOBalance = doBalanceAmount
                    });
                    count++;
                }
            });

            var report = salesReportList.GroupBy(x => new { x.CustomerCode, x.CustomerName, x.SalesDivisionName, x.SalesAreaName, x.SalesOfficer })
                .Select(g => new SalesReportVm
                {
                    SalesDivisionName = g.Key.SalesDivisionName,
                    SalesAreaName = g.Key.SalesAreaName,
                    SalesOfficer = g.Key.SalesOfficer,
                    CustomerCode = g.Key.CustomerCode,
                    CustomerName = g.Key.CustomerName,
                    DOAmount = g.Sum(k => k.DOAmount),
                    DOPaid = g.Sum(k => k.DOPaid),
                    DOBalance = g.Sum(k => k.DOBalance)
                }).ToList();

            return report;
        }
        public IQueryable<MonthlyProcessing> GetMonthlyProcessing()
        {
            return _ppsDbContext.MonthlyProcessing;
        }
        public IQueryable<EmployeeSalesTargetMonthly> GetEmployeeSalesTargetMonthly()
        {
            return _ppsDbContext.EmployeeSalesTargetMonthly;
        }
        public IQueryable<CustomerTransaction> GetCustomerTransaction()
        {
            return _ppsDbContext.CustomerTransaction;
        }
        public IQueryable<DemandOrderTransaction> GetDemandOrderTransaction()
        {
            return _ppsDbContext.DemandOrderTransaction;
        }
        public IQueryable<DemandOrder> GetDemandOrder()
        {
            return _ppsDbContext.DemandOrder;
        }
        public async Task<bool> ProcessMonthlyAchievement(MonthlyProcessing monthlyProcessing, List<EmployeeSalesTargetMonthly> monthlySalesTarget, bool reprocess, int year, int month, int userId)
        {
            using (var db = new PPSDbContext())
            {
                if (reprocess)
                {
                    var process = db.MonthlyProcessing.FirstOrDefault(x => x.Year == year && x.Month == month);
                    if (process == null)
                    {
                        throw new Exception("Data couldn't found into the system.");
                    }

                    process.ReprocessedBy = userId;
                    process.ReprocessedOn = DateTime.Now;

                    db.MonthlyProcessing.Attach(process);
                    db.Entry(process).State = EntityState.Modified;
                }
                else
                {
                    db.MonthlyProcessing.Add(monthlyProcessing);
                }

                monthlySalesTarget.ForEach(x =>
                {
                    var existedMonthlySalesTarget = db.EmployeeSalesTargetMonthly.FirstOrDefault(y => y.Id == x.Id);
                    if (existedMonthlySalesTarget == null)
                    {
                        throw new Exception("Data couldn't found into the system.");
                    }

                    existedMonthlySalesTarget.Achievement = x.Achievement;
                    existedMonthlySalesTarget.Percentage = x.Percentage;

                    db.EmployeeSalesTargetMonthly.Attach(existedMonthlySalesTarget);
                    db.Entry(existedMonthlySalesTarget).State = EntityState.Modified;

                });
                await db.SaveChangesAsync();
            }
            return true;
        }
        public List<ProductionForecastVm> GetProductionForecastList(int year, int month)
        {
            var productionForecastList = _ppsDbContext.ProductionForecastMonthly
                    .Where(k => k.SalesYear == year && k.SalesMonth == month)
                .Select(x => new ProductionForecastVm
                {
                    Id = x.Id,
                    ProductId = x.ProductId,
                    ProductName = x.Product.Name + " (" + x.Product.Color + ")",
                    UnitPrice = x.Product.UnitPrice,
                    Quantity = x.Quantity,
                    TotalUnitPrice = x.Quantity * x.Product.UnitPrice,
                    TargetDate = x.SalesYear + "-" + x.SalesMonth + "-01",
                    SalesYear = x.SalesYear,
                    SalesMonth = x.SalesMonth
                }).ToList();
            return productionForecastList;
        }
        public List<ProductionForecastVm> SaveProductionForecast(List<ProductionForecastVm> productionForecastList)
        {
            if (productionForecastList == null)
            {
                throw new Exception("Please add production forecast");
            }

            var newProductionForecastList = new List<ProductionForecastMonthly>();

            productionForecastList.ForEach(s =>
            {
                var existedProductionForecast = _ppsDbContext.ProductionForecastMonthly.FirstOrDefault(x =>
                    x.ProductId == s.ProductId && x.SalesYear == s.SalesYear && x.SalesMonth == s.SalesMonth);
                if (existedProductionForecast != null)
                {
                    existedProductionForecast.Quantity = s.Quantity;

                    _ppsDbContext.ProductionForecastMonthly.Attach(existedProductionForecast);
                    _ppsDbContext.Entry(existedProductionForecast).State = EntityState.Modified;
                }
                else
                {
                    var companySalesTargetId = _ppsDbContext.CompanySalesTarget.FirstOrDefault(x => x.SalesYear == s.SalesYear && x.SalesMonth == s.SalesMonth)?.Id;
                    if (companySalesTargetId == null)
                    {
                        throw new Exception("Please setup your company sales target");
                    }

                    newProductionForecastList.Add(new ProductionForecastMonthly
                    {
                        ProductId = s.ProductId,
                        Quantity = s.Quantity,
                        SalesYear = s.SalesYear,
                        SalesMonth = s.SalesMonth,
                        CompanySalesTargetId = (int)companySalesTargetId
                    });
                }
            });

            _ppsDbContext.ProductionForecastMonthly.AddRange(newProductionForecastList);
            _ppsDbContext.SaveChanges();
            return productionForecastList;
        }
        public IQueryable<SalesArea> GetSalesAreaList()
        {
            return _ppsDbContext.SalesArea;
        }
        public IQueryable<Employee> GetEmployees()
        {
            return _ppsDbContext.Employee;
        }
        public IQueryable<Customer> GetCustomers()
        {
            return _ppsDbContext.Customer;
        }
        //invoice return process start
        public IQueryable<InvoiceReturn> InvoiceReturnList()
        {
            return _ppsDbContext.InvoiceReturn;
        }
        public IQueryable<Invoice> GetAllInvoiceList()
        {
            return _ppsDbContext.Invoice;
        }
        //invoice return process end
        //total sales report repository interface

        public IQueryable<Invoice> GetTotalSalesReportList(DateTime startDate, DateTime endDate)
        {
            return _ppsDbContext.Invoice.Where(m => m.DeliveredOn >= startDate && m.DeliveredOn <= endDate);

        }

        #region Private members
        private int GetDemandOrderTransactionStatus(DemandOrder demandOrder, double totalDueAmount)
        {
            int doPaymentStatusId;
            if (demandOrder.ApprovedBy == null)
            {
                doPaymentStatusId = (int)DemandOrderTransactionStatusEnum.NotApproved;
                return doPaymentStatusId;
            }
            if (demandOrder.ApprovedOn == null)
            {
                throw new InvalidDataException();
            }

            var durationInDays = demandOrder.SaleType.DurationInDays;
            var warningInDays = demandOrder.SaleType.WarningInDays;

            var daysDifference = Math.Round((DateTime.Now.Date - (DateTime)demandOrder.ApprovedOn).TotalDays) + 1;
            // TODO: Need to minus holidays
            if (totalDueAmount.Equals(0))
            {
                doPaymentStatusId = (int)DemandOrderTransactionStatusEnum.Paid;
            }
            else if (daysDifference > durationInDays)
            {
                doPaymentStatusId = (int)DemandOrderTransactionStatusEnum.Danger;
            }
            else if (daysDifference.Equals(durationInDays))
            {
                doPaymentStatusId = (int)DemandOrderTransactionStatusEnum.PayDay;
            }
            else if (daysDifference >= warningInDays)
            {
                doPaymentStatusId = (int)DemandOrderTransactionStatusEnum.Warning;
            }
            else
            {
                doPaymentStatusId = (int)DemandOrderTransactionStatusEnum.Regular;
            }
            return doPaymentStatusId;
        }
        private string GetUserByOn(int userId, DateTime on)
        {
            var user = _ppsDbContext.User.FirstOrDefault(x => x.Id == userId);
            var text = StringExtension.ToFullName(user?.FirstName, user?.LastName);
            text += " - " + on.ToString("dd/MM/yyyy");
            return text;
        }
        private string GetUserName(int userId)
        {
            var user = _ppsDbContext.User.FirstOrDefault(x => x.Id == userId);
            var text = StringExtension.ToFullName(user?.FirstName, user?.LastName);
            return text;
        }
        private SalesPersonVm CreateSalesPersonHierarchy(EmployeeDetailVm manager, int year)
        {
            if (manager == null)
            {
                return null;
            }

            var currentSalesTarget = GetTotalCurrentSalesTarget(manager);
            var totalDoAmount = GetTotalDoAmount(manager);
            var currentCollection = GetTotalCurrentCollection(manager);
            var totalDueCollection = GetTotalDueAmount(manager);
            var dueBalance = totalDoAmount - currentCollection;


            var vm = new SalesPersonVm
            {
                Name = manager.FullName,
                Designation = manager.Designation,
                SalesCode = manager.EmployeeCode,
                Area = manager.Address,
                CurrentSalesTarget = currentSalesTarget,
                TotalDoAmount = totalDoAmount,
                TotalDueCollection = totalDueCollection,
                DueBalance = dueBalance,
                CurrentCollection = currentCollection,
                SalesPersonList = new List<SalesPersonVm>(),
                SalesMonthlyHistoryList = new List<SalesMonthlyHistoryVm>(),
                DealerMonthlyHistoryList = new List<DealerMonthlyHistoryVm>()
            };
            if (manager.Employees.Count > 0)
            {
                //TODO Find out current achievement 
                manager.Employees.ToList().ForEach(emp =>
                {
                    var mgr = CreateSalesPersonHierarchy(emp, year);
                    //TODO Sales Monthly History & Dealer List
                    vm.SalesPersonList.Add(mgr);
                    //Sales history monthly
                    var salesHistoryMonthly = GetSalesHistoryMonthly(emp.Id, year);
                    //Dealer history monthly
                    var dealerHistoryMonthly = GetDealerMonthlyHistory(emp.Id, year);
                    vm.SalesMonthlyHistoryList = salesHistoryMonthly;
                    vm.DealerMonthlyHistoryList = dealerHistoryMonthly;
                });
            }
            else
            {
                //Sales history monthly
                var salesHistoryMonthly = GetSalesHistoryMonthly(manager.Id, year);
                //Dealer history monthly
                var dealerHistoryMonthly = GetDealerMonthlyHistory(manager.Id, year);
                vm.SalesMonthlyHistoryList = salesHistoryMonthly;
                vm.DealerMonthlyHistoryList = dealerHistoryMonthly;
            }
            return vm;
        }
        private double GetTotalCurrentSalesTarget(EmployeeDetailVm manager)
        {
            var totalCurrentSalesTarget = 0.0;
            //var managedEmployeeIdList = _employeeRepository.GetManagedEmployee(manager.Id).Select(x => x.Item1);
            //var employeeList = _ppsDbContext.Employee
            //    .Where(x => managedEmployeeIdList.Contains(x.Id) && x.CurrentSalesTarget != null).ToList();
            //if (employeeList.Count <= 0)
            //{
            //    return totalCurrentSalesTarget;
            //}
            //decimal? sum = 0;
            //foreach (var x in employeeList)
            //{
            //    var @decimal = x.CurrentSalesTarget;
            //    if (@decimal.HasValue) sum += @decimal.Value;
            //}

            //totalCurrentSalesTarget = (double)sum;
            return totalCurrentSalesTarget;
        }
        private double GetTotalDoAmount(EmployeeDetailVm manager)
        {
            var totalDoAmount = 0.0;
            var managedEmployeeIdList = _employeeRepository.GetManagedEmployee(manager.Id).Select(x => x.Item1);
            var doList = _ppsDbContext.DemandOrder.Where(x =>
                x.CreatedOn.Month == DateTime.Now.Month && managedEmployeeIdList.Contains(x.EmployeeId)).ToList();
            if (doList.Count <= 0) return totalDoAmount;
            {
                totalDoAmount = doList.Sum(x => x.TotalGrandAmount);
            }
            return totalDoAmount;
        }
        private double GetTotalDueAmount(EmployeeDetailVm manager)
        {
            var totalDueAmount = 0.0;
            var managedEmployeeIdList = _employeeRepository.GetManagedEmployee(manager.Id).Select(x => x.Item1);
            var doList = _ppsDbContext.DemandOrder.Where(x => x.CreatedOn.Month != DateTime.Now.Month && managedEmployeeIdList.Contains(x.EmployeeId))
                .SelectMany(k => k.DemandOrderTransaction.Where(x => x.TransactionDate.Month == DateTime.Now.Month).Select(z => new { z.TransactionAmount })).ToList();
            if (doList.Count > 0)
            {
                totalDueAmount = doList.Sum(x => x.TransactionAmount);
            }
            return totalDueAmount;
        }
        private double GetTotalCurrentCollection(EmployeeDetailVm manager)
        {
            var totalCurrentAmount = 0.0;
            var managedEmployeeIdList = _employeeRepository.GetManagedEmployee(manager.Id).Select(x => x.Item1);

            var dolist = _ppsDbContext.DemandOrder.Where(x => x.CreatedOn.Month == DateTime.Now.Month && managedEmployeeIdList.Contains(x.EmployeeId))
                .SelectMany(k => k.DemandOrderTransaction.Select(z => z.TransactionAmount)).ToList();

            if (dolist.Count > 0)
            {
                totalCurrentAmount = dolist.Sum(x => x);
            }
            return totalCurrentAmount;
        }
        private List<SalesMonthlyHistoryVm> GetSalesHistoryMonthly(int employeeId, int year)
        {
            var salesHistoryMonthly =
                _ppsDbContext.EmployeeSalesTargetMonthly.Where(x => x.EmployeeId == employeeId && x.SalesYear <= year)
                .OrderByDescending(x => x.SalesYear)
                .ThenByDescending(x => x.SalesMonth)
                .ToList();
            var salesHistory = new List<SalesMonthlyHistoryVm>();
            salesHistoryMonthly.ForEach(x =>
            {
                salesHistory.Add(new SalesMonthlyHistoryVm
                {
                    Year = x.SalesYear,
                    Month = DateTimeFormatInfo.CurrentInfo?.GetAbbreviatedMonthName(x.SalesMonth),
                    SalesTarget = x.SalesTarget,
                    Achievement = (decimal)x.Achievement,
                    Percentage = (decimal)x.Percentage
                });
            });
            return salesHistory;
        }
        private List<DealerMonthlyHistoryVm> GetDealerMonthlyHistory(int employeeId, int year)
        {
            var customerList = _ppsDbContext.Customer.Where(x => x.EmployeeId == employeeId).Select(x => x.Id).ToList();
            var transaction = _ppsDbContext.CustomerTransactionMonthly
                .Where(x => x.TransactionMonth.Year == year && customerList.Contains(x.CustomerId))
                .OrderByDescending(x => x.TransactionMonth.Year)
                .ThenByDescending(x => x.TransactionMonth.Month)
                .ToList();
            var dealerMonthlyHistory = new List<DealerMonthlyHistoryVm>();
            transaction.ForEach(x =>
            {
                var customer = _ppsDbContext.Customer.FirstOrDefault(k => k.Id == x.CustomerId);
                dealerMonthlyHistory.Add(new DealerMonthlyHistoryVm
                {
                    DealerName = customer?.CustomerName,
                    DearlerCode = customer?.CustomerCode ?? 0,
                    Address = customer?.CustomerAddress,
                    Phone = customer?.CustomerPhone,
                    //TODO find out rating and risklevel of a customer
                    //Rating = 
                    //RiskLevel = 
                    TotalDoAmount = x.TotalDoAmount,
                    TotalPaidAmount = x.TotalPaidAmount,
                    TotalDueAmount = x.BalanceAmount
                });
            });
            return dealerMonthlyHistory;
        }
        private void DemandOrderCalculation(DemandOrder demandOrder)
        {
            var tUnitAmount = 0.0;
            var finalDiscount = 0.0;
            var subTotal = 0.0;

            var regularDiscount = demandOrder.RegularDiscountInPercentage ?? 0;
            var regularDiscountAmount = 0.0;
            var specialDiscount = demandOrder.SpecialDiscountInPercentage ?? 0;
            var additionDiscount = demandOrder.AdditionalDiscountInPercentage ?? 0;
            var extraDiscount = demandOrder.ExtraDiscountInPercentage ?? 0;
            var cashBackAmount = demandOrder.CashBackAmount ?? 0;

            if (demandOrder.DemandOrderDetail.Count >= 0)
            {
                foreach (var item in demandOrder.DemandOrderDetail)
                {
                    tUnitAmount = tUnitAmount + (item.Quantity * item.UnitPrice);
                }
            }
            var totalAmount = tUnitAmount;

            if (regularDiscount >= 0)
            {
                var rgdiscount = regularDiscount / 100;
                regularDiscountAmount = totalAmount * rgdiscount;
                demandOrder.RegularDiscountAmount = regularDiscountAmount;
                subTotal = totalAmount - regularDiscountAmount;
                finalDiscount = finalDiscount + regularDiscountAmount;
            }
            if (specialDiscount >= 0)
            {
                var spDiscount = specialDiscount / 100;
                var specialDiscountAmount = subTotal * spDiscount;
                demandOrder.SpecialDiscountAmount = specialDiscountAmount;
                finalDiscount = finalDiscount + specialDiscountAmount;
            }
            if (additionDiscount >= 0)
            {
                var adDiscount = additionDiscount / 100;
                var additionalDiscountAmount = subTotal * adDiscount;
                demandOrder.AdditionalDiscountAmount = additionalDiscountAmount;
                finalDiscount = finalDiscount + additionalDiscountAmount;
            }
            if (extraDiscount >= 0)
            {
                var exDiscount = extraDiscount / 100;
                var extraDiscountAmount = subTotal * exDiscount;
                demandOrder.ExtraDiscountAmount = extraDiscountAmount;
                finalDiscount = finalDiscount + extraDiscountAmount;
            }
            finalDiscount = finalDiscount + cashBackAmount;
            demandOrder.TotalDiscountAmount = finalDiscount;
            demandOrder.TotalGrandAmount = Math.Round(subTotal - finalDiscount + regularDiscountAmount);
        }
        private Tuple<string, string> GetUserNameById(int userId)
        {
            var userName = "";
            var designation = "";
            var user = _ppsDbContext.User.FirstOrDefault(x => x.Id == userId);
            if (user != null)
            {
                userName = StringExtension.ToFullName(user.FirstName, user.LastName);
                designation = _ppsDbContext.Employee.FirstOrDefault(x => x.Id == user.EmployeeId)?.Designation?
                    .DesignationName;
            }
            var tuple = Tuple.Create(userName, designation);

            return tuple;
        }
        private string GetDemandOrderEarlyPaymentTransactionStatus(DemandOrderDiscountTransaction demandOrderDiscountTransaction)
        {
            var status = "Pending";
            if (demandOrderDiscountTransaction == null)
            {

            }
            else if (demandOrderDiscountTransaction.IsApproved == true)
            {
                status = "Approved";
            }
            else if (demandOrderDiscountTransaction.IsVerified == true)
            {
                status = "Verified";
            }
            return status;
        }
        public Task<bool> ProcessMonthlyAchievement1(MonthlyProcessing newMonthlyProcessing, List<EmployeeSalesTargetMonthly> monthlySalesTarget, bool reprocess, int year, int month, int userId)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Invoice Return Entry

        public InvoiceReturn SaveReturnInvoice(InvoiceReturn invoiceReturn)
        {
            using (var db = new PPSDbContext())
            {
                db.InvoiceReturn.Add(invoiceReturn);
                db.SaveChanges();

                return invoiceReturn;
            }
        }

        public InvoiceReturn GetInvoiceReturnById(int id)
        {
            return _ppsDbContext.InvoiceReturn.Where(m => m.Id == id).FirstOrDefault();
        }

        public InvoiceReturn UpdateReturnInvoice(InvoiceReturn invoiceReturn)
        {
            using (var db = new PPSDbContext())
            {

                db.InvoiceReturn.Attach(invoiceReturn);
                db.Entry(invoiceReturn).State = EntityState.Modified;
                db.SaveChanges();
                return invoiceReturn;
            }
        }
        public InvoiceReturn ApproveReturnInvoice(InvoiceReturnVm invoiceReturnVm)
        {
            using (var db = new PPSDbContext())
            {
                var invoiceReturn = db.InvoiceReturn.Where(m => m.Id == invoiceReturnVm.Id).FirstOrDefault();
                if (invoiceReturn == null)
                {
                    throw new KeyNotFoundException($"This return invoice no. {invoiceReturn.Id} doesn't exist.");
                }
                if (invoiceReturn.ApprovedBy != null)
                {
                    throw new Exception($"The return invoice no: {invoiceReturn.Id} has already been approved.");
                }
                invoiceReturn.ApprovedBy = invoiceReturnVm.ApprovedBy;
                invoiceReturn.ApprovedOn = invoiceReturnVm.ApprovedOn;


                List<CurrentProductStock> productStock = new List<CurrentProductStock>();
                foreach (var item in invoiceReturn.InvoiceReturnDetail)
                {
                    var currentStock = db.CurrentProductStock.Where(m => m.ProductId == item.ProductId).FirstOrDefault();
                    if (currentStock != null)
                    {
                        currentStock.DeliveredQuantity = currentStock.DeliveredQuantity - item.Quantity;
                        currentStock.AvailableQuantity = currentStock.AvailableQuantity + item.Quantity;
                    };
                    db.CurrentProductStock.Attach(currentStock);
                    db.Entry(currentStock).State = EntityState.Modified;
                }

                var customerAccountHeadId = invoiceReturn.Invoice.DemandOrder.Customer.AccountHeadId;
                var salesReturnAccountHeadId = db.ReferenceTable.Where(m => m.Id == (int)ReferenceTableEnum.SalesReturnAccountHeadId).Select(m => m.ReferenceValue).FirstOrDefault();
                if (customerAccountHeadId == null)
                {
                    throw new KeyNotFoundException("Customer account head doesn't exist.");
                }
                // Hit the accounting head for this dealer
                var lastTran = _ppsDbContext.TransactionEntry
                .Where(x => x.TransactionTypeId == (int)TransactionTypeEnum.SalesReturn
                            && x.FiscalYear == DateTime.Now.Year
                            && x.CompanyId == invoiceReturnVm.CompanyId)
                .OrderByDescending(x => x.TransactionNumber.Substring(9, 4))
                .FirstOrDefault();

                var lastNumber = 0;
                if (lastTran != null)
                {
                    lastNumber = int.Parse(lastTran.TransactionNumber.Substring(9, 4));
                }
                var transactionNumber = _transactionRepository.CreateTransactionNumber((int)TransactionTypeEnum.SalesReturn, DateTime.Now, lastNumber + 1);

                // Dealer(Cr), Sales Return(Dr) for Company
                var systemTransaction = new TransactionEntry
                {
                    IsSystemGenerated = true,
                    TransactionNumber = transactionNumber,
                    TransactionDate = (DateTime)invoiceReturnVm.ApprovedOn,
                    FiscalYear = DateTime.Now.Year,
                    TransactionTypeId = (int)TransactionTypeEnum.SalesReturn,
                    CompanyId = (int)invoiceReturnVm.CompanyId,
                    PostingDate = (DateTime)invoiceReturnVm.ApprovedOn,
                    Active = true,
                    Deleted = false,
                    Accepted = false,
                    CreatedById = (int)invoiceReturnVm.ApprovedBy,
                    CreatedDate = (DateTime)invoiceReturnVm.ApprovedOn,

                };
                var sysTranDetailList = new List<TransactionDetail>();

                sysTranDetailList.Add(new TransactionDetail
                {
                    AccountHeadId = Convert.ToInt32(salesReturnAccountHeadId),
                    CrAmount = 0,
                    DrAmount = (double)invoiceReturn.TotalGrandAmount,
                    TransactionEntryId = systemTransaction.Id
                });
                sysTranDetailList.Add(new TransactionDetail
                {
                    AccountHeadId = Convert.ToInt32(customerAccountHeadId),
                    CrAmount = (double)invoiceReturn.TotalGrandAmount,
                    DrAmount = 0,
                    TransactionEntryId = systemTransaction.Id
                });
                var totalDrAmount = db.TransactionDetail.Sum(x => x.DrAmount);
                var totalCrAmount = db.TransactionDetail.Sum(x => x.CrAmount);
                if (totalDrAmount == 0 || totalDrAmount != totalCrAmount)
                {
                    throw new Exception("The amount of debit and credit is not matched.");
                }


                db.TransactionEntry.Add(systemTransaction);
                invoiceReturn.TransactionEntryId = systemTransaction.Id;
                systemTransaction.TransactionDetail = sysTranDetailList;
                db.InvoiceReturn.Attach(invoiceReturn);
                db.Entry(invoiceReturn).State = EntityState.Modified;

                //db.SaveChanges();

                return invoiceReturn;
            }
        }
        #endregion

        #region Delivery Challan Start
        public IQueryable<DeliveryQuantity> InvoiceDeliveryChallanList()
        {
            return _ppsDbContext.DeliveryQuantity;
        }

        public DeliveryQuantity DeliveryQuantitySave(DeliveryQuantity deliveryQuantity)
        {
            using (var db = new PPSDbContext())
            {
                db.DeliveryQuantity.Add(deliveryQuantity);
                db.SaveChanges();
                return deliveryQuantity;
            }
        }
        public DeliveryQuantity GetDeliveryQuantityById(int id)
        {
            return _ppsDbContext.DeliveryQuantity.Where(m => m.Id == id).FirstOrDefault();
        }
        public DeliveryQuantity DeliveryQuantityUpdate(DeliveryQuantity deliveryQuantity)
        {
            using (var db = new PPSDbContext())
            {
                db.DeliveryQuantity.Attach(deliveryQuantity);
                db.Entry(deliveryQuantity).State = EntityState.Modified;
                db.SaveChanges();
                return deliveryQuantity;
            }
        }

        public DeliveryQuantity ApproveDeliveryQuantityById(DeliveryQuantity deliveryQuantity)
        {
            using (var db = new PPSDbContext())
            {
                db.DeliveryQuantity.Attach(deliveryQuantity);
                db.Entry(deliveryQuantity).State = EntityState.Modified;
                db.SaveChanges();
                return deliveryQuantity;
            }
        }
        public DeliveryQuantity GetUndeliveryQuantityById(int id)
        {
            return _ppsDbContext.DeliveryQuantity.Where(m => m.Id == id).FirstOrDefault();
        }

        public Invoice InvoiceDetailsByIdForDeliveryChallan(int id)
        {
            return _ppsDbContext.Invoice.Where(m => m.Id == id).FirstOrDefault();
        }
        #endregion Delivery Challan end
    }
}

