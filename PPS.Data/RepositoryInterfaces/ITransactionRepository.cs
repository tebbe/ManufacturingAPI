using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PPS.API.Shared.ViewModel.Transaction;
using PPS.Data.Dtos.Transaction;
using PPS.Data.Edmx;

namespace PPS.Data.RepositoryInterfaces
{
    public interface ITransactionRepository
    {
        TransactionModel SaveTransaction(TransactionModel transactionEntry);
        List<TransactionDto> GetTransactionList(int fiscalYear, int companyId, int transactionTypeId);
        List<TransactionListDto> GetUnapprovedAccountsTransaction(int fiscalYear, int companyId);
        List<TransactionListDto> GetUnapprovedSalesTransaction(int fiscalYear, int companyId);
        List<TransactionListDto> GetUnapprovedPurchaseTransaction(int fiscalYear, int companyId);
        TransactionModel UpdateTransaction(TransactionModel transactionModel);
        List<TransactionRejectReasonTypeDto> GetTransactionRejectReasonType();
        string CreateTransactionNumber(int tranType, DateTime tranDate, int number);
        List<TransactionListDto> GetTransactionAccountsRejectedList(int fiscalYear, int companyId);
        List<TransactionListDto> GetTransactionByTransactionNo(int fiscalYear, int companyId, string transactionNo);

        IQueryable<TransactionEntry> GetTransactionEntry();
        IQueryable<AccountSubHead> GetAccountSubHead();
        IQueryable<AccountHead> GetAccountHead();
    }
}
