using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.API.Shared.ViewModel.Account
{
    public class AccountHeadVm
    {
        public int Id { get; set; }
        public string AccountType { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
    }
}