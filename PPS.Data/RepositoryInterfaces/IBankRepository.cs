using PPS.API.Shared.ViewModel.Bank;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PPS.Data.RepositoryInterfaces
{
    public interface IBankRepository
    {
        List<BankVm> GetBankList();
    }
}