using PPS.API.Shared.ViewModel.Account;
using PPS.Data.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.Service.ServiceInterfaces
{
    public interface ILedgerInterface
    {
        List<AccountHeadModel> GetLegerList(int fiscalYear, int companyId);
        List<AccountTypeModel> GetAccountTypeList(int fiscalYear, int companyId);
        List<AccountPrimaryHeadModel> GetAccountPrimaryHeadListForLedger(int fiscalYear, int companyId);
        List<AccountPrimaryHeadModel> GetAccountPrimaryHeadList(int fiscalYear, int companyId);
        List<AccountSubHeadModel> GetAccountSubHeadList(int companyId, int accountPrimaryHeadId);
        AccountHeadModel SaveAccountHead(AccountHeadModel ledger);
        List<AccountHeadModel> GetAccountHeadList(int fiscalYear, int companyId);
        List<AccountHeadVm> GetBankCashAccountHeadList(int customerId);
        List<AccountHeadVm> GetSalesAccount();
        List<AccountHeadVm> GetBankCashAccountHeadList();
        List<AccountHeadVm> GetLCAccountHeadList();
    }
}