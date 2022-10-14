using PPS.API.Shared.ViewModel.Bank;
using PPS.API.Shared.ViewModel.Sales;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PPS.Service.ServiceInterfaces
{
    public interface IBankService
    {
        List<BankVm> GetBankList();
    }
}