using System;
using System.Collections.Generic;
using System.Text;

namespace StadhawkLaundry.ViewModel
{
    public class ItemViewModel
    {
        public string ItemId { get; set; }
        public Guid? ServiceId { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? SubcategoryId { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public string ServiceName { get; set; }
        public string CategoryName { get; set; }
        public string SubcategoryName { get; set; }
    }
}
