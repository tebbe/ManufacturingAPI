using System.Collections.Generic;

namespace PPS.API.Shared.ViewModel
{
    public class UserModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public int CompanyId { get; set; }
        public int StatusId { get; set; }
        public IList<RoleModel> UserRoles { get; set; }
    }
}