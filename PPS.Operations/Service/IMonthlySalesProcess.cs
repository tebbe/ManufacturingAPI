using PPS.API.Shared.ViewModel.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PPS.Operations.Service
{
    public interface IMonthlySalesProcess
    {
        Task<bool> ProcessMonthlyAchievement(int month, int year, int userId, bool reprocess);
        Task<List<MonthlySalesProcessVm>> GetMonthlySalesProcess();
    }
}