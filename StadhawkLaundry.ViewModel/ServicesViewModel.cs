using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StadhawkLaundry.ViewModel
{
    public class ServicesViewModel
    {
        public string ServiceId { get; set; }

        [Required(ErrorMessage = "Service is requard")]
        public string Name { get; set; }

        public IFormFile ServiceImage { get; set; }
    }
}
