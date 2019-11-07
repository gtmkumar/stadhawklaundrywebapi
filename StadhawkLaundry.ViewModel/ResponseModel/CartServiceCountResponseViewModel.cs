using System;
using System.Collections.Generic;
using System.Text;

namespace StadhawkLaundry.ViewModel.ResponseModel
{
    public class CartServiceCountResponseViewModel
    {
        public decimal CartCount { get; set; }
        public decimal CartPrice { get; set; }
        public int? CartId { get; set; }
        public bool IsKg { get; set; }
    }
}
