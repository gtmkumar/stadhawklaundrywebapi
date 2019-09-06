using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StadhawkLaundry.ViewModel
{
    public class LoginRequestViewModel
    {
        [Required(ErrorMessage = "Mobile No is requerd")]
        public string MobileNo { get; set; }
        [Required(ErrorMessage = "OTP is requerd")]
        public string OTP { get; set; }
        [Required(ErrorMessage = "Fcm tocken is requerd")]
        public string FcmToken { get; set; }
        [Required(ErrorMessage = "Device id is requerd")]
        public string DeviceId { get; set; }
        [Required(ErrorMessage = "Device type is requerd")]
        public string DeviceType { get; set; }
    }
}
