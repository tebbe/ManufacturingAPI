using PPS.API.Shared.ViewModel.Account;
using PPS.API.Shared.ViewModel.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPS.Service.ServiceInterfaces
{
    public interface ITransactionInterface
    {
        TransactionModel SaveTransaction(TransactionModel transactionModel);
        List<TransactionModel> GetTransactionList(int fiscalYear, int companyId, int tranTypeId);
        List<TransactionListVm> GetUnapprovedAccountsTransaction(int fiscalYear, int companyId);
        List<TransactionListVm> GetUnapprovedSalesTransaction(int fiscalYear, int companyId);
        List<TransactionListVm> GetUnapprovedPurchaseTransaction(int fiscalYear, int companyId);
        TransactionModel UpdateTransaction(TransactionModel transactionModel);
        List<TransactionRejectReasonTypeVm> GetTransactionRejectReasonType();
        List<TransactionListVm> GetTransactionAccountsRejectedList(int fiscalYear, int companyId);
        List<TransactionListVm> GetTransactionByTransactionNo(int fiscalYear, int companyId, string transactionNo);
    }
}
