using System;
using System.Collections.Generic;
using System.Text;

namespace StadhawkLaundry.ViewModel.ResponseModel
{
    public class DeliveryBoyOrderDetailResponseModel
    {
        public DeliveryBoyOrderDetailResponseModel()
        {
            Services = new List<OrderServiceResponseViewModel>();
        }
        public int OrderId { get; set; }
        public string OrderRef { get; set; }
        public string OrderDate { get; set; }
        public int OrderStatusId { get; set; }
        public string OrderStatus { get; set; }
        public decimal TotalKG { get; set; }
        public int ItemCount { get; set; }
        public bool IsKg { get; set; }
        public decimal TotalPrice { get; set; }
        public string PickupDateTime { get; set; }
        public string DeliveryDateTime { get; set; }
        public bool IsCOD { get; set; }
        public string PickName { get; set; }
        public string PickUpAddress { get; set; }
        public string PickUpContact { get; set; }
        public decimal PickUpLat { get; set; }
        public decimal PickUpLong { get; set; }
        public string DeliveryName { get; set; }
        public string DeliveryAddress { get; set; }
        public string DeliveryContact { get; set; }
        public decimal DeliveryLat { get; set; }
        public decimal DeliveryLong { get; set; }
        public List<OrderServiceResponseViewModel> Services { get; set; }
    }
}
