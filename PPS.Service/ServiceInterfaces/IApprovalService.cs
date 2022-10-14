using PPS.API.Shared.ViewModel.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPS.Service.ServiceInterfaces
{
    public interface IApprovalService
    {
        bool VerifyTransaction(AcceptRejectTransactionVm vm);
        bool AcceptTransaction(AcceptRejectTransactionVm vm);
        bool RejectTransaction(AcceptRejectTransactionVm vm);
    }
}
