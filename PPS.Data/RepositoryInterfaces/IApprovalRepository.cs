using PPS.API.Shared.ViewModel.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.Data.RepositoryInterfaces
{
    public interface IApprovalRepository
    {
        bool VerifyTransaction(AcceptRejectTransactionVm vm);
        bool AcceptTransaction(AcceptRejectTransactionVm vm);
        bool RejectTransaction(AcceptRejectTransactionVm vm);
    }
}