using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPS.Core
{
    public class ActionStatus
    {
        public ActionStatus (bool success)
        {
            this.Messages = new List<string>();
            this.Success = success;
            this.Messages.Add(success ? "The operation completed successfully." : "There was a problem completing the operation.");
        }
        public ActionStatus (Exception exception)
        {
            this.Messages = new List<string>();
            this.Success = false;
            this.Messages.Add(exception.Message);

            var inner = exception.InnerException;
            while (inner != null)
            {
                this.Messages.Add(inner.Message);
                inner = inner.InnerException;
            }
        }
        public string MessageString
        {
            get { return String.Join("|", this.Messages); }
        }
        public ActionStatus AddMessage(string message)
        {
            this.Messages = new List<string>();
            this.Messages.Add(message);
            return this;
        }

        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public object ResponseData { get; set; }
        //public bool IsWarning { get; set; }
        public List<string> Messages { get; set; }
        //public Exception Exception { get; set; }               
    }
}
