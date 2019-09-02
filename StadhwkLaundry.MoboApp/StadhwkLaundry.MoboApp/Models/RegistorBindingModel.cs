using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StadhwkLaundry.MoboApp.Models
{
    public class RegistorBindingModel
    {
        public string Id { get; set; }
        public string EmailId { get; set; }
        public string Password { get; set; }
        public string Contactno { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
    }
}
