using System;
using System.Collections.Generic;
using System.Text;

namespace StadhawkLaundry.ViewModel
{
    public class AssignRolesViewModel
    {
        public Guid? UserId { get; set; }
        public string UserName { get; set; }
        public Guid? RoleId { get; set; }
        public string RoleName { get; set; }
        public Guid? UserRolesId { get; set; }
    }
}
