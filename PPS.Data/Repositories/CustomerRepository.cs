using PPS.Data.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using PPS.Data.Edmx;
using PPS.API.Shared.ViewModel.Sales;
using System.Collections.Concurrent;
using PPS.API.Shared.Extensions;
using System.Threading.Tasks;
using System.Data.Entity;
using PPS.API.Shared.Enums;
using PPS.API.Shared.ViewModel.Customer;
using PPS.API.Shared.ViewModel.User;
using PPS.Shared.Service.Services;
using PPS.Shared.Service.Vm;
using PPS.API.Shared.ViewModel.Filter;
using PPS.Shared.Service.Extensions;
using PPS.Caching;

namespace PPS.Data.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly PPSDbContext _ppsDbContext;
        private readonly ITransactionRepository _transactionRepository;
        public CustomerRepository()
        {
            _ppsDbContext = new PPSDbContext();
            _transactionRepository = new TransactionRepository();
        }

        public List<CustomerModel> GetCustomerList()
        {
            var key = CacheKey.CustomerList;
            var cachedObject = GlobalCachingProvider.Instance.GetItem(key);
            var customerList = new List<CustomerModel>();
            if (cachedObject != null)
            {
                customerList = (List<CustomerModel>)cachedObject;                
            }
            else
            {
                customerList = _ppsDbContext.Customer
                    .Where(x => x.CustomerStatusId == (int)CustomerStatusEnum.Activated)
                    .ToList()
                    .Select(x =>
                new CustomerModel
                {
                    Id = x.Id,
                    CustomerName = x.CustomerName,
                    CustomerCode = x.CustomerCode,
                    CustomerAddress = x.CustomerAddress,
                    CustomerMobile = x.CustomerMobile,
                    CustomerPhone = x.CustomerPhone,
                    Village = x.Village,
                    PostOffice = x.PostOffice?.PostOfficeName,
                    Email = x.Email,
                    Area = x.Area?.AreaName,
                    EmployeeId = x.EmployeeId,
                    EmployeeCode = x.Employee.EmployeeCode,
                    Status = x.CustomerStatus?.Status,
                    TotalBalance = GetAvailableAmountByCustomer(x.Id, x).Result
                }).ToList();
                GlobalCachingProvider.Instance.AddItem(key, customerList);
            }
            return customerList;
        }
        public List<CustomerModel> GetPendingDeactivatedCustomer()
        {
            var customerList = _ppsDbContext.Customer
                .Where(x => x.CustomerStatusId != (int)CustomerStatusEnum.Activated)
                .ToList()
                .Select(x =>
            new CustomerModel
            {
                Id = x.Id,
                CustomerName = x.CustomerName,
                CustomerCode = x.CustomerCode,
                CustomerAddress = x.CustomerAddress,
                CustomerMobile = x.CustomerMobile,
                CustomerPhone = x.CustomerPhone,
                Village = x.Village,
                PostOffice = x.PostOffice?.PostOfficeName,
                Email = x.Email,
                Area = x.Area?.AreaName,
                EmployeeCode = x.Employee.EmployeeCode,
                Status = x.CustomerStatus?.Status,
                TotalBalance = GetAvailableAmountByCustomer(x.Id, x).Result
            }).ToList();
            return customerList;
        }
        public CustomerModel GetCustomerById(int customerId)
        {
            var customer = _ppsDbContext.Customer.FirstOrDefault(x => x.Id == customerId);
            if (customer == null)
            {
                throw new Exception($"The customer id: {customerId} doesn't exist.");
            }
            var customerVm = new CustomerModel
            {
                Id = customer.Id,
                CustomerName = customer.CustomerName,
                CustomerCode = customer.CustomerCode,
                CustomerAddress = customer.CustomerAddress,
                CustomerMobile = customer.CustomerMobile,
                CustomerPhone = customer.CustomerPhone,
                OwnerName = customer.OwnerName,
                OwnerMobile = customer.OwnerMobile,
                OwnerPhone = customer.OwnerPhone,
                OwnerBirthDate = customer.OwnerBirthDate,
                ContactPersonName = customer.ContactPersonName,
                ContactPersonMobile = customer.ContactPersonMobile,
                PrimaryContactNo = customer.PrimaryContactNo,
                Village = customer.Village,
                PostOfficeId = customer.PostOfficeId,
                PostOffice = customer.PostOffice?.PostOfficeName,
                Email = customer.Email,
                AreaId = customer.AreaId,
                Area = customer.Area?.AreaName,
                EmployeeId = customer.EmployeeId,
                EmployeeName = customer.Employee.FirstName + " " + customer.Employee.LastName + " (" + customer.Employee.EmployeeCode + ")",
                CustomerTypeName = customer.CustomerType?.CustomerTypeName,
                AccountHeadId = customer.AccountHeadId,
                TotalBalance = GetAvailableAmountByCustomer(customer.Id, customer).Result,
                Status = customer.CustomerStatus?.Status
            };

            var salesCredit = customer.CustomerSalesCredit.FirstOrDefault();
            if (salesCredit == null)
            {
                return null;
            }
            customerVm.MonthlyCredit = salesCredit.MonthlyCredit ?? 0;
            customerVm.YearlyCredit = salesCredit.YearlyCredit ?? 0;
            customerVm.SalesCapacityYearly = salesCredit.SalesCapacityYearly ?? 0;
            customerVm.EffectiveDate = salesCredit.EffectiveDate;

            var customerTransactionVm = new ConcurrentBag<CustomerTransactionVm>();
            //TODO it is to be implemented.
            //customer.CustomerTransaction.ToList().ForEach(tran =>
            //{
            //    customerTransactionVm.Add(new CustomerTransactionVm
            //    {
            //        Id = tran.Id,
            //        AccountName = tran.AccountHead?.AccountHeadName,
            //        AccountCode = tran.AccountHead?.AccountHeadCode,
            //        CreatedByName = GetUserName(tran.CreatedBy),
            //        CreatedOn = tran.CreatedOn,
            //        TransactionAmount = tran.TransactionAmount,
            //        TransactionReference = tran.TransactionReference,
            //        TransactionDate = tran.TransactionDate,
            //        IsApproved = tran.IsApproved
            //    });
            //});
            customerVm.CustomerTransaction = customerTransactionVm.ToList();
            return customerVm;
        }

        public CustomerModel SaveCustomer(CustomerModel customerEntry)
        {
            using (var db = new PPSDbContext())
            {
                var isCustomerExist = db.Customer.FirstOrDefault(x =>
                    x.CustomerName == customerEntry.CustomerName.Trim() && x.CustomerCode == customerEntry.CustomerCode);
                if (isCustomerExist != null)
                {
                    throw new Exception("This customer is already exist into the system.");
                }

                var customer = new Customer
                {
                    CustomerName = customerEntry.CustomerName,
                    CustomerCode = customerEntry.CustomerCode,
                    CustomerAddress = customerEntry.CustomerAddress,
                    CustomerMobile = customerEntry.CustomerMobile,
                    CustomerPhone = customerEntry.CustomerPhone,
                    OwnerName = customerEntry.OwnerName,
                    OwnerMobile = customerEntry.OwnerMobile,
                    OwnerPhone = customerEntry.OwnerPhone,
                    OwnerBirthDate = customerEntry.OwnerBirthDate,
                    ContactPersonName = customerEntry.ContactPersonName,
                    ContactPersonMobile = customerEntry.ContactPersonMobile,
                    PrimaryContactNo = customerEntry.PrimaryContactNo,
                    Village = customerEntry.Village,
                    PostOfficeId = customerEntry.PostOfficeId,
                    Email = customerEntry.Email,
                    AreaId = customerEntry.AreaId,
                    EmployeeId = customerEntry.EmployeeId,
                    AccountHeadId = customerEntry.AccountHeadId,
                    CustomerStatusId = (int)CustomerStatusEnum.Pending,
                    CreatedBy = customerEntry.CreatedBy,
                    CreatedOn = customerEntry.CreatedOn
                };

                //Create Customer Sales Credit
                var customerSalesCredit = new CustomerSalesCredit
                {
                    CustomerId = customer.Id,
                    MonthlyCredit = customerEntry.MonthlyCredit,
                    YearlyCredit = customerEntry.YearlyCredit,
                    SalesCapacityYearly = customerEntry.SalesCapacityYearly,
                    EffectiveDate = customerEntry.EffectiveDate
                };

                // Get Customer SubHead Id 
                var customerSubHeadId = db.ReferenceTable.FirstOrDefault(x => x.ReferenceName == ReferenceTableEnum.CustomerAccountSubHeadId.ToString())?.ReferenceValue;
                if (customerSubHeadId == null)
                {
                    throw new Exception("Customer sub head doesn't set in the reference table.");
                }

                var isCustomerAccountHeadNameExist =
                    db.AccountHead.FirstOrDefault(x => x.AccountHeadName == customerEntry.CustomerName);
                if (isCustomerAccountHeadNameExist != null)
                {
                    throw new Exception("The account head for this customer is already exist.");
                }

                // Create Account Head for the customer
                var accountHead = new AccountHead
                {
                    AccountSubHeadId = Convert.ToInt32(customerSubHeadId),
                    AccountHeadCode = customerEntry.CustomerCode.ToString(),
                    AccountHeadName = customerEntry.CustomerName,
                    Active = true,
                    CompanyId = customerEntry.CompanyId,
                    CreatedById = customerEntry.CreatedBy,
                    CreatedDate = customerEntry.CreatedOn,
                    UpdatedById = customerEntry.CreatedBy,
                    UpdatedDate = customerEntry.CreatedOn
                };
                var accountHeadOpening = new AccountHeadOpening
                {
                    DrAmount = 0,
                    CrAmount = 0,
                    FiscalYear = customerEntry.FiscalYear,
                    CompanyId = customerEntry.CompanyId,
                    CreatedById = customerEntry.CreatedBy,
                    CreatedDate = customerEntry.CreatedOn,
                    UpdatedById = customerEntry.CreatedBy,
                    UpdatedDate = customerEntry.CreatedOn
                };
                db.CustomerSalesCredit.Add(customerSalesCredit);
                accountHead.Customer.Add(customer);
                accountHead.AccountHeadOpening.Add(accountHeadOpening);
                db.AccountHead.Add(accountHead);
                db.SaveChanges();
                GlobalCachingProvider.Instance.InvalidateItem(CacheKey.CustomerList);
                return customerEntry;
            }
        }
        public async Task<bool> VerifyCustomer(CustomerModel customerModel)
        {
            using (var db = new PPSDbContext())
            {
                var customer = db.Customer.FirstOrDefault(x => x.Id == customerModel.Id);
                if (customer == null)
                {
                    throw new Exception($"This customer no {customerModel.Id} doesn't exist.");
                }
                var customerHistory = new CustomerHistory
                {
                    CustomerId = customer.Id,
                    CustomerName = customer.CustomerName,
                    CustomerCode = customer.CustomerCode,
                    CustomerAddress = customer.CustomerAddress,
                    CustomerMobile = customer.CustomerMobile,
                    CustomerPhone = customer.CustomerPhone,
                    OwnerName = customer.OwnerName,
                    OwnerMobile = customer.OwnerMobile,
                    OwnerPhone = customer.OwnerPhone,
                    OwnerBirthDate = customer.OwnerBirthDate,
                    ContactPersonName = customer.ContactPersonName,
                    ContactPersonMobile = customer.ContactPersonMobile,
                    PrimaryContactNo = customer.PrimaryContactNo,
                    Village = customer.Village,
                    PostOfficeId = customer.PostOfficeId,
                    Email = customer.Email,
                    AreaId = customer.AreaId,
                    EmployeeId = customer.EmployeeId,
                    AccountHeadId = customer.AccountHeadId,
                    CustomerStatusId = customer.CustomerStatusId,
                    CreatedBy = customer.CreatedBy,
                    CreatedOn = customer.CreatedOn,
                    UpdatedBy = customer.UpdatedBy,
                    UpdatedOn = customer.UpdatedOn
                };
                db.CustomerHistory.Add(customerHistory);

                customer.UpdatedBy = customerModel.CreatedBy;
                customer.UpdatedOn = customerModel.CreatedOn;
                customer.CustomerStatusId = (int)CustomerStatusEnum.Activated;
                db.Customer.Attach(customer);
                db.Entry(customer).State = EntityState.Modified;
                await db.SaveChangesAsync();
                GlobalCachingProvider.Instance.InvalidateItem(CacheKey.CustomerList);
            }
            return true;
        }
        public async Task<bool> DeactivateCustomerAsync(CustomerModel customerModel)
        {
            using (var db = new PPSDbContext())
            {
                var customer = db.Customer.FirstOrDefault(x => x.Id == customerModel.Id);
                if (customer == null)
                {
                    throw new Exception($"This customer no {customerModel.Id} doesn't exist.");
                }
                if (customer.CustomerStatusId == (int)CustomerStatusEnum.Deactivated)
                {
                    throw new Exception($"This customer no {customerModel.Id} has already been deactivated.");
                }
                var customerHistory = new CustomerHistory
                {
                    CustomerId = customer.Id,
                    CustomerName = customer.CustomerName,
                    CustomerCode = customer.CustomerCode,
                    CustomerAddress = customer.CustomerAddress,
                    CustomerMobile = customer.CustomerMobile,
                    CustomerPhone = customer.CustomerPhone,
                    OwnerName = customer.OwnerName,
                    OwnerMobile = customer.OwnerMobile,
                    OwnerPhone = customer.OwnerPhone,
                    OwnerBirthDate = customer.OwnerBirthDate,
                    ContactPersonName = customer.ContactPersonName,
                    ContactPersonMobile = customer.ContactPersonMobile,
                    PrimaryContactNo = customer.PrimaryContactNo,
                    Village = customer.Village,
                    PostOfficeId = customer.PostOfficeId,
                    Email = customer.Email,
                    AreaId = customer.AreaId,
                    EmployeeId = customer.EmployeeId,
                    AccountHeadId = customer.AccountHeadId,
                    CustomerStatusId = customer.CustomerStatusId,
                    CreatedBy = customer.CreatedBy,
                    CreatedOn = customer.CreatedOn,
                    UpdatedBy = customer.UpdatedBy,
                    UpdatedOn = customer.UpdatedOn
                };
                db.CustomerHistory.Add(customerHistory);

                customer.UpdatedBy = customerModel.CreatedBy;
                customer.UpdatedOn = customerModel.CreatedOn;
                customer.CustomerStatusId = (int)CustomerStatusEnum.Deactivated;
                db.Customer.Attach(customer);
                db.Entry(customer).State = EntityState.Modified;
                await db.SaveChangesAsync();
                GlobalCachingProvider.Instance.InvalidateItem(CacheKey.CustomerList);
            }
            return true;
        }
        public async Task<bool> ActivateCustomerAsync(CustomerModel customerModel)
        {
            using (var db = new PPSDbContext())
            {
                var customer = db.Customer.FirstOrDefault(x => x.Id == customerModel.Id);
                if (customer == null)
                {
                    throw new Exception($"This customer no {customerModel.Id} doesn't exist.");
                }
                if (customer.CustomerStatusId == (int)CustomerStatusEnum.Activated)
                {
                    throw new Exception($"This customer no {customerModel.Id} has already been activated.");
                }
                var customerHistory = new CustomerHistory
                {
                    CustomerId = customer.Id,
                    CustomerName = customer.CustomerName,
                    CustomerCode = customer.CustomerCode,
                    CustomerAddress = customer.CustomerAddress,
                    CustomerMobile = customer.CustomerMobile,
                    CustomerPhone = customer.CustomerPhone,
                    OwnerName = customer.OwnerName,
                    OwnerMobile = customer.OwnerMobile,
                    OwnerPhone = customer.OwnerPhone,
                    OwnerBirthDate = customer.OwnerBirthDate,
                    ContactPersonName = customer.ContactPersonName,
                    ContactPersonMobile = customer.ContactPersonMobile,
                    PrimaryContactNo = customer.PrimaryContactNo,
                    Village = customer.Village,
                    PostOfficeId = customer.PostOfficeId,
                    Email = customer.Email,
                    AreaId = customer.AreaId,
                    EmployeeId = customer.EmployeeId,
                    AccountHeadId = customer.AccountHeadId,
                    CustomerStatusId = customer.CustomerStatusId,
                    CreatedBy = customer.CreatedBy,
                    CreatedOn = customer.CreatedOn,
                    UpdatedBy = customer.UpdatedBy,
                    UpdatedOn = customer.UpdatedOn
                };
                db.CustomerHistory.Add(customerHistory);

                customer.UpdatedBy = customerModel.CreatedBy;
                customer.UpdatedOn = customerModel.CreatedOn;
                customer.CustomerStatusId = (int)CustomerStatusEnum.Activated;
                db.Customer.Attach(customer);
                db.Entry(customer).State = EntityState.Modified;
                await db.SaveChangesAsync();
                GlobalCachingProvider.Instance.InvalidateItem(CacheKey.CustomerList);
            }
            return true;
        }
        public async Task<bool> SaveCustomerTransaction(int userId, CustomerTransactionVm customerTransactionVm)
        {
            var totalCustomerTranAmount = customerTransactionVm.CustomerTransactionDetail.Sum(x => x.TransactionAmount);
            if (customerTransactionVm.TransactionAmount + customerTransactionVm.BankChargeAmount - totalCustomerTranAmount != 0)
            {
                throw new Exception("Transaction amount mismatch.");
            }

            int? bankChargeAccountHeadId = null;
            if (customerTransactionVm.BankChargeAmount > 0)
            {
                var bankChargeAccountHead = _ppsDbContext.ReferenceTable.FirstOrDefault(x => x.ReferenceName == ReferenceTableEnum.BankChargeAccountHeadId.ToString()).ReferenceValue;
                if (bankChargeAccountHead == null)
                {
                    throw new Exception("Bank charge account head not found");
                }
                else
                {
                    bankChargeAccountHeadId = int.Parse(bankChargeAccountHead);
                }
            }

            var customerTransaction = new CustomerTransaction
            {
                CashBankAccountHeadId = customerTransactionVm.CashBankAccountHeadId,
                TransactionReference = customerTransactionVm.TransactionReference,
                BankChargeAccountHeadId = bankChargeAccountHeadId,
                BankChargeAmount = customerTransactionVm.BankChargeAmount,
                TransactionAmount = customerTransactionVm.TransactionAmount,
                TransactionDate = customerTransactionVm.TransactionDate,
                CreatedBy = customerTransactionVm.CreatedBy,
                CreatedOn = customerTransactionVm.CreatedOn,
                IsApproved = false,
                CustomerTransactionDetail = new List<CustomerTransactionDetail>()
            };

            foreach (var tempCtEntry in customerTransactionVm.CustomerTransactionDetail)
            {
                customerTransaction.CustomerTransactionDetail.Add(
                        new CustomerTransactionDetail
                        {
                            CustomerTransactionId = customerTransaction.Id,
                            CustomerId = tempCtEntry.CustomerId,
                            BookNo = tempCtEntry.BookNo,
                            BookSerialNo = tempCtEntry.BookSerialNo,
                            TransactionAmount = tempCtEntry.TransactionAmount
                        });
            }

            _ppsDbContext.CustomerTransaction.Add(customerTransaction);
            await _ppsDbContext.SaveChangesAsync();
            return true;
        }
        public bool ApproveCustomerTransaction(UserVm userVm, int fiscalYear, CustomerTransactionVm customerTransactionVm)
        {
            using (var db = new PPSDbContext())
            {
                var customerTran = db.CustomerTransaction.FirstOrDefault(x => x.Id == customerTransactionVm.Id);
                if (customerTran == null)
                {
                    throw new Exception($"This customer transaction no. {customerTransactionVm.Id} doesn't exist.");
                }
                if (customerTran.IsApproved == true)
                {
                    throw new Exception($"This customer transaction no. {customerTransactionVm.Id} has already been approved.");
                }

                customerTran.ApprovedBy = customerTransactionVm.ApprovedBy;
                customerTran.ApprovedOn = customerTransactionVm.ApprovedOn;
                customerTran.IsApproved = true;

                db.CustomerTransaction.Attach(customerTran);
                db.Entry(customerTran).State = EntityState.Modified;

                // Hit the accounting head for this customer
                var lastTran = _ppsDbContext.TransactionEntry
                .Where(x => x.TransactionTypeId == (int)TransactionTypeEnum.Receipt
                            && x.FiscalYear == fiscalYear
                            && x.CompanyId == userVm.CompanyId)
                .OrderByDescending(x => x.TransactionNumber.Substring(9, 4))
                .FirstOrDefault();

                var lastNumber = 0;
                if (lastTran != null)
                {
                    lastNumber = int.Parse(lastTran.TransactionNumber.Substring(9, 4));
                }
                var transactionNumber = _transactionRepository.CreateTransactionNumber((int)TransactionTypeEnum.Receipt, customerTransactionVm.ApprovedOn, lastNumber + 1);

                var sysTranDetail = new List<TransactionDetail>();

                // Customer Paid to Cash/Bank -> Customer(Cr), Cash / Bank(Dr)
                var systemTransaction = new TransactionEntry
                {
                    IsSystemGenerated = true,
                    TransactionNumber = transactionNumber,
                    TransactionDate = customerTran.TransactionDate,
                    FiscalYear = fiscalYear,
                    TransactionTypeId = (int)TransactionTypeEnum.Receipt,
                    CompanyId = userVm.CompanyId,
                    PostingDate = DateTime.Now,
                    Active = true,
                    Deleted = false,
                    Accepted = true,
                    VerifiedById = userVm.Id,
                    VerifiedDate = DateTime.Now,
                    CreatedById = customerTran.CreatedBy,
                    CreatedDate = customerTran.CreatedOn,
                    AcceptedById = userVm.Id,
                    AcceptedDate = DateTime.Now,
                    TransactionDetail = sysTranDetail
                };

                var customerTranList = customerTran.CustomerTransactionDetail.Select(x =>
                    new TransactionDetail
                    {
                        AccountHeadId = x.Customer?.AccountHeadId ?? 0,
                        DrAmount = 0,
                        CrAmount = x.TransactionAmount
                    }).ToList();

                if (customerTran.BankChargeAmount > 0)
                {
                    var bankChargeTranDetail = new TransactionDetail
                    {
                        AccountHeadId = customerTran.BankChargeAccountHeadId ?? 0,
                        DrAmount = customerTran.BankChargeAmount ?? 0,
                        CrAmount = 0
                    };
                    sysTranDetail.Add(bankChargeTranDetail);
                    customerTran.TransactionDetail = bankChargeTranDetail;
                }
                var cashBankTranDetail = new TransactionDetail
                {
                    AccountHeadId = customerTran.CashBankAccountHeadId,
                    DrAmount = customerTran.TransactionAmount,
                    CrAmount = 0
                };
                sysTranDetail.Add(cashBankTranDetail);
                customerTran.TransactionDetail1 = cashBankTranDetail;

                sysTranDetail.AddRange(customerTranList);

                var drTotalAmount = systemTransaction.TransactionDetail.Sum(x => x.DrAmount);
                var crTotalAmount = systemTransaction.TransactionDetail.Sum(x => x.CrAmount);
                if (drTotalAmount != crTotalAmount)
                {
                    throw new Exception("The debit and credit amounts are mismatched.");
                }

                customerTran.TransactionEntry = systemTransaction;
                
                customerTranList.ForEach(x =>
                {
                    var customerTranDetail = customerTran.CustomerTransactionDetail.First(y => y.Customer.AccountHeadId == x.AccountHeadId);
                    customerTranDetail.TransactionDetail = x;
                });

                db.CustomerTransaction.Attach(customerTran);
                db.Entry(customerTran).State = EntityState.Modified;

                var cashBankHeadName = db.AccountHead.FirstOrDefault(x => x.Id == customerTran.CashBankAccountHeadId)?.AccountHeadName;
                db.SaveChanges();

                customerTran.CustomerTransactionDetail.ToList().ForEach(x =>
                {
                    var mobile = "88" + string.Join("", x.Customer.CustomerMobile.Split(' ', '-'));
                    var smsVm = new SMSVm
                    {
                        Amount = x.TransactionAmount,
                        BankThrough = cashBankHeadName,
                        DateOn = customerTran.TransactionDate,
                        Numbers = new List<string> { mobile }
                    };
                    var smsService = new SMSService();
                    smsService.SendDelarReceiptSMS(smsVm);
                });
            }
            return true;
        }
        public async Task<double> GetAvailableAmountByCustomer(int customerId, Customer customer = null)
        {
            var amount = 0d;
             if (customer == null)
            {
                customer = _ppsDbContext.Customer.FirstOrDefault(x => x.Id == customerId);
            }

            var customerTotalPaid = _ppsDbContext.CustomerTransaction.Where(x => x.IsApproved == true)
                .SelectMany(x => x.CustomerTransactionDetail.Where(y => y.CustomerId == customerId).Select(k => k.TransactionAmount)).ToList().Sum();

            var customerTotalInvoiceAmmount = _ppsDbContext.Invoice.Where(m => m.DemandOrder.CustomerId == customerId).SelectMany(m => m.InvoiceDetail.Where(x => x.InvoiceId == x.Invoice.Id)).Any() ? _ppsDbContext.Invoice.Where(m => m.DemandOrder.CustomerId == customerId&&m.ApprovedBy>0).ToList().Sum(x => x.TotalGrandAmount) : 0;
            amount = customerTotalPaid - (double)customerTotalInvoiceAmmount;
            return amount;
        }

        public List<CustomerTransactionVm> GetUnapprovedCustomerTransaction()
        {
            var trans = _ppsDbContext.CustomerTransaction
                .Where(x => x.IsApproved == false)
                .OrderByDescending(x => x.TransactionDate).ToList()
                .Select(x =>
                    new CustomerTransactionVm
                    {
                        Id = x.Id,
                        CashBankAccountHeadId = x.CashBankAccountHeadId,
                        AccountName = x.AccountHead.AccountHeadName,
                        AccountCode = x.AccountHead.AccountHeadCode,
                        TransactionReference = x.TransactionReference,
                        TransactionAmount = x.TransactionAmount,
                        BankChargeAmount = x.BankChargeAmount,
                        TransactionDate = x.TransactionDate,
                        CustomerTransactionDetail = x.CustomerTransactionDetail.Select(y => new CustomerTransactionDetailVm
                        {
                            CustomerId = y.CustomerId,
                            CustomerName = y.Customer?.CustomerName,
                            CustomerCode = y.Customer?.CustomerCode,
                            TransactionAmount = y.TransactionAmount,
                            BookNo = y.BookNo,
                            BookSerialNo = y.BookSerialNo
                        }),
                        CreatedByName = GetUserName(x.CreatedBy),
                        CreatedOn = x.CreatedOn
                    }
                ).ToList();
            return trans;
        }
        public IList<CustomerTypeModel> GetCustomerType()
        {
            var customerTypeList = (from ct in _ppsDbContext.CustomerType
                                    select new CustomerTypeModel()
                                    {
                                        Id = ct.Id,
                                        CustomerTypeName = ct.CustomerTypeName
                                    }).ToList();
            return customerTypeList.ToList();
        }

        public IList<AttachmentTypeVm> GetAttachmentType()
        {
            var attachmentType = _ppsDbContext.AttachmentType
                .Select(x => new AttachmentTypeVm()
                {
                    Id = x.Id,
                    AttachmentTypeName = x.AttachmentTypeName,
                    Strength = (int)x.Strength
                }).ToList();
            return attachmentType;
        }
        public CustomerModel UpdateCustomer(CustomerModel customerModel)
        {
            using (var db = new PPSDbContext())
            {
                var customer = db.Customer.FirstOrDefault(x => x.Id == customerModel.Id);
                if (customer == null)
                {
                    throw new Exception($"This customer no {customerModel.Id} doesn't exist.");
                }
                //Save current customer value to customer history
                var customerHistory = new CustomerHistory
                {
                    CustomerId = customer.Id,
                    CustomerName = customer.CustomerName,
                    CustomerCode = customer.CustomerCode,
                    CustomerAddress = customer.CustomerAddress,
                    CustomerMobile = customer.CustomerMobile,
                    CustomerPhone = customer.CustomerPhone,
                    OwnerName = customer.OwnerName,
                    OwnerMobile = customer.OwnerMobile,
                    OwnerPhone = customer.OwnerPhone,
                    OwnerBirthDate = customer.OwnerBirthDate,
                    ContactPersonName = customer.ContactPersonName,
                    ContactPersonMobile = customer.ContactPersonMobile,
                    PrimaryContactNo = customer.PrimaryContactNo,
                    Village = customer.Village,
                    PostOfficeId = customer.PostOfficeId,
                    Email = customer.Email,
                    AreaId = customer.AreaId,
                    EmployeeId = customer.EmployeeId,
                    AccountHeadId = customer.AccountHeadId,
                    CustomerStatusId = customer.CustomerStatusId,
                    CreatedBy = customer.CreatedBy,
                    CreatedOn = customer.CreatedOn,
                    UpdatedBy = customer.UpdatedBy,
                    UpdatedOn = customer.UpdatedOn
                };

                //Add customer value from UI
                customer.CustomerAddress = customerModel.CustomerAddress;
                customer.CustomerMobile = customerModel.CustomerMobile;
                customer.CustomerCode = customerModel.CustomerCode;
                customer.CustomerPhone = customerModel.CustomerPhone;
                customer.OwnerName = customerModel.OwnerName;
                customer.OwnerMobile = customerModel.OwnerMobile;
                customer.OwnerPhone = customerModel.OwnerPhone;
                customer.OwnerBirthDate = customerModel.OwnerBirthDate;
                customer.ContactPersonName = customerModel.ContactPersonName;
                customer.ContactPersonMobile = customerModel.ContactPersonMobile;
                customer.PrimaryContactNo = customerModel.PrimaryContactNo;
                customer.Village = customerModel.Village;
                customer.PostOfficeId = customerModel.PostOfficeId;
                customer.Email = customerModel.Email;
                customer.AreaId = customerModel.AreaId;
                customer.EmployeeId = customerModel.EmployeeId;
                customer.UpdatedBy = customerModel.UpdatedBy;
                customer.UpdatedOn = customerModel.UpdatedOn;

                db.CustomerHistory.Add(customerHistory);

                var customerSalesCredit = customer.CustomerSalesCredit.FirstOrDefault();
                if (customerSalesCredit == null)
                {
                    return null;
                }
                //Save current value to customer sales credit history
                var customerSalesCreditHistory = new CustomerSalesCreditHistory
                {
                    CustomerId = customerSalesCredit.CustomerId,
                    MonthlyCredit = customerSalesCredit.MonthlyCredit,
                    YearlyCredit = customerSalesCredit.YearlyCredit,
                    SalesCapacityYearly = customerSalesCredit.SalesCapacityYearly,
                    EffectiveDate = customerSalesCredit.EffectiveDate
                };

                //Add value to customer sales credit from ui
                customerSalesCredit.MonthlyCredit = customerModel.MonthlyCredit;
                customerSalesCredit.YearlyCredit = customerModel.YearlyCredit;
                customerSalesCredit.SalesCapacityYearly = customerModel.SalesCapacityYearly;
                customerSalesCredit.EffectiveDate = customerModel.EffectiveDate;

                db.CustomerSalesCreditHistory.Add(customerSalesCreditHistory);

                db.CustomerSalesCredit.Attach(customerSalesCredit);
                db.Entry(customerSalesCredit).State = EntityState.Modified;

                db.Customer.Attach(customer);
                db.Entry(customer).State = EntityState.Modified;

                db.SaveChanges();
                return customerModel;
            }
        }

        public List<CustomerTransactionVm> GetCustomerTransactionList()
        {
            var trans = _ppsDbContext.CustomerTransaction
                .OrderByDescending(x => x.TransactionDate).ToList()
                .Select(x =>
                    new CustomerTransactionVm
                    {
                        Id = x.Id,
                        CashBankAccountHeadId = x.CashBankAccountHeadId,
                        AccountName = x.AccountHead.AccountHeadName,
                        AccountCode = x.AccountHead.AccountHeadCode,
                        TransactionReference = x.TransactionReference,
                        TransactionAmount = x.TransactionAmount,
                        BankChargeAmount = x.BankChargeAmount,
                        TransactionDate = x.TransactionDate,
                        IsApproved=x.IsApproved,
                        Status = x.IsApproved == true ? "Approved" : "Pending",
                        CustomerTransactionDetail = x.CustomerTransactionDetail.Select(y => new CustomerTransactionDetailVm
                        {
                            CustomerId = y.CustomerId,
                            CustomerName = y.Customer?.CustomerName,
                            CustomerCode = y.Customer?.CustomerCode,
                            TransactionAmount = y.TransactionAmount,
                            BookNo = y.BookNo,
                            BookSerialNo = y.BookSerialNo
                        }),
                        CreatedByName = GetUserName(x.CreatedBy),
                        CreatedOn = x.CreatedOn
                    }
                ).ToList();
            return trans;
        }

        public List<CustomerTransactionVm> GetCustomerTransactionListForFiltering(FilterVm filterVm)
        {
            var totalCount = _ppsDbContext.CustomerTransaction.Count();
            var trans = _ppsDbContext.CustomerTransaction
                .OrderByField(filterVm.SortColumn, filterVm.SortDirection.ToString().Equals(SortDirectionEnum.ASC.ToString()))
                .Skip((filterVm.PageIndex - 1) * filterVm.PageSize).Take(filterVm.PageSize)
                .ToList()
                .Select(x =>
                    new CustomerTransactionVm
                    {
                        Id = x.Id,
                        CashBankAccountHeadId = x.CashBankAccountHeadId,
                        AccountName = x.AccountHead.AccountHeadName,
                        AccountCode = x.AccountHead.AccountHeadCode,
                        TransactionReference = x.TransactionReference,
                        TransactionAmount = x.TransactionAmount,
                        BankChargeAmount = x.BankChargeAmount,
                        TransactionDate = x.TransactionDate,
                        IsApproved=x.IsApproved,
                        Status = x.IsApproved == true ? "Approved" : "Pending",
                        CustomerTransactionDetail = x.CustomerTransactionDetail.Select(y => new CustomerTransactionDetailVm
                        {
                            CustomerId = y.CustomerId,
                            CustomerName = y.Customer?.CustomerName,
                            CustomerCode = y.Customer?.CustomerCode,
                            TransactionAmount = y.TransactionAmount,
                            BookNo = y.BookNo,
                            BookSerialNo = y.BookSerialNo
                        }),
                        CreatedByName = GetUserName(x.CreatedBy),
                        CreatedOn = x.CreatedOn,
                        TotalCount = totalCount
                    }
                ).ToList();
            return trans;
        }
        public List<CustomerTransactionVm> GetUnapprovedCustomerTransactionForFiltering(FilterVm filterVm)
        {
            var totalCount = _ppsDbContext.CustomerTransaction.Where(x => x.IsApproved == false).Count();
            var trans = _ppsDbContext.CustomerTransaction
                .Where(x => x.IsApproved == false)
                .OrderByField(filterVm.SortColumn, filterVm.SortDirection.ToString().Equals(SortDirectionEnum.ASC.ToString()))
                .Skip((filterVm.PageIndex - 1) * filterVm.PageSize).Take(filterVm.PageSize)
                .ToList()
                .Select(x =>
                    new CustomerTransactionVm
                    {
                        Id = x.Id,
                        CashBankAccountHeadId = x.CashBankAccountHeadId,
                        AccountName = x.AccountHead.AccountHeadName,
                        AccountCode = x.AccountHead.AccountHeadCode,
                        TransactionReference = x.TransactionReference,
                        TransactionAmount = x.TransactionAmount,
                        BankChargeAmount = x.BankChargeAmount,
                        TransactionDate = x.TransactionDate,
                        CustomerTransactionDetail = x.CustomerTransactionDetail.Select(y => new CustomerTransactionDetailVm
                        {
                            CustomerId = y.CustomerId,
                            CustomerName = y.Customer?.CustomerName,
                            CustomerCode = y.Customer?.CustomerCode,
                            TransactionAmount = y.TransactionAmount,
                            BookNo = y.BookNo,
                            BookSerialNo = y.BookSerialNo
                        }),
                        CreatedByName = GetUserName(x.CreatedBy),
                        CreatedOn = x.CreatedOn,
                        TotalCount = totalCount
                    }
                ).ToList();
            return trans;
        }
        public CustomerTransaction GetCustomerTransactionByTransactionId(int id)
        {
            return _ppsDbContext.CustomerTransaction.Where(m => m.Id == id).FirstOrDefault();
        }
        public List<CustomerTransactionVm> CustomerTransactionSearch(DateTime startDate, DateTime endDate)
        {
            var totalCount = _ppsDbContext.CustomerTransaction.Count();
            var trans = _ppsDbContext.CustomerTransaction
                .Where(x => DbFunctions.TruncateTime(x.TransactionDate) >= DbFunctions.TruncateTime(startDate) && DbFunctions.TruncateTime(x.TransactionDate) <= DbFunctions.TruncateTime(endDate))
                .ToList()
                .Select(x =>
                    new CustomerTransactionVm
                    {
                        Id = x.Id,
                        CashBankAccountHeadId = x.CashBankAccountHeadId,
                        AccountName = x.AccountHead.AccountHeadName,
                        AccountCode = x.AccountHead.AccountHeadCode,
                        TransactionReference = x.TransactionReference,
                        TransactionAmount = x.TransactionAmount,
                        BankChargeAmount = x.BankChargeAmount,
                        TransactionDate = x.TransactionDate,
                        Status = x.IsApproved == true ? "Approved" : "Pending",
                        CustomerTransactionDetail = x.CustomerTransactionDetail.Select(y => new CustomerTransactionDetailVm
                        {
                            CustomerId = y.CustomerId,
                            CustomerName = y.Customer.CustomerName,
                            CustomerCode = y.Customer.CustomerCode,
                            TransactionAmount = y.TransactionAmount,
                            BookNo = y.BookNo,
                            BookSerialNo = y.BookSerialNo
                        }),
                        CreatedByName = GetUserName(x.CreatedBy),
                        CreatedOn = x.CreatedOn,
                        TotalCount = totalCount
                    }
                ).ToList();
            return trans;
        }
        public CustomerTransaction UpdateCustomerTransaction(CustomerTransaction customerTransaction)
        {
            using (var db = new PPSDbContext())
            {
                db.CustomerTransaction.Attach(customerTransaction);
                db.Entry(customerTransaction).State = EntityState.Modified;
                db.SaveChanges();
                return customerTransaction;
            };
        }

        #region Private members
        private string GetUserByOn(int userId, DateTime on)
        {
            var text = string.Empty;
            var user = _ppsDbContext.User.FirstOrDefault(x => x.Id == userId);
            text = StringExtension.ToFullName(user?.FirstName, user?.LastName);
            text += " - " + on.ToString("dd/MM/yyyy");
            return text;
        }
        private string GetUserName(int userId)
        {
            var text = string.Empty;
            var user = _ppsDbContext.User.FirstOrDefault(x => x.Id == userId);
            text = StringExtension.ToFullName(user?.FirstName, user?.LastName);
            return text;
        }

        


        #endregion
    }
}
