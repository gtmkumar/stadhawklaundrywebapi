﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StadhawkLaundry.ViewModel
{
    public class SubcategoryViewModel
    {
        public int ServiceId { get; set; }
        public string SubcategoryId { get; set; }
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Subcategory Name is Required")]
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal? Price { get; set; }
    }
}
