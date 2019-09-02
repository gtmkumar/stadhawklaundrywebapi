using System;
using System.Collections.Generic;
using System.Text;

namespace StadhawkLaundry.ViewModel
{
    public class LoginResponse
    {
        public string UserName { get; set; }
        public string EmailId { get; set; }
        public string MobileNo { get; set; }
        public string Token { get; set; }
    }
}
