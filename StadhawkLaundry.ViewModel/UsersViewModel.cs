using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StadhawkLaundry.ViewModel
{
    public class UsersViewModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string EmailId { get; set; }
        [Required(ErrorMessage = "Contact no is required")]
        public string ContactNo { get; set; }
        public bool Status { get; set; }
    }
}
