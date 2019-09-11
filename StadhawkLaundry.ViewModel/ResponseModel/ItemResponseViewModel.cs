using System;
using System.Collections.Generic;
using System.Text;

namespace StadhawkLaundry.ViewModel.ResponseModel
{
    public class ItemResponseViewModel
    {
        public int StoreId { get; set; }
        public int ServiceId { get; set; }
        public int CategoryId { get; set; }
        public int StoreItemId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int CartId { get; set; }
        public int Quantity { get; set; }
        public string Image { get; set; }
        public bool IsKg { get; set; }
    }
}
