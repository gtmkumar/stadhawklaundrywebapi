using System;
using System.Collections.Generic;
using System.Text;

namespace StadhawkLaundry.ViewModel.ResponseModel
{
    public class OrderCategoryResponceViewModel
    {
        public OrderCategoryResponceViewModel()
        {
            OrderItemList = new List<OrderItemDetailResponseViewModel>();
        }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<OrderItemDetailResponseViewModel> OrderItemList { get; set; }
    }
}
