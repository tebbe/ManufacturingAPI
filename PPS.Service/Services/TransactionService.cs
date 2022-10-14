using PPS.Service.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PPS.API.Shared.ViewModel.Transaction;
using PPS.Data.RepositoryInterfaces;
using PPS.Data.Repositories;
using PPS.API.Shared.ViewModel.Account;

namespace PPS.Service.Services
{
    public class TransactionService : ITransactionInterface
    {
        ITransactionRepository _tranRepo;
        public TransactionService()
        {
            _tranRepo = new TransactionRepository();
        }
        public TransactionModel SaveTransaction(TransactionModel transactionModel)
        {
            var transactionDto = _tranRepo.SaveTransaction(transactionModel);
            return transactionDto;
        }

        public TransactionModel UpdateTransaction(TransactionModel transactionModel)
        {
            var transactionDto = _tranRepo.UpdateTransaction(transactionModel);
            return transactionDto;
        }

        public List<TransactionModel> GetTransactionList(int fiscalYear, int companyId, int tranTypeId)
        {
            var tranList = _tranRepo.GetTransactionList(fiscalYear, companyId, tranTypeId);
            if (tranList == null)
                return null;
            var list = new List<TransactionModel>();
            foreach (var tran in tranList)
            {
                list.Add(tran.ToTransactionModel());
            }
            return list;
        }

        public List<TransactionListVm> GetUnapprovedAccountsTransaction(int fiscalYear, int companyId)
        {
            var tranList = _tranRepo.GetUnapprovedAccountsTransaction(fiscalYear, companyId);
            if (tranList == null)
                return null;
            var list = new List<TransactionListVm>();
            foreach (var tran in tranList)
            {
                list.Add(tran.ToTransactionVm());
            }
            return list;
        }
        public List<TransactionListVm> GetUnapprovedSalesTransaction(int fiscalYear, int companyId)
        {
            var tranList = _tranRepo.GetUnapprovedSalesTransaction(fiscalYear, companyId);
            if (tranList == null)
                return null;
            var list = new List<TransactionListVm>();
            foreach (var tran in tranList)
            {
                list.Add(tran.ToTransactionVm());
            }
            return list;
        }
        public List<TransactionListVm> GetUnapprovedPurchaseTransaction(int fiscalYear, int companyId)
        {
            var tranList = _tranRepo.GetUnapprovedPurchaseTransaction(fiscalYear, companyId);
            if (tranList == null)
                return null;
            var list = new List<TransactionListVm>();
            foreach (var tran in tranList)
            {
                list.Add(tran.ToTransactionVm());
            }
            return list;
        }

        public List<TransactionRejectReasonTypeVm> GetTransactionRejectReasonType()
        {
            var tranList = _tranRepo.GetTransactionRejectReasonType();
            if (tranList == null)
                return null;
            var list = new List<TransactionRejectReasonTypeVm>();
            foreach (var tran in tranList)
            {
                list.Add(tran.ToTransactionRejectReasonTypeVm());
            }
            return list;
        }

        public List<TransactionListVm> GetTransactionAccountsRejectedList(int fiscalYear, int companyId)
        {
            var tranList = _tranRepo.GetTransactionAccountsRejectedList(fiscalYear, companyId);
            if (tranList == null)
                return null;
            var list = new List<TransactionListVm>();
            foreach (var tran in tranList)
            {
                list.Add(tran.ToTransactionVm());
            }
            return list;
        }

        public List<TransactionListVm> GetTransactionByTransactionNo(int fiscalYear, int companyId, string transactionNo)
        {
            var tranList = _tranRepo.GetTransactionByTransactionNo(fiscalYear, companyId, transactionNo);
            if (tranList == null)
                return null;
            var list = new List<TransactionListVm>();
            foreach (var tran in tranList)
            {
                list.Add(tran.ToTransactionVm());
            }
            return list;
        }
    }
}