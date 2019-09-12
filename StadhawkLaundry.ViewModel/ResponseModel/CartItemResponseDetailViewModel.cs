using System;
using System.Collections.Generic;
using System.Text;

namespace StadhawkLaundry.ViewModel.ResponseModel
{
    public class CartItemResponseDetailViewModel
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public int CartId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
