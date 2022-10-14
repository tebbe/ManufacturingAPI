using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPS.Shared.Core.HelperClasses
{
    public interface ILogger
    {
        void Log(string userId, string absoluteUrl, Exception ex);
        void Log(string userId, string absoluteUrl, string errorMessage);
        void UserActivityLog(string email, string controllerName, string actionName, decimal elapsed, string requestUri, string referrerUri);
        void UserLoginLog(string email, bool isSucceeded, string errorMessage);
    }
}
