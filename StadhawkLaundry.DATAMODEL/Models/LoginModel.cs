using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StadhawkLaundry.DataModel.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage ="User is Requared")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is Requared")]
        public string Password { get; set; }
    }
}
