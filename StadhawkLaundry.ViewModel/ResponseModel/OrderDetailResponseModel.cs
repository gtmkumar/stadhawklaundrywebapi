using System;
using System.Collections.Generic;
using System.Text;

namespace StadhawkLaundry.ViewModel.ResponseModel
{
    public class OrderDetailResponseModel
    {
        public OrderDetailResponseModel()
        {
            Services = new List<OrderServiceResponseViewModel>();
        }
        public int OrderId { get; set; }
        public string OrderDate { get; set; }
        public int OrderStatus { get; set; }
        public decimal TotalKG { get; set; }
        public int ItemCount { get; set; }
        public bool IsKg { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal OrderAmount { get; set; }
        public string PickUpAddress { get; set; }
        public string PickupDateTime { get; set; }
        public string DeliveryDateTime { get; set; }
        public List<OrderServiceResponseViewModel> Services { get; set; }
    }
}
