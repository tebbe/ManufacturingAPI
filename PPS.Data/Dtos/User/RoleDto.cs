using PPS.API.Shared.ViewModel.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.Data.Dtos.User
{
    public class RoleDto
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public bool Selected { get; set; }

        public RoleVm ToRoleVm()
        {
            var vm = new RoleVm
            {
                Id = this.Id,
                RoleName = this.RoleName,
                Description = this.Description,
                Selected = this.Selected
            };

            return vm;
        }
    }
}