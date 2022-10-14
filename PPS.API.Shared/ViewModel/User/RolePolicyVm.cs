using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.API.Shared.ViewModel.User
{
    public class RolePolicyVm
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public List<PolicyVm> Policies { get; set; }
    }
}