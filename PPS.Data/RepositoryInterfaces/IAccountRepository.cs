using PPS.API.Shared.ViewModel.Account;
using PPS.Data.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PPS.Data.RepositoryInterfaces
{
    public interface IAccountRepository
    {
        List<AccountDto> GetLegerList(int fiscalYear, int companyId);
        List<AccountDto> GetAccountTypeList(int fiscalYear, int companyId);
        List<AccountDto> GetAccountPrimaryHeadListForLedger(int fiscalYear, int companyId);
        List<AccountDto> GetAccountPrimaryHeadList(int fiscalYear, int companyId);
        List<AccountDto> GetAccountSubHeadList(int companyId, int mainGroupId);
        AccountHeadModel SaveAccountHead(AccountHeadModel ledger);
        List<AccountHeadVm> GetBankCashAccountHeadList(int customerId);
        List<AccountHeadVm> GetSalesAccount();
        List<AccountHeadVm> GetBankCashAccountHeadList();
        List<AccountHeadVm> GetLCAccountHeadList();
    }
}