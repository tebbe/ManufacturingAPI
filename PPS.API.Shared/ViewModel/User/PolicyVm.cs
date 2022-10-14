using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.API.Shared.ViewModel.User
{
    public class PolicyVm
    {
        public int Id { get; set; }
        public string PolicyName { get; set; }
        public string Description { get; set; }
        public bool Selected { get; set; }
        public int PolicyCode { get; set; }
        public int? AppTypeId { get; set; }
    }
}