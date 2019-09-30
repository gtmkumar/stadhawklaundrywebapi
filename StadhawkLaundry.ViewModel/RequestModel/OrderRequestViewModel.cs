using System;
using System.Collections.Generic;
using System.Text;

namespace StadhawkLaundry.ViewModel.RequestModel
{
    public class OrderRequestViewModel
    {
        public int AddressId { get; set; }
        public int PickUpSlotId { get; set; }
        public string PickUpDate { get; set; }
        public string DeliverDate { get; set; }
        public int DeliverSlotId { get; set; }
        public string DeliveryNote { get; set; }
        public int ServiceId { get; set; }
        public int? PaymentType { get; set; }
    }
}
