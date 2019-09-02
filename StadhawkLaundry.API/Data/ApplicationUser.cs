using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StadhawkLaundry.API.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public DateTime? DOB { get; set; }
        public string FCMToken { get; set; }
        public string DeviceType { get; set; }
        public string DeviceId { get; set; }
        public bool? IsGuestUser { get; set; }
        public string CustomerImage { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? Status { get; set; }
        public int? UserType { get; set; }
    }
}
