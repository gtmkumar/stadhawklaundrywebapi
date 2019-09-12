using System;
using System.Collections.Generic;
using System.Text;

namespace StadhawkLaundry.ViewModel.ResponseModel
{
    public class CartCountResponseViewModel
    {
        public int CartCount { get; set; }
        public int CartPrice { get; set; }
        public int? CartId { get; set; }
    }
}
