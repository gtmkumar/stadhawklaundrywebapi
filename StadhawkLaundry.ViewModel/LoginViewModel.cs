using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StadhawkLaundry.ViewModel
{
    public class LoginRequestViewModel
    {
        [Required(ErrorMessage = "Enter UserName")]
        public string UserId { get; set; }
        //[Required(ErrorMessage = "Enter Password")]
        //public string Password { get; set; }
        [Required(ErrorMessage = "Mobile No is requerd")]
        public string MobileNo { get; set; }
        public bool RememberMe { get; set; }
        public string OTP { get; set; }
        public string EmailId { get; set; }
        public string FcmToken { get; set; }
        public string DeviceType { get; set; }
        public string DeviceId { get; set; }
    }
}
