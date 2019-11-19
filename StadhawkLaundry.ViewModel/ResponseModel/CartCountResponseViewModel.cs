using System;
using System.Collections.Generic;
using System.Text;

namespace StadhawkLaundry.ViewModel.ResponseModel
{
    public class CartCountResponseViewModel
    {
        public int CartCount { get; set; }
        public decimal CartPrice { get; set; }
        public decimal KgCount { get; set; }
        public int? CartId { get; set; }
        public bool IsKg { get; set; }
    }
}
