using System;
using System.Collections.Generic;
using System.Text;

namespace StadhawkLaundry.ViewModel.RequestModel
{
    public class OrderStatusUpdateRequestModel
    {
        public int OrderId { get; set; }
        public int OrderStatusId { get; set; }
    }
}
