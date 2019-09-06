﻿using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace StadhawkLaundry.DataModel.Models
{
    public class UserAddressDataViewModel
    {
        [Key]
        public int? AddressId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string LandMark { get; set; }
        public int? AddressTypeId { get; set; }
    }
}
