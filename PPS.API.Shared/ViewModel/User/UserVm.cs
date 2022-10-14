using System.Collections.Generic;

namespace PPS.API.Shared.ViewModel.User
{
    public class UserVm
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int CompanyId { get; set; }
        public string AspNetUserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Locked { get; set; }
        public string Status { get; set; }
        public int AssignedUserId { get; set; }

        public List<RoleVm> Roles { get; set; }
    }
}