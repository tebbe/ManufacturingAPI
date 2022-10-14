using System.Collections.Generic;
using System.Threading.Tasks;
using PPS.API.Shared.ViewModel.Process;

namespace PPS.Operation.Operation.Service
{
    public interface IMonthlySalesProcess
    {
        Task<bool> ProcessMonthlyAchievement(int month, int year, int userId, bool reprocess);
        Task<List<MonthlySalesProcessVm>> GetMonthlySalesProcess();
    }
}