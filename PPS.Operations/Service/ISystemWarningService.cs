using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPS.Operations.Service
{
    public interface ISystemWarningService
    {
        bool CheckSystemWarning(int fiscalYear, int companyId, int userId);
    }
}
