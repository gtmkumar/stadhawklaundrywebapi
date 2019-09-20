using System;
using System.Collections.Generic;
using System.Text;

namespace StadhawkLaundry.ViewModel.ResponseModel
{
    public class CartOrderDetailResponseViewModel
    {
        public int CartCount { get; set; }
        public decimal CartPrice { get; set; }
        public bool IsKg { get; set; }
        public bool IsValidShipment { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
