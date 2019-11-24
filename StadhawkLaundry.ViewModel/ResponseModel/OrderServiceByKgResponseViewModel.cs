using System;
using System.Collections.Generic;
using System.Text;

namespace StadhawkLaundry.ViewModel.ResponseModel
{
    public class OrderServiceByKgResponseViewModel
    {
        public int OrderId { get; set; }
        public int StoreId { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Quantity { get; set; }
        public string ServiceImage { get; set; }
    }
}
