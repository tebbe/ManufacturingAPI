using PPS.Service.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PPS.Data.Dtos;
using PPS.Data.RepositoryInterfaces;
using PPS.Data.Repositories;
using PPS.API.Shared.ViewModel.Account;

namespace PPS.Service.Services
{
    public class LedgerService : ILedgerInterface
    {
        private IAccountRepository _ledgerRepository;
        public LedgerService()
        {
            _ledgerRepository = new AccountRepository();
        }

        public List<AccountHeadModel> GetLegerList(int fiscalYear, int companyId)
        {
            var headList = _ledgerRepository.GetLegerList(fiscalYear, companyId);
            if (headList == null)
                return null;
            var list = new List<AccountHeadModel>();
            foreach (var head in headList)
            {
                list.Add(head.ToAccountHeadModel());
            }
            return list;
        }

        public List<AccountTypeModel> GetAccountTypeList(int fiscalYear, int companyId)
        {
            var typeList = _ledgerRepository.GetAccountTypeList(fiscalYear, companyId);
            if (typeList == null)
                return null;
            var list = new List<AccountTypeModel>();
            foreach (var head in typeList)
            {
                list.Add(head.ToAccountTypeModel());
            }
            return list;
        }

        public List<AccountPrimaryHeadModel> GetAccountPrimaryHeadListForLedger(int fiscalYear, int companyId)
        {
            var primaryHeadList = _ledgerRepository.GetAccountPrimaryHeadListForLedger(fiscalYear, companyId);
            if (primaryHeadList == null)
                return null;
            var list = new List<AccountPrimaryHeadModel>();
            foreach (var head in primaryHeadList)
            {
                list.Add(head.ToAccountPrimaryHeadModel());
            }
            return list;
        }

        public List<AccountPrimaryHeadModel> GetAccountPrimaryHeadList(int fiscalYear, int companyId)
        {
            var primaryHeadList = _ledgerRepository.GetAccountPrimaryHeadList(fiscalYear, companyId);
            if (primaryHeadList == null)
                return null;
            var list = new List<AccountPrimaryHeadModel>();
            foreach (var head in primaryHeadList)
            {
                list.Add(head.ToAccountPrimaryHeadModel());
            }
            return list;
        }

        public List<AccountSubHeadModel> GetAccountSubHeadList(int companyId, int accountPrimaryHeadId)
        {
            var subHeadList = _ledgerRepository.GetAccountSubHeadList(companyId, accountPrimaryHeadId);
            if (subHeadList == null)
                return null;
            var list = new List<AccountSubHeadModel>();
            foreach (var head in subHeadList)
            {
                list.Add(head.ToAccountSubHeadModel());
            }
            return list;
        }

        public AccountHeadModel SaveAccountHead(AccountHeadModel accountHead)
        {
            return _ledgerRepository.SaveAccountHead(accountHead);
        }

        public List<AccountHeadModel> GetAccountHeadList(int fiscalYear, int companyId)
        {
            var headList = _ledgerRepository.GetLegerList(fiscalYear, companyId);
            if (headList == null)
                return null;
            var list = new List<AccountHeadModel>();
            foreach (var head in headList)
            {
                list.Add(head.ToAccountHeadModel());
            }
            return list;
        }

        public List<AccountHeadVm> GetBankCashAccountHeadList(int customerId)
        {
            return _ledgerRepository.GetBankCashAccountHeadList(customerId);
        }

        public List<AccountHeadVm> GetSalesAccount()
        {
            return _ledgerRepository.GetSalesAccount();
        }

        public List<AccountHeadVm> GetBankCashAccountHeadList()
        {
            return _ledgerRepository.GetBankCashAccountHeadList();
        }
        public List<AccountHeadVm> GetLCAccountHeadList()
        {
            return _ledgerRepository.GetLCAccountHeadList();
        }
    }
}