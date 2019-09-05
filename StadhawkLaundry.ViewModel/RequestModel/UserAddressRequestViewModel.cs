using System;
using System.Collections.Generic;
using System.Text;

namespace StadhawkLaundry.ViewModel.RequestModel
{
    public class UserAddressRequestViewModel
    {
        public int? AddressId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string LandMark { get; set; }
        public int AddressTypeId { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public bool? IsDefaultDeliveryLocation { get; set; }
    }
}
