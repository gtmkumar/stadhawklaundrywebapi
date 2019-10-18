using System;
using System.Collections.Generic;
using System.Text;

namespace StadhawkLaundry.ViewModel.ResponseModel
{
    public class OrderDetailResponseViewModel
    {
        public int OrderId { get; set; }
        public string OrderRef { get; set; }
        public string OrderDate { get; set; }
        public int TotalKg { get; set; }
        public decimal TotalPrice { get; set; }
        public int OrderStatusId { get; set; }
        public int ItemCount { get; set; }
        public bool IsKG { get; set; }
        public bool isRepeatOrder { get; set; }
        public string OrderStatus { get; set; }
        public string PickName { get; set; }
        public string PickUpAddress { get; set; }
        public string DeliveryName { get; set; }
        public string DeliveryAddress { get; set; }
        public bool IsCod { get; set; }
    }
}
