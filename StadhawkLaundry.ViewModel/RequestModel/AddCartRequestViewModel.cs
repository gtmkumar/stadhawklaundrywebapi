using System;
using System.Collections.Generic;
using System.Text;

namespace StadhawkLaundry.ViewModel.RequestModel
{
    public class AddCartRequestViewModel
    {
        public int? CartId { get; set; }
        public int StoreItemId { get; set; }
        public bool IsCartRemoved { get; set; }
        public int AddressId { get; set; }
        public int? Quantity { get; set; }
        public bool IsKg { get; set; }
    }
}
