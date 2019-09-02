using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StadhawkLaundry.API.Models
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string SmsUserUrl { get; set; }
        public string SmsHasKey { get; set; }
        public string SmsVendorUrl { get; set; }
    }
}
