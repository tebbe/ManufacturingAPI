using PPS.Service.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PPS.API.Shared.ViewModel.Account;
using PPS.Data.RepositoryInterfaces;
using PPS.Data.Repositories;

namespace PPS.Service.Services
{
    public class ApprovalService : IApprovalService
    {
        private IApprovalRepository _approvalRepository;
        public ApprovalService()
        {
            _approvalRepository = new ApprovalRepository();
        }
        public bool VerifyTransaction(AcceptRejectTransactionVm vm)
        {
            return _approvalRepository.VerifyTransaction(vm);
        }

        public bool AcceptTransaction(AcceptRejectTransactionVm vm)
        {
            return _approvalRepository.AcceptTransaction(vm);
        }

        public bool RejectTransaction(AcceptRejectTransactionVm vm)
        {
            return _approvalRepository.RejectTransaction(vm);
        }
    }
}