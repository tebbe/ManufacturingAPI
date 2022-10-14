using PPS.API.Shared.ViewModel.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.Data.Dtos.User
{
    public class UserDto
    {
        public int Id { get; set; }
        public string AspNetUserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Locked { get; set; }
        public string Status { get; set; }

        public UserVm ToUserVm()
        {
            var vm = new UserVm
            {
                Id = this.Id,
                AspNetUserId = this.AspNetUserId,
                Email = this.Email,
                Name = this.Name,
                Locked = this.Locked,
                Status = this.Status
            };

            return vm;
        }
    }
}