using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PPS.API.Shared.ViewModel.User
{
    public class UserRegisterVm
    {
        [Required]
        //[Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Password { get; set; }
        public string PasswordKey { get; set; }
        [Required]
        public int CompanyId { get; set; }
        public int? EmployeeId { get; set; }
    }
}