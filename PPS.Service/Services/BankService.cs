using PPS.Service.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PPS.Data.Dtos;
using PPS.Data.RepositoryInterfaces;
using PPS.Data.Repositories;
using PPS.API.Shared.ViewModel.Account;
using PPS.API.Shared.ViewModel.Sales;
using System.Threading.Tasks;
using PPS.API.Shared.ViewModel.Bank;

namespace PPS.Service.Services
{
    public class BankService : IBankService
    {
        private IBankRepository _bankRepository;
        public BankService()
        {
            _bankRepository = new BankRepository();
        }

        public List<BankVm> GetBankList()
        {
            return _bankRepository.GetBankList();
        }
    }
}