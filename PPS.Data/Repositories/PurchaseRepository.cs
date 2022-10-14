using System;
using PPS.Data.RepositoryInterfaces;
using System.Collections.Generic;
using System.Linq;
using PPS.Data.Edmx;
using System.Collections.Concurrent;
using System.Data.Entity;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using PPS.API.Shared.Enums;
using PPS.API.Shared.Extensions;
using PPS.API.Shared.RequestVm;
using PPS.API.Shared.ReturnVm;
using PPS.API.Shared.ViewModel.Purchase;
using PPS.API.Shared.ViewModel.User;


namespace PPS.Data.Repositories
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly PPSDbContext _ppsDbContext;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IReportRepository _reportRepository;

        public PurchaseRepository()
        {
            _ppsDbContext = new PPSDbContext();
            _transactionRepository = new TransactionRepository();
            _reportRepository = new ReportRepository();
        }

        public IList<PurchaseOrderVm> GetPurchaseOrderList(int userId)
        {
            var poList = _ppsDbContext.PurchaseOrder.ToList();

            var poVm = new ConcurrentBag<PurchaseOrderVm>();

            poList.ForEach(p =>
            {
                var totalAmount = p.TotalAmount;
                var paidAmount = p.PurchaseOrderTransaction?.Where(x => x.PurchaseOrderId == p.Id)
                    .ToList().Sum(x => x.TransactionAmount) ?? 0;

                var paidCashAmount = p.CashAmount ?? 0;
                var paidBankAmount = p.BankAmount ?? 0;
                var paidCashBankAmountTotal = paidCashAmount + paidBankAmount + paidAmount;
                var balanceAmount = totalAmount - paidCashBankAmountTotal;

                poVm.Add(new PurchaseOrderVm
                {
                    POId = p.Id,
                    PurchaseOrderNo = p.PurchaseOrderNo,
                    PurchaseOrderDate = p.PurchaseOrderDate,
                    SupplierName = p.Supplier?.SupplierName,
                    Note = p.Note,
                    PaymentType = p.PaymentType,
                    EstimatedDeliveryDate = p.EstimatedDeliveryDate,
                    PriceValidity = p.PriceValidity,
                    TotalAmount = totalAmount,
                    PaidAmount = paidCashBankAmountTotal,
                    BalanceAmount = balanceAmount,
                    POStatusName = p.PurchaseOrderStatus.Status,
                    CreatedByName = StringExtension.ToFullName(p.User.FirstName, p.User.LastName),
                    CreatedDate = p.CreatedOn
                });
            });
            return poVm.ToList();
        }

        public IList<SupplierVm> GetSupplierList(int userId)
        {
            var supplierList = (from supplier in _ppsDbContext.Supplier
                                select new SupplierVm
                                {
                                    Id = supplier.Id,
                                    SupplierName = supplier.SupplierName,
                                    Address = supplier.Address,
                                    ContactPerson = supplier.ContactPerson,
                                    Phone = supplier.Phone,
                                    ContactPersonPhone = supplier.ContactPersonPhone,
                                    Email = supplier.Email,
                                    ContactPersonEmail = supplier.ContactPersonEmail,
                                    AccountHeadId = supplier.AccountHeadId
                                }).ToList();
            return supplierList.ToList();
        }

        public IList<RawMaterialTypeVm> GetRawMaterialType(int userId)
        {
            var rawMaterialType = (from rawMaterial in _ppsDbContext.RawMaterialType
                                   select new RawMaterialTypeVm
                                   {
                                       Id = rawMaterial.Id,
                                       RawMaterialTypeName = rawMaterial.RawMaterialTypeName,
                                       UnitTypeId = rawMaterial.UnitTypeId,
                                       UnitTypeName = rawMaterial.UnitType.UnitTypeName,
                                       AccountHeadId = rawMaterial.AccountHeadId
                                   }).ToList();
            return rawMaterialType.ToList();
        }

        public PurchaseOrderModel SavePurchaseOrder(PurchaseOrderModel purchaseOrderEntry)
        {
            var lastPurchaseOrder = _ppsDbContext.PurchaseOrder
                .OrderByDescending(x => x.PurchaseOrderNo)
                .FirstOrDefault();

            var lastNumber = 0;
            if (lastPurchaseOrder != null)
            {
                lastNumber = lastPurchaseOrder.PurchaseOrderNo;
            }
            purchaseOrderEntry.PurchaseOrderNo = lastNumber + 1;

            var poEntry = new PurchaseOrder
            {
                PurchaseOrderNo = purchaseOrderEntry.PurchaseOrderNo,
                PurchaseOrderDate = purchaseOrderEntry.PurchaseOrderDate,
                IsCashPurchase = purchaseOrderEntry.IsCashPurchase,
                IsCreditPurchase = purchaseOrderEntry.IsCreditPurchase,
                IsLcPurchase = purchaseOrderEntry.IsLcPurchase,

                Note = purchaseOrderEntry.Note,
                PaymentType = purchaseOrderEntry.PaymentType,
                EstimatedDeliveryDate = purchaseOrderEntry.EstimatedDeliveryDate,
                PriceValidity = purchaseOrderEntry.PriceValidity,
                TotalAmount = purchaseOrderEntry.TotalAmount,


                CreatedBy = purchaseOrderEntry.CreatedBy,
                CreatedOn = purchaseOrderEntry.CreatedOn,
                PurchaseOrderStatusId = (int)PurchaseOrderStatusEnum.Initiated,
                IsCurrentRecord = true,

                CashAccountHeadId = purchaseOrderEntry.CashAccountHeadId,
                CashAmount = purchaseOrderEntry.CashAmount,
                BankAccountHeadId = purchaseOrderEntry.BankAccountHeadId,
                BankAmount = purchaseOrderEntry.BankAmount,
                SupplierId = purchaseOrderEntry.SupplierId,
                SupplierAccountHeadId = purchaseOrderEntry.SupplierAccountHeadId,
                SupplierAmount = purchaseOrderEntry.SupplierAmount,
                LCNo = purchaseOrderEntry.LCNo,
                LCAccountHeadId = purchaseOrderEntry.LCAccountHeadId,
                LCAmount = purchaseOrderEntry.LCAmount,
                IsPaymentComplete = false,

                PurchaseOrderDetail = new List<PurchaseOrderDetail>()
            };


            foreach (var tempPoEntry in purchaseOrderEntry.PurchaseOrderDetail)
            {
                poEntry.PurchaseOrderDetail.Add(
                    new PurchaseOrderDetail
                    {
                        PurchaseOrderId = poEntry.Id,
                        RawMaterialTypeId = tempPoEntry.RawMaterialTypeId,
                        Quantity = tempPoEntry.Quantity,
                        Price = tempPoEntry.Price,
                        AccountHeadId = tempPoEntry.AccountHeadId
                    });
            }

            _ppsDbContext.PurchaseOrder.Add(poEntry);
            _ppsDbContext.SaveChanges();
            purchaseOrderEntry.Id = poEntry.Id;
            return purchaseOrderEntry;
        }

        public PurchaseOrderModel GetPurchaseOrderById(int userId, int poId)
        {
            var purchaseOrder = _ppsDbContext.PurchaseOrder.FirstOrDefault(x => x.Id == poId && x.IsCurrentRecord);

            if (purchaseOrder == null)
            {
                throw new KeyNotFoundException($"The purchase order no. {poId} doesn't exist.");
            }
            var purchaseOrderDetail = purchaseOrder.PurchaseOrderDetail.ToList();

            var purchaseOrderDetailVm = new List<PurchaseOrderDetailModel>();
            purchaseOrderDetail.ForEach(p =>
            {
                purchaseOrderDetailVm.Add(new PurchaseOrderDetailModel
                {
                    Id = p.Id,
                    RawMaterialTypeId = p.RawMaterialTypeId,
                    RawMaterialTypeName = p.RawMaterialType?.RawMaterialTypeName,
                    UnitTypeName = p.RawMaterialType?.UnitType.UnitTypeName,
                    Quantity = p.Quantity,
                    Price = p.Price,
                    TotalUnitPrice = p.Quantity * p.Price
                });
            });

            var totalAmount = purchaseOrder.TotalAmount;
            var paidAmount = purchaseOrder.PurchaseOrderTransaction?.Where(x => x.PurchaseOrderId == poId)
                .ToList().Sum(x => x.TransactionAmount) ?? 0;

            var paidCashAmount = purchaseOrder.CashAmount ?? 0;
            var paidBankAmount = purchaseOrder.BankAmount ?? 0;
            var paidCashBankAmountTotal = paidCashAmount + paidBankAmount;
            var balanceAmount = totalAmount - paidAmount - paidCashBankAmountTotal;

            var purchaseOrderVm = new PurchaseOrderModel
            {
                Id = purchaseOrder.Id,
                PurchaseOrderNo = purchaseOrder.PurchaseOrderNo,
                PurchaseOrderDate = purchaseOrder.PurchaseOrderDate,
                SupplierId = purchaseOrder.SupplierId ?? 0,
                SupplierName = purchaseOrder.Supplier?.SupplierName,
                PurchaseOrderStatusId = purchaseOrder.PurchaseOrderStatusId,
                POStatusName = purchaseOrder.PurchaseOrderStatus.Status,
                TotalAmount = totalAmount,
                PaidAmount = paidAmount + paidCashBankAmountTotal,
                BalanceAmount = balanceAmount,
                Note = purchaseOrder.Note,
                PaymentType = purchaseOrder.PaymentType,
                EstimatedDeliveryDate = purchaseOrder.EstimatedDeliveryDate,
                PriceValidity = purchaseOrder.PriceValidity,
                PurchaseOrderDetail = purchaseOrderDetailVm,
                VerifiedByOn = purchaseOrder.VerifiedBy != null && purchaseOrder.VerifiedOn != null
                    ? GetUserByOn((int)purchaseOrder.VerifiedBy, (DateTime)purchaseOrder.VerifiedOn)
                    : "-",
                ApprovedByOn = purchaseOrder.ApprovedBy != null && purchaseOrder.ApprovedOn != null
                    ? GetUserByOn((int)purchaseOrder.ApprovedBy, (DateTime)purchaseOrder.ApprovedOn)
                    : "-",
                RejectedByOn = purchaseOrder.RejectedBy != null && purchaseOrder.RejectedOn != null
                    ? GetUserByOn((int)purchaseOrder.RejectedBy, (DateTime)purchaseOrder.RejectedOn)
                    : "-"
            };
            return purchaseOrderVm;
        }
        public PurchaseOrderModel UpdatePurchaseOrder(PurchaseOrderModel purchaseOrderEntryVm)
        {
            using (_ppsDbContext)
            {
                var poEntry = _ppsDbContext.PurchaseOrder.FirstOrDefault(x => x.Id == purchaseOrderEntryVm.Id);
                if (poEntry == null)
                {
                    throw new KeyNotFoundException($"Purchase Order Id {purchaseOrderEntryVm.Id} does not exist.");
                }

                poEntry.PurchaseOrderDate = purchaseOrderEntryVm.PurchaseOrderDate;
                poEntry.SupplierId = purchaseOrderEntryVm.SupplierId;
                poEntry.Note = purchaseOrderEntryVm.Note;
                poEntry.PaymentType = purchaseOrderEntryVm.PaymentType;
                poEntry.EstimatedDeliveryDate = purchaseOrderEntryVm.EstimatedDeliveryDate;
                poEntry.PriceValidity = purchaseOrderEntryVm.PriceValidity;

                var poEntryDetail = poEntry.PurchaseOrderDetail;
                poEntry.PurchaseOrderDetail = new List<PurchaseOrderDetail>();

                foreach (var poDetail in purchaseOrderEntryVm.PurchaseOrderDetail)
                {
                    poEntry.PurchaseOrderDetail.Add(
                        new PurchaseOrderDetail
                        {
                            RawMaterialTypeId = poDetail.RawMaterialTypeId,
                            Quantity = poDetail.Quantity,
                            Price = poDetail.Price
                        });
                }
                _ppsDbContext.PurchaseOrderDetail.RemoveRange(poEntryDetail);
                _ppsDbContext.PurchaseOrder.Attach(poEntry);
                _ppsDbContext.Entry(poEntry).State = EntityState.Modified;
                _ppsDbContext.SaveChanges();
                purchaseOrderEntryVm.Id = poEntry.Id;
                return purchaseOrderEntryVm;
            }
        }

        public async Task<bool> VerifyPO(int poId, int userId)
        {
            var purchaseOrder = _ppsDbContext.PurchaseOrder.FirstOrDefault(x => x.Id == poId);
            if (purchaseOrder == null)
            {
                throw new Exception($"This purchase order no: {poId} doesn't exist.");
            }
            if (purchaseOrder.VerifiedBy != null)
            {
                throw new Exception($"This purchase order no: {poId} has already been verified.");
            }
            purchaseOrder.VerifiedBy = userId;
            purchaseOrder.VerifiedOn = DateTime.Now;
            purchaseOrder.IsVerified = true;
            purchaseOrder.PurchaseOrderStatusId = (int)PurchaseOrderStatusEnum.Verified;
            _ppsDbContext.PurchaseOrder.Attach(purchaseOrder);
            _ppsDbContext.Entry(purchaseOrder).State = EntityState.Modified;
            await _ppsDbContext.SaveChangesAsync();
            return true;
        }

        public ReturnVm ApprovePO(PurchaseOrderWithSupplierRequestVm purchaseOrderWithSupplierRequestVm)
        {
            using (var db = new PPSDbContext())
            {
                var returnVm = new ReturnVm
                {
                    Success = true
                };

                var purchaseOrder = db.PurchaseOrder.FirstOrDefault(x => x.Id == purchaseOrderWithSupplierRequestVm.PurchaseOrderId);
                if (purchaseOrder == null)
                {
                    throw new Exception($"This purchase order no: {purchaseOrderWithSupplierRequestVm.PurchaseOrderId} doesn't exist.");
                }

                if (purchaseOrder.IsVerified != true)
                {
                    throw new Exception($"This purchase order no: {purchaseOrderWithSupplierRequestVm.PurchaseOrderId} has already been verified.");
                }

                if (purchaseOrder.ApprovedBy != null)
                {
                    throw new Exception($"This purchase order no: {purchaseOrderWithSupplierRequestVm.PurchaseOrderId} has already been approved.");
                }

                //Check account balance to make this transaction
                var fiscalYearDetail = db.FiscalYear.FirstOrDefault(x => x.Year == purchaseOrderWithSupplierRequestVm.FiscalYear);
                if (fiscalYearDetail == null)
                {
                    throw new KeyNotFoundException($"The fiscal year {purchaseOrderWithSupplierRequestVm.FiscalYear} doesn't exist.");
                }

                var openDate = fiscalYearDetail.OpenDate;
                var closeDate = fiscalYearDetail.CloseDate;

                if (purchaseOrder.IsCashPurchase == true)
                {
                    if (purchaseOrder.CashAmount > 0)
                    {
                        var getCashAccountBalance = _reportRepository.GetIndividualLedger(purchaseOrderWithSupplierRequestVm.FiscalYear, purchaseOrderWithSupplierRequestVm.CompanyId, purchaseOrder.CashAccountHeadId ?? 0, openDate, closeDate);
                        var drCashAmount = getCashAccountBalance.ClosingBalanceDr;
                        var crCashAmount = getCashAccountBalance.ClosingBalanceCr;
                        var balanceCash = drCashAmount - crCashAmount;
                        if (balanceCash < purchaseOrder.CashAmount)
                        {
                            returnVm.Success = false;
                            returnVm.Message = "Sorry! your cash account balance is not sufficient";
                            return returnVm;
                        }
                    }
                    if (purchaseOrder.BankAmount > 0)
                    {
                        var getBankAccountBalance = _reportRepository.GetIndividualLedger(purchaseOrderWithSupplierRequestVm.FiscalYear, purchaseOrderWithSupplierRequestVm.CompanyId, purchaseOrder.BankAccountHeadId ?? 0, openDate, closeDate);
                        var drBankAmount = getBankAccountBalance.ClosingBalanceDr;
                        var crBankAmount = getBankAccountBalance.ClosingBalanceCr;
                        var balanceBank = drBankAmount - crBankAmount;
                        if (balanceBank < purchaseOrder.BankAmount)
                        {
                            returnVm.Success = false;
                            returnVm.Message = "Sorry! your bank account balance is not sufficient";
                            return returnVm;
                        }
                    }
                }

                var txType = purchaseOrder.IsCashPurchase == true ? (int)TransactionTypeEnum.Payment : purchaseOrder.IsCreditPurchase == true ? (int)TransactionTypeEnum.Purchase : (int)TransactionTypeEnum.Purchase;

                // Hit the accounting head for this supplier
                var lastTran = _ppsDbContext.TransactionEntry
                .Where(x => x.TransactionTypeId == txType
                            && x.FiscalYear == purchaseOrderWithSupplierRequestVm.FiscalYear
                            && x.CompanyId == purchaseOrderWithSupplierRequestVm.CompanyId)
                .OrderByDescending(x => x.TransactionNumber.Substring(9, 4))
                .FirstOrDefault();

                var lastNumber = 0;
                if (lastTran != null)
                {
                    lastNumber = int.Parse(lastTran.TransactionNumber.Substring(9, 4));
                }
                var transactionNumber = _transactionRepository.CreateTransactionNumber(txType, purchaseOrderWithSupplierRequestVm.DatedOn, lastNumber + 1);

                // Paid to Supplier from Cash/Bank -> Supplier(Dr), Cash / Bank(Cr)
                var systemTransaction = new TransactionEntry
                {
                    IsSystemGenerated = true,
                    TransactionNumber = transactionNumber,
                    TransactionDate = DateTime.Now,
                    FiscalYear = purchaseOrderWithSupplierRequestVm.FiscalYear,
                    TransactionTypeId = txType,
                    CompanyId = purchaseOrderWithSupplierRequestVm.CompanyId,
                    PostingDate = DateTime.Now,
                    Active = true,
                    Deleted = false,
                    CreatedById = purchaseOrder.CreatedBy,
                    CreatedDate = purchaseOrder.CreatedOn,
                    VerifiedById = purchaseOrder.VerifiedBy,
                    VerifiedDate = purchaseOrder.VerifiedOn,
                    Accepted = true,
                    AcceptedById = purchaseOrderWithSupplierRequestVm.UserId,
                    AcceptedDate = purchaseOrderWithSupplierRequestVm.DatedOn,
                    TransactionDetail = new List<TransactionDetail>()
                };

                purchaseOrder.PurchaseOrderDetail.ToList().ForEach(x =>
                systemTransaction.TransactionDetail.Add(new TransactionDetail
                {
                    AccountHeadId = x.RawMaterialType.AccountHeadId,
                    DrAmount = x.Price * (double)x.Quantity ?? 0,
                    CrAmount = 0
                }));

                if (purchaseOrder.IsCashPurchase == true)
                {
                    if (purchaseOrder.CashAccountHeadId != null)
                    {
                        systemTransaction.TransactionDetail.Add(
                            new TransactionDetail
                            {
                                AccountHeadId = purchaseOrder.CashAccountHeadId ?? 0,
                                CrAmount = purchaseOrder.CashAmount ?? 0,
                                DrAmount = 0
                            });
                    }
                    if (purchaseOrder.BankAccountHeadId != null)
                    {
                        systemTransaction.TransactionDetail.Add(
                             new TransactionDetail
                             {
                                 AccountHeadId = purchaseOrder.BankAccountHeadId ?? 0,
                                 CrAmount = purchaseOrder.BankAmount ?? 0,
                                 DrAmount = 0
                             });
                    }
                    if (purchaseOrder.SupplierAccountHeadId != null)
                    {
                        systemTransaction.TransactionDetail.Add(
                             new TransactionDetail
                             {
                                 AccountHeadId = purchaseOrder.SupplierAccountHeadId ?? 0,
                                 CrAmount = purchaseOrder.SupplierAmount ?? 0,
                                 DrAmount = 0
                             });

                    }
                }
                else if (purchaseOrder.IsCreditPurchase == true)
                {
                    systemTransaction.TransactionDetail = new List<TransactionDetail>
                    {
                        new TransactionDetail
                        {
                            AccountHeadId = purchaseOrder.SupplierAccountHeadId ?? 0,
                            CrAmount = purchaseOrder.SupplierAmount ?? 0,
                            DrAmount = 0
                        }
                    };
                }
                else if (purchaseOrder.IsLcPurchase == true)
                {
                    systemTransaction.TransactionDetail = new List<TransactionDetail>
                    {
                        new TransactionDetail
                        {
                            AccountHeadId = purchaseOrder.LCAccountHeadId ?? 0,
                            CrAmount = purchaseOrder.LCAmount ?? 0,
                            DrAmount = 0
                        }
                    };
                }

                db.TransactionEntry.Add(systemTransaction);

                purchaseOrder.ApprovedBy = purchaseOrderWithSupplierRequestVm.UserId;
                purchaseOrder.ApprovedOn = purchaseOrderWithSupplierRequestVm.DatedOn;
                purchaseOrder.IsApproved = true;
                purchaseOrder.PurchaseOrderStatusId = (int)PurchaseOrderStatusEnum.Approved;

                db.PurchaseOrder.Attach(purchaseOrder);
                db.Entry(purchaseOrder).State = EntityState.Modified;

                db.SaveChanges();

                purchaseOrder.TransactionEntryId = systemTransaction.Id;
                db.PurchaseOrder.Attach(purchaseOrder);
                db.Entry(purchaseOrder).State = EntityState.Modified;

                db.SaveChanges();

                return returnVm;
            }


        }

        public SupplierVm GetSupplierById(PurchaseOrderWithSupplierRequestVm requestVm)
        {
            if (requestVm.SupplierId == 0)
            {
                var supplierVm1 = new SupplierVm
                {

                };
                var purchaseOrderTransactionVm1 = new ConcurrentBag<PurchaseOrderTransactionVm>();

                var purchaseOrder1 = _ppsDbContext.PurchaseOrder.FirstOrDefault(x => x.Id == requestVm.PurchaseOrderId);
                if (purchaseOrder1.IsCashPurchase == true)
                {
                    if (purchaseOrder1.CashAccountHeadId != null)
                    {
                        purchaseOrderTransactionVm1.Add(new PurchaseOrderTransactionVm
                        {
                            Id = purchaseOrder1.Id,
                            AccountName = purchaseOrder1.AccountHead1?.AccountHeadName,
                            AccountCode = purchaseOrder1.AccountHead1?.AccountHeadCode,
                            CreatedByName = GetUserName(purchaseOrder1.CreatedBy),
                            CreatedOn = purchaseOrder1.CreatedOn,
                            TransactionAmount = purchaseOrder1.CashAmount ?? 0,
                            TransactionDate = purchaseOrder1.PurchaseOrderDate,
                            IsApproved = true
                        });
                    }
                    if (purchaseOrder1.BankAccountHeadId != null)
                    {
                        purchaseOrderTransactionVm1.Add(new PurchaseOrderTransactionVm
                        {
                            Id = purchaseOrder1.Id,
                            AccountName = purchaseOrder1.AccountHead?.AccountHeadName,
                            AccountCode = purchaseOrder1.AccountHead?.AccountHeadCode,
                            CreatedByName = GetUserName(purchaseOrder1.CreatedBy),
                            CreatedOn = purchaseOrder1.CreatedOn,
                            TransactionAmount = purchaseOrder1.BankAmount ?? 0,
                            TransactionDate = purchaseOrder1.PurchaseOrderDate,
                            IsApproved = true
                        });
                    }
                }
                supplierVm1.PurchaseOrderTransaction = purchaseOrderTransactionVm1.ToList();
                return supplierVm1;
            }
            else
            {

                var supplier = _ppsDbContext.Supplier.FirstOrDefault(x => x.Id == requestVm.SupplierId && x.IsActive);
                if (supplier == null)
                {
                    throw new KeyNotFoundException($"Supplier {requestVm.SupplierId} does not exist.");
                }

                var supplierVm = new SupplierVm
                {
                    Id = supplier.Id,
                    SupplierName = supplier.SupplierName,
                    SupplierCode = supplier.SupplierCode
                };
                var purchaseOrderTransactionVm = new ConcurrentBag<PurchaseOrderTransactionVm>();

                var purchaseOrder = _ppsDbContext.PurchaseOrder.FirstOrDefault(x => x.Id == requestVm.PurchaseOrderId);
                if (purchaseOrder.IsCashPurchase == true)
                {
                    if (purchaseOrder.CashAccountHeadId != null)
                    {
                        purchaseOrderTransactionVm.Add(new PurchaseOrderTransactionVm
                        {
                            Id = purchaseOrder.Id,
                            AccountName = purchaseOrder.AccountHead1?.AccountHeadName,
                            AccountCode = purchaseOrder.AccountHead1?.AccountHeadCode,
                            CreatedByName = GetUserName(purchaseOrder.CreatedBy),
                            CreatedOn = purchaseOrder.CreatedOn,
                            TransactionAmount = purchaseOrder.CashAmount ?? 0,
                            TransactionDate = purchaseOrder.PurchaseOrderDate,
                            IsApproved = true
                        });
                    }
                    if (purchaseOrder.BankAccountHeadId != null)
                    {
                        purchaseOrderTransactionVm.Add(new PurchaseOrderTransactionVm
                        {
                            Id = purchaseOrder.Id,
                            AccountName = purchaseOrder.AccountHead?.AccountHeadName,
                            AccountCode = purchaseOrder.AccountHead?.AccountHeadCode,
                            CreatedByName = GetUserName(purchaseOrder.CreatedBy),
                            CreatedOn = purchaseOrder.CreatedOn,
                            TransactionAmount = purchaseOrder.BankAmount ?? 0,
                            TransactionDate = purchaseOrder.PurchaseOrderDate,
                            IsApproved = true
                        });
                    }

                }

                supplier.PurchaseOrderTransaction.Where(x => x.PurchaseOrderId == requestVm.PurchaseOrderId).ToList().ForEach(tran =>
                {
                    purchaseOrderTransactionVm.Add(new PurchaseOrderTransactionVm
                    {
                        Id = tran.Id,
                        AccountName = tran.AccountHead?.AccountHeadName,
                        AccountCode = tran.AccountHead?.AccountHeadCode,
                        CreatedByName = GetUserName(tran.CreatedBy),
                        CreatedOn = tran.CreatedOn,
                        TransactionAmount = tran.TransactionAmount,
                        TransactionDate = tran.TransactionDate,
                        IsApproved = tran.IsApproved
                    });
                });
                supplierVm.PurchaseOrderTransaction = purchaseOrderTransactionVm.ToList();
                return supplierVm;
            }
        }

        //public double GetAvailableAmountBySupplierId(int supplierId)
        //{
        //    var amount = 0d;
        //    var supplierTotalPaid = _ppsDbContext.PurchaseOrderTransaction.Where(x => x.SupplierId == supplierId && x.IsApproved == true).ToList().Sum(x => x.TransactionAmount);
        //    var supplierTotalTransaction = _ppsDbContext.PurchaseOrder.Where(x => x.SupplierId == supplierId && x.IsCurrentRecord == true).ToList().SelectMany(x => x.PurchaseOrderTransaction).ToList().Sum(x => x.TransactionAmount);
        //    amount = supplierTotalPaid - supplierTotalTransaction;
        //    return amount;
        //}

        public async Task<bool> SavePurchaseOrderTransaction(int userId, PurchaseOrderTransactionVm purchaseOrderTransactionVm)
        {
            var purchaseOrderTransaction = new PurchaseOrderTransaction
            {
                PurchaseOrderId = purchaseOrderTransactionVm.PurchaseOrderId,
                SupplierId = purchaseOrderTransactionVm.SupplierId,
                CashBankAccountHeadId = purchaseOrderTransactionVm.CashBankAccountHeadId,
                TransactionAmount = purchaseOrderTransactionVm.TransactionAmount,
                TransactionDate = purchaseOrderTransactionVm.TransactionDate,
                CreatedBy = purchaseOrderTransactionVm.CreatedBy,
                CreatedOn = purchaseOrderTransactionVm.CreatedOn,
                IsApproved = false
            };
            _ppsDbContext.PurchaseOrderTransaction.Add(purchaseOrderTransaction);
            await _ppsDbContext.SaveChangesAsync();
            return true;
        }

        public ReturnVm ApprovePurchaseOrderTransaction(UserVm userVm, PurchaseOrderTransactionVm purchaseOrderTransactionVm, int fiscalYear)
        {
            using (var db = new PPSDbContext())
            {
                var returnVm = new ReturnVm
                {
                    Success = true
                };

                var purchaseOrderTran = db.PurchaseOrderTransaction.FirstOrDefault(x => x.Id == purchaseOrderTransactionVm.Id);
                if (purchaseOrderTran == null)
                {
                    throw new KeyNotFoundException($"Purchase transaction {purchaseOrderTransactionVm.Id} doesn't exist");
                }

                if (purchaseOrderTran.ApprovedBy != null)
                {
                    throw new Exception($"This purchase order transaction no: {purchaseOrderTransactionVm.Id} has already been approved.");
                }

                //Check account balance to make this transaction
                var fiscalYearDetail = db.FiscalYear.FirstOrDefault(x => x.Year == fiscalYear);
                if (fiscalYearDetail == null)
                {
                    throw new KeyNotFoundException($"The fiscal year {fiscalYear} doesn't exist.");
                }

                var openDate = fiscalYearDetail.OpenDate;
                var closeDate = fiscalYearDetail.CloseDate;

                var getIndividualLedgerBalance = _reportRepository.GetIndividualLedger(fiscalYear, userVm.CompanyId, purchaseOrderTran.CashBankAccountHeadId, openDate, closeDate);
                var drAmount = getIndividualLedgerBalance.ClosingBalanceDr;
                var crAmount = getIndividualLedgerBalance.ClosingBalanceCr;
                var balance = drAmount - crAmount;
                if (balance < purchaseOrderTransactionVm.TransactionAmount)
                {
                    returnVm.Success = false;
                    returnVm.Message = "Sorry! your account balance is not sufficient";
                    return returnVm;
                }

                purchaseOrderTran.ApprovedBy = purchaseOrderTransactionVm.ApprovedBy;
                purchaseOrderTran.ApprovedOn = purchaseOrderTransactionVm.ApprovedOn;
                purchaseOrderTran.IsApproved = true;

                db.PurchaseOrderTransaction.Attach(purchaseOrderTran);
                db.Entry(purchaseOrderTran).State = EntityState.Modified;

                // Hit the accounting head for this supplier
                var lastTran = _ppsDbContext.TransactionEntry
                .Where(x => x.TransactionTypeId == (int)TransactionTypeEnum.Payment
                            && x.FiscalYear == fiscalYear
                            && x.CompanyId == userVm.CompanyId)
                .OrderByDescending(x => x.TransactionNumber.Substring(9, 4))
                .FirstOrDefault();

                var lastNumber = 0;
                if (lastTran != null)
                {
                    lastNumber = int.Parse(lastTran.TransactionNumber.Substring(9, 4));
                }
                var transactionNumber = _transactionRepository.CreateTransactionNumber((int)TransactionTypeEnum.Payment, purchaseOrderTransactionVm.ApprovedOn, lastNumber + 1);

                var systemTransaction = new TransactionEntry
                {
                    IsSystemGenerated = true,
                    TransactionNumber = transactionNumber,
                    TransactionDate = DateTime.Now,
                    FiscalYear = fiscalYear,
                    TransactionTypeId = (int)TransactionTypeEnum.Payment,
                    CompanyId = userVm.CompanyId,
                    PostingDate = DateTime.Now,
                    Active = true,
                    Deleted = false,
                    Accepted = false,
                    CreatedById = purchaseOrderTransactionVm.ApprovedBy,
                    CreatedDate = purchaseOrderTransactionVm.ApprovedOn,
                    VerifiedById = purchaseOrderTransactionVm.ApprovedBy,
                    VerifiedDate = purchaseOrderTransactionVm.ApprovedOn,
                    TransactionDetail = new List<TransactionDetail>()
                };

                systemTransaction.TransactionDetail.Add(new TransactionDetail
                {
                    AccountHeadId = purchaseOrderTran.PurchaseOrder.CashAccountHeadId ?? 0,
                    CrAmount = purchaseOrderTran.PurchaseOrder.CashAmount ?? 0,
                    DrAmount = 0
                });
                systemTransaction.TransactionDetail.Add(new TransactionDetail
                {
                    AccountHeadId = purchaseOrderTran.PurchaseOrder.SupplierAccountHeadId ?? 0,
                    CrAmount = 0,
                    DrAmount = purchaseOrderTran.PurchaseOrder.SupplierAmount ?? 0
                });

                db.TransactionEntry.Add(systemTransaction);
                db.SaveChanges();
                return returnVm;
            }
        }

        public List<PurchaseOrderTransactionVm> GetUnapprovedPurchaseOrderTransaction()
        {
            var trans = _ppsDbContext.PurchaseOrderTransaction
                .Where(x => x.IsApproved == false)
                .ToList()
                .Select(x =>
                    new PurchaseOrderTransactionVm
                    {
                        Id = x.Id,
                        PurchaseOrderId = x.PurchaseOrderId,
                        PurchaseOrderNo = x.PurchaseOrder.PurchaseOrderNo,
                        SupplierName = x.Supplier?.SupplierName + " - " + x.Supplier?.SupplierCode,
                        SupplierCode = x.Supplier?.SupplierCode.ToString() ?? "",
                        TransactionDate = x.TransactionDate,
                        TransactionAmount = x.TransactionAmount,
                        AccountName = x.AccountHead?.AccountHeadName,
                        AccountCode = x.AccountHead?.AccountHeadCode,
                        CreatedByName = GetUserName(x.CreatedBy),
                        CreatedOn = x.CreatedOn
                    }
                ).ToList();
            return trans;
        }

        public List<PurchaseOrderTransactionVm> GetApprovedPurchaseOrderTransaction()
        {
            var trans = _ppsDbContext.PurchaseOrderTransaction
                .Where(x => x.IsApproved)
                .ToList()
                .Select(x =>
                    new PurchaseOrderTransactionVm
                    {
                        Id = x.Id,
                        PurchaseOrderId = x.PurchaseOrderId,
                        PurchaseOrderNo = x.PurchaseOrder.PurchaseOrderNo,
                        SupplierName = x.Supplier?.SupplierName + " - " + x.Supplier?.SupplierCode,
                        SupplierCode = x.Supplier?.SupplierCode.ToString() ?? "",
                        TransactionDate = x.TransactionDate,
                        TransactionAmount = x.TransactionAmount,
                        AccountName = x.AccountHead?.AccountHeadName,
                        AccountCode = x.AccountHead?.AccountHeadCode,
                        CreatedByName = GetUserName(x.CreatedBy),
                        CreatedOn = x.CreatedOn
                    }
                ).ToList();
            return trans;
        }

        #region Private memebers

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

        #endregion
    }
}
