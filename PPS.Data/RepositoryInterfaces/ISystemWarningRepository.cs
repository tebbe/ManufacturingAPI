using PPS.Data.Edmx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PPS.Data.RepositoryInterfaces
{
    public interface ISystemWarningRepository
    {
        List<SystemWarningType> GetSystemWarningType();
        IQueryable<SystemWarning> GetSystemWarning();
        SystemWarningHistory SaveSystemWarningHistory(SystemWarningHistory systemWarningHistory);
        SystemWarning SaveSystemWarning(SystemWarning systemWarning);
        SystemWarning UpdateSystemWarning(SystemWarning systemWarning);
        List<SystemWarning> UpdateBulkSystemWarning(List<SystemWarning> systemWarning);
    }
}