using System;
using System.Collections.Generic;
using System.Text;

namespace StadhawkLaundry.ViewModel
{
    public class ItemViewModel
    {
        public int ItemId { get; set; }
        public int ServiceId { get; set; }
        public int CategoryId { get; set; }
        public int SubcategoryId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ServiceName { get; set; }
        public string CategoryName { get; set; }
        public string SubcategoryName { get; set; }
    }
}
