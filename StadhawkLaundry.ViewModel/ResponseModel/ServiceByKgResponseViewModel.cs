using System;
using System.Collections.Generic;
using System.Text;

namespace StadhawkLaundry.ViewModel.ResponseModel
{
    public class ServiceByKgResponseViewModel
    {
        public int StoreId { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public decimal pricePerKG { get; set; }
        public decimal QuantityInKG { get; set; }
        public string ServiceImageUrl { get; set; }
        public int CartId { get; set; }
    }
}
