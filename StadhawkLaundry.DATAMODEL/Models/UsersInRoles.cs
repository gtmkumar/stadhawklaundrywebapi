using System;
using System.Collections.Generic;

namespace StadhawkLaundry.DataModel.Models
{
    public partial class UsersInRoles
    {
        public Guid UserRolesId { get; set; }
        public Guid? RoleId { get; set; }
        public Guid? UserId { get; set; }
    }
}
