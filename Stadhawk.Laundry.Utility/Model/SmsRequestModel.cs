using System;
using System.Collections.Generic;
using System.Text;

namespace Stadhawk.Laundry.Utility.Model
{
    public class SmsRequestModel
    {
        public string SmsVendorUrl { get; set; }
        public string StrHasKey { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string OTP { get; set; }
    }
}
