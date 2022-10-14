using PPS.API.Shared.ViewModel.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.Data.Dtos.User
{
    public class PolicyDto
    {
        public int Id { get; set; }
        public string PolicyName { get; set; }
        public string Description { get; set; }
        public int PolicyCode { get; set; }
        public bool Selected { get; set; }
        public int? AppTypeId { get; internal set; }

        public PolicyVm ToPolicyVm()
        {
            var vm = new PolicyVm
            {
                Id = this.Id,
                PolicyName = this.PolicyName,
                Description = this.Description,
                PolicyCode = this.PolicyCode,
                AppTypeId = this.AppTypeId,
                Selected = this.Selected
            };

            return vm;
        }
    }
}