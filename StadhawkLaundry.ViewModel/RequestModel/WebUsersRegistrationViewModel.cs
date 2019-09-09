using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StadhawkLaundry.ViewModel.RequestModel
{
    public class WebUsersRegistrationViewModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string EmailId { get; set; }
        [Required(ErrorMessage = "Contact no is required")]
        public string ContactNo { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
