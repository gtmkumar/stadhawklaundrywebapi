using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StadhawkLaundry.ViewModel
{
    public class CategoryViewModel
    {
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Service is Required")]
        public int ServiceId { get; set; }

        [Required(ErrorMessage = "Category Name is Required")]
        public string Name { get; set; }
        public string Service { get; set; }
    }
}
