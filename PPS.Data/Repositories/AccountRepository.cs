using PPS.Data.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PPS.Data.Dtos;
using System.Threading.Tasks;
using PPS.API.Shared.ViewModel.Account;
using PPS.Data.Edmx;
using PPS.API.Shared.Enums;
using PPS.Caching;

namespace PPS.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private PPSDbContext _ppsDbContext;
        public AccountRepository()
        {
            _ppsDbContext = new PPSDbContext();
        }
        public List<AccountDto> GetLegerList(int fiscalYear, int companyId)
        {
            var key = CacheKey.AccountLedgerHeadList;
            var cachedObject = GlobalCachingProvider.Instance.GetItem(key);
            var headList = new List<AccountDto>();
            if (cachedObject != null)
            {
                headList = (List<AccountDto>)cachedObject;
            }
            else
            {
                headList = (from accountNature in _ppsDbContext.AccountNature
                                join accountType in _ppsDbContext.AccountType
                                on accountNature.Id equals accountType.AccountNatureId
                                join accountPrimaryHead in _ppsDbContext.AccountPrimaryHead
                                on accountType.Id equals accountPrimaryHead.AccountTypeId
                                join accountSubHead in _ppsDbContext.AccountSubHead
                                on accountPrimaryHead.Id equals accountSubHead.AccountPrimaryHeadId
                                join accountHead in _ppsDbContext.AccountHead
                                on accountSubHead.Id equals accountHead.AccountSubHeadId
                                where accountHead.CompanyId == companyId
                                select new AccountDto
                                {
                                    AccountNatureId = accountNature.Id,
                                    AccountNature = accountNature.AccountNatureName,
                                    AccountTypeId = accountNature.Id,
                                    AccountType = accountType.AccountTypeName,
                                    PrimaryHeadId = accountPrimaryHead.Id,
                                    PrimaryHead = accountPrimaryHead.AccountPrimaryHeadName,
                                    SubHeadId = accountSubHead.Id,
                                    SubHead = accountSubHead.AccountSubHeadName,
                                    HeadId = accountHead.Id,
                                    HeadName = accountHead.AccountHeadName,
                                    HeadCode = accountHead.AccountHeadCode,
                                    Active = accountHead.Active,
                                    CreatedById = accountHead.CreatedById,
                                    CreatedDate = accountHead.CreatedDate,
                                    UpdatedById = accountHead.UpdatedById,
                                    UpdatedDate = accountHead.UpdatedDate,
                                    FiscalYear = fiscalYear,
                                    CompanyId = companyId
                                }).ToList();

                if (headList != null)
                {
                    foreach (var head in headList)
                    {
                        var openBalance = _ppsDbContext.AccountHeadOpening.FirstOrDefault(x => x.AccountHeadId == head.HeadId && x.FiscalYear == fiscalYear);
                        if (openBalance != null)
                        {
                            head.DrAmount = openBalance.DrAmount;
                            head.CrAmount = openBalance.CrAmount;
                        }
                    }
                }
                GlobalCachingProvider.Instance.AddItem(key, headList);
            }
            return headList.ToList();
        }

        public List<AccountDto> GetAccountTypeList(int fiscalYear, int companyId)
        {
            var key = CacheKey.AccountLedgerTypeList;
            var cachedObject = GlobalCachingProvider.Instance.GetItem(key);
            var accountTypeList = new List<AccountDto>();
            if (cachedObject != null)
            {
                accountTypeList = (List<AccountDto>)cachedObject;
            }
            else
            {
                accountTypeList = (from accountNature in _ppsDbContext.AccountNature
                                      join accountType in _ppsDbContext.AccountType on accountNature.Id equals accountType.AccountNatureId
                                      where accountType.CompanyId == companyId
                                      select new AccountDto
                                      {
                                          AccountNatureId = accountNature.Id,
                                          AccountNature = accountNature.AccountNatureName,
                                          AccountTypeId = accountType.Id,
                                          AccountType = accountType.AccountTypeName
                                      }).ToList();
                GlobalCachingProvider.Instance.AddItem(key, accountTypeList);
            }
            return accountTypeList;
        }

        public List<AccountDto> GetAccountPrimaryHeadListForLedger(int fiscalYear, int companyId)
        {
            var key = CacheKey.AccountLedgerPrimaryHeadListForLedger;
            var cachedObject = GlobalCachingProvider.Instance.GetItem(key);
            var accountPrimaryHeadList = new List<AccountDto>();
            if (cachedObject != null)
            {
                accountPrimaryHeadList = (List<AccountDto>)cachedObject;
            }
            else
            {
                accountPrimaryHeadList = (from accountNature in _ppsDbContext.AccountNature
                                             join accountType in _ppsDbContext.AccountType
                                             on accountNature.Id equals accountType.AccountNatureId
                                             join accountPrimaryHead in _ppsDbContext.AccountPrimaryHead
                                             on accountType.Id equals accountPrimaryHead.AccountTypeId
                                             where accountPrimaryHead.CompanyId == companyId
                                             select new AccountDto
                                             {
                                                 AccountNatureId = accountNature.Id,
                                                 AccountNature = accountNature.AccountNatureName,
                                                 AccountTypeId = accountType.Id,
                                                 AccountType = accountType.AccountTypeName,
                                                 PrimaryHeadId = accountPrimaryHead.Id,
                                                 PrimaryHead = accountPrimaryHead.AccountPrimaryHeadName
                                             }).ToList();
                GlobalCachingProvider.Instance.AddItem(key, accountPrimaryHeadList);
            }
            return accountPrimaryHeadList;
        }

        public List<AccountDto> GetAccountPrimaryHeadList(int companyId, int accountNatureId)
        {
            var key = CacheKey.AccountLedgerPrimaryHeadList;
            var cachedObject = GlobalCachingProvider.Instance.GetItem(key);
            var accountPrimaryList = new List<AccountDto>();
            if (cachedObject != null)
            {
                accountPrimaryList = (List<AccountDto>)cachedObject;
            }
            else
            {
                accountPrimaryList = (from accountType in _ppsDbContext.AccountType
                                          join accountPrimaryHead in _ppsDbContext.AccountPrimaryHead on accountType.Id equals accountPrimaryHead.AccountTypeId
                                          where accountType.Id == accountNatureId && accountType.CompanyId == companyId
                                          select new AccountDto
                                          {
                                              AccountNatureId = accountType.Id,
                                              AccountNature = accountType.AccountTypeName,
                                              PrimaryHeadId = accountPrimaryHead.Id,
                                              PrimaryHead = accountPrimaryHead.AccountPrimaryHeadName
                                          }).ToList();
                GlobalCachingProvider.Instance.AddItem(key, accountPrimaryList);
            }
            return accountPrimaryList.ToList();
        }

        public List<AccountDto> GetAccountSubHeadList(int companyId, int accountPrimaryHeadId)
        {
            var key = CacheKey.AccountLedgerSubHeadList;
            var cachedObject = GlobalCachingProvider.Instance.GetItem(key);
            var accountSubHeadList = new List<AccountDto>();
            if (cachedObject != null)
            {
                accountSubHeadList = (List<AccountDto>)cachedObject;
            }
            else
            {
                accountSubHeadList = (from accountPrimaryHead in _ppsDbContext.AccountPrimaryHead
                                          join accountSubHead in _ppsDbContext.AccountSubHead
                                          on accountPrimaryHead.Id equals accountSubHead.AccountPrimaryHeadId
                                          where accountPrimaryHead.Id == accountPrimaryHeadId
                                          && accountPrimaryHead.CompanyId == companyId
                                          select new AccountDto
                                          {
                                              SubHeadId = accountSubHead.Id,
                                              SubHead = accountSubHead.AccountSubHeadName,
                                              PrimaryHeadId = accountPrimaryHead.Id,
                                              PrimaryHead = accountPrimaryHead.AccountPrimaryHeadName
                                          }).ToList();
                GlobalCachingProvider.Instance.AddItem(key, accountSubHeadList);
            }
            return accountSubHeadList;
        }

        public AccountHeadModel SaveAccountHead(AccountHeadModel headModel)
        {
            var accountHead = new AccountHead
            {
                //AccountSubHeadId = headModel.PrimaryHeadId,
                AccountSubHeadId = headModel.SubHeadId,
                AccountHeadCode = headModel.HeadCode,
                AccountHeadName = headModel.HeadName,
                Active = headModel.Active,
                CompanyId = headModel.CompanyId,
                CreatedById = headModel.CreatedById,
                CreatedDate = headModel.CreatedDate,
                UpdatedById = headModel.UpdatedById,
                UpdatedDate = headModel.UpdatedDate
            };
            var accountHeadOpening = new AccountHeadOpening
            {
                DrAmount = headModel.DrAmount,
                CrAmount = headModel.CrAmount,
                FiscalYear = headModel.FiscalYear,
                CompanyId = headModel.CompanyId,
                CreatedById = headModel.CreatedById,
                CreatedDate = headModel.CreatedDate,
                UpdatedById = headModel.UpdatedById,
                UpdatedDate = headModel.UpdatedDate
            };
            accountHead.AccountHeadOpening.Add(accountHeadOpening);

            var createdLedger = _ppsDbContext.AccountHead.Add(accountHead);
            var count = _ppsDbContext.SaveChanges();
            //ledger.Id = createdLedger.Id;
            // Invalidate all the Account Head related cache items. 
            GlobalCachingProvider.Instance.InvalidateItem(CacheKey.AccountLedgerHeadList);
            GlobalCachingProvider.Instance.InvalidateItem(CacheKey.AccountLedgerBankCashAccountHeadList);
            GlobalCachingProvider.Instance.InvalidateItem(CacheKey.AccountLedgerSalesAccountList);
            GlobalCachingProvider.Instance.InvalidateItem(CacheKey.AccountLedgerLCAccountHeadList);
            return headModel;
        }

        public List<AccountHeadVm> GetBankCashAccountHeadList(int customerId)
        {
            // TODO 1: Need to remove customerId param
            // TODO 2: Another method with same name, remove which is not required, if required then update CacheKey 
            var key = CacheKey.AccountLedgerBankCashAccountHeadList;
            var cachedObject = GlobalCachingProvider.Instance.GetItem(key);
            var bankCashAccountList = new List<AccountHeadVm>();
            if (cachedObject != null)
            {
                bankCashAccountList = (List<AccountHeadVm>)cachedObject;
            }
            else
            {
                var bankAccountSubHeadId = _ppsDbContext.ReferenceTable.FirstOrDefault(x => x.ReferenceName == ReferenceTableEnum.BankAccountSubHeadId.ToString())?.ReferenceValue;
                var cashAccountSubHeadId = _ppsDbContext.ReferenceTable.FirstOrDefault(x => x.ReferenceName == ReferenceTableEnum.CashAccountSubHeadId.ToString())?.ReferenceValue;

                var bankAccountSubHeadIdValue = Convert.ToInt32(bankAccountSubHeadId);
                var cashAccountSubHeadIdValue = Convert.ToInt32(cashAccountSubHeadId);

                bankCashAccountList = (_ppsDbContext.AccountHead.Where(x =>
                        (x.AccountSubHead.Id == bankAccountSubHeadIdValue ||
                         x.AccountSubHead.Id == cashAccountSubHeadIdValue) && x.Active == true)
                    .ToList()
                    .Select(x =>
                        new AccountHeadVm
                        {
                            Id = x.Id,
                            AccountCode = x.AccountHeadCode,
                            AccountName = x.AccountHeadName,
                            AccountType = x.Id == Convert.ToInt32(bankAccountSubHeadId) ? "Bank" : "Cash"
                        }
                    )).ToList();
                GlobalCachingProvider.Instance.AddItem(key, bankCashAccountList);
            }
            return bankCashAccountList;
        }

        public List<AccountHeadVm> GetSalesAccount()
        {
            var key = CacheKey.AccountLedgerSalesAccountList;
            var cachedObject = GlobalCachingProvider.Instance.GetItem(key);
            var salesAccounts = new List<AccountHeadVm>();
            if (cachedObject != null)
            {
                salesAccounts = (List<AccountHeadVm>)cachedObject;
            }
            else
            {
                var salesAccountHeadId = _ppsDbContext.ReferenceTable.FirstOrDefault(x => x.ReferenceName == ReferenceTableEnum.SalesAccountHeadId.ToString())?.ReferenceValue;
                // TODO: Need to remove .ToList() from the query
                salesAccounts = (_ppsDbContext.AccountHead.Where(x => (x.AccountSubHead.Id == Convert.ToInt32(salesAccountHeadId)) && x.Active == true)
                    .ToList()
                    .Select(x =>
                        new AccountHeadVm
                        {
                            Id = x.Id,
                            AccountCode = x.AccountHeadCode,
                            AccountName = x.AccountHeadName,
                        }
                    )).ToList();
                GlobalCachingProvider.Instance.AddItem(key, salesAccounts);
            }
            return salesAccounts;
        }

        public List<AccountHeadVm> GetBankCashAccountHeadList()
        {
            var key = CacheKey.AccountLedgerBankCashAccountHeadList;
            var cachedObject = GlobalCachingProvider.Instance.GetItem(key);
            var bankCashAccountList = new List<AccountHeadVm>();
            if (cachedObject != null)
            {
                bankCashAccountList = (List<AccountHeadVm>)cachedObject;
            }
            else
            {
                var bankAccountSubHeadId = _ppsDbContext.ReferenceTable.FirstOrDefault(x => x.ReferenceName == ReferenceTableEnum.BankAccountSubHeadId.ToString())?.ReferenceValue;
                var cashAccountSubHeadId = _ppsDbContext.ReferenceTable.FirstOrDefault(x => x.ReferenceName == ReferenceTableEnum.CashAccountSubHeadId.ToString())?.ReferenceValue;

                var bankAccountSubHeadIdValue = Convert.ToInt32(bankAccountSubHeadId);
                var cashAccountSubHeadIdValue = Convert.ToInt32(cashAccountSubHeadId);

                bankCashAccountList = (_ppsDbContext.AccountHead.Where(x =>
                        (x.AccountSubHead.Id == bankAccountSubHeadIdValue ||
                         x.AccountSubHead.Id == cashAccountSubHeadIdValue) && x.Active == true)
                    .ToList()
                    .Select(x =>
                        new AccountHeadVm
                        {
                            Id = x.Id,
                            AccountCode = x.AccountHeadCode,
                            AccountName = x.AccountHeadName,
                            AccountType = x.AccountSubHeadId == bankAccountSubHeadIdValue ? "Bank" : "Cash"
                        }
                    )).ToList();
                GlobalCachingProvider.Instance.AddItem(key, bankCashAccountList);
            }
            return bankCashAccountList;
        }

        public List<AccountHeadVm> GetLCAccountHeadList()
        {
            var key = CacheKey.AccountLedgerLCAccountHeadList;
            var cachedObject = GlobalCachingProvider.Instance.GetItem(key);
            var lcAccountList = new List<AccountHeadVm>();
            if (cachedObject != null)
            {
                lcAccountList = (List<AccountHeadVm>)cachedObject;
            }
            else
            {
                var lcAccountSubHeadId = _ppsDbContext.ReferenceTable.FirstOrDefault(x => x.ReferenceName == ReferenceTableEnum.LCAccountSubHeadId.ToString())?.ReferenceValue;

                var lcAccountSubHeadIdValue = Convert.ToInt32(lcAccountSubHeadId);

                lcAccountList = _ppsDbContext.AccountHead.Where(x =>
                        (x.AccountSubHead.Id == lcAccountSubHeadIdValue) && x.Active == true)
                    .ToList()
                    .Select(x =>
                        new AccountHeadVm
                        {
                            Id = x.Id,
                            AccountCode = x.AccountHeadCode,
                            AccountName = x.AccountHeadName
                        }
                    ).ToList();
                GlobalCachingProvider.Instance.AddItem(key, lcAccountList);
            }
            return lcAccountList;
        }
    }
}