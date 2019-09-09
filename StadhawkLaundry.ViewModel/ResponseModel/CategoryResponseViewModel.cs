using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StadhawkLaundry.ViewModel.ResponseModel
{
    public class CategoryResponseViewModel
    {
        public int CategoryId { get; set; }
        public int ServiceId { get; set; }
        public string Name { get; set; }
        public string IconUrl { get; set; }
    }
}
