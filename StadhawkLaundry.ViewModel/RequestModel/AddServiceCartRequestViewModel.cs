using System;
using System.Collections.Generic;
using System.Text;

namespace StadhawkLaundry.ViewModel.RequestModel
{
   public class AddServiceCartRequestViewModel
    {
        public int? CartId { get; set; }
        public int StoreId { get; set; }
        public int ServiceId { get; set; }
        public decimal? Quantity { get; set; }
        public bool IsCartRemoved { get; set; }
        public int AddressId { get; set; }
    }
}
