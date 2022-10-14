using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace PPS.Shared.Service.Vm
{
    public class EmailVm
    {
        public MailAddress FromAddress { get; set; }
        public MailAddress ToAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}