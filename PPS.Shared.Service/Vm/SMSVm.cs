using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.Shared.Service.Vm
{
    public class SMSVm
    {
        public string SmsText { get; set; }
        public List<string> Numbers { get; set; }
        public double Amount { get; set; }
        public DateTime DateOn { get; set; }
        public string BankThrough { get; set; }
    }
}