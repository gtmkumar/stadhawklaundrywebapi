using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StadhawkLaundry.ViewModel.RequestModel
{
    public class WebLoginRequestViewModel
    {
        [Required(ErrorMessage = "User is requerd")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is requerd")]
        public string Password { get; set; }
        public bool IsLogedIn { get; set; }
    }
}
