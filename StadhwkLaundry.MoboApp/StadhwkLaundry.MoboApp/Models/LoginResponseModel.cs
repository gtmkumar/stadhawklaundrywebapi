using System;
using System.Collections.Generic;
using System.Text;

namespace StadhwkLaundry.MoboApp.Models
{
    public class LoginResponseModel
    {
        public string UserName { get; set; }
        public object Password { get; set; }
        public string Token { get; set; }
        public Guid Usertype { get; set; }
    }
}
