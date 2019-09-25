using System;
using System.Collections.Generic;
using System.Text;

namespace StadhawkLaundry.ViewModel.ResponseModel
{
    public class OrderServiceResponseViewModel
    {
        public OrderServiceResponseViewModel()
        {
            Categories = new List<OrderCategoryResponceViewModel>();
        }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string ServiceUrl { get; set; }
        public bool IsKg { get; set; }
        public List<OrderCategoryResponceViewModel> Categories { get; set; }
    }
}
