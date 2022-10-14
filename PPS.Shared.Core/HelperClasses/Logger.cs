using PPS.Data.DbContexts;
using PPS.Data.Edmx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPS.Shared.Core.HelperClasses
{
    public class Logger : ILogger
    {
        LogContext _logger;
        public Logger()
        {
            _logger = new LogContext();
        }
        public void Log(string userId, string absoluteUrl, Exception ex)
        {
            var log = new Log()
            {
                UserId = userId,
                AbsolutePath = absoluteUrl,
                CreatedOn = DateTime.Now,
                ErrorMessage = ex.Message,
                InnerMessage = ex.InnerException?.ToString(),
                StackTrace = ex.StackTrace
            };
            _logger.Log.Add(log);
            _logger.SaveChanges();
        }

        public void Log(string userId, string absoluteUrl, string errorMessage)
        {
            var log = new Log
            {
                UserId = userId,
                AbsolutePath = absoluteUrl,
                CreatedOn = DateTime.Now,
                ErrorMessage = errorMessage,
                StackTrace = null
            };
            _logger.Log.Add(log);
            _logger.SaveChanges();
        }

        public void UserActivityLog(string email, string controllerName, string actionName, decimal elapsed, string requestUri, string referrerUri)
        {
            var userActivityLog = new UserActivityLog
            {
                UserEmail = email,
                ControllerName = controllerName,
                ActionName = actionName,
                Elapsed = elapsed,
                RequestUri = requestUri,
                ReferrerUri = referrerUri,
                CreatedOn = DateTime.Now
            };
            _logger.UserActivityLog.Add(userActivityLog);
            _logger.SaveChanges();
        }

        public void UserLoginLog(string email, bool isSucceeded, string errorMessage)
        {
            var userLoginLog = new UserLoginLog
            {
                UserEmail = email,
                IsSucceeded = isSucceeded,
                ErrorMessage = errorMessage,
                LoggedOn = DateTime.Now
            };
            _logger.UserLoginLog.Add(userLoginLog);
            _logger.SaveChanges();
        }
    }
}
