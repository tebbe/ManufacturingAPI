using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.API.Shared.ViewModel.Company
{
    public class CompanyVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string ContactPerson { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string LogoPath { get; set; }
        public string Email { get; set; }
        public int GroupId { get; set; }
        public int AllowedInvalid { get; set; }
    }
}