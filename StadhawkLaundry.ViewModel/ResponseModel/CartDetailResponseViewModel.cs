using System;
using System.Collections.Generic;
using System.Text;

namespace StadhawkLaundry.ViewModel.ResponseModel
{
    public class CartDetailResponseViewModel
    {
        public CartDetailResponseViewModel()
        {
            CategoryData = new List<CartCategoryResponceViewModel>();
        }
        public int StoreId { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string ServiceUrl { get; set; }
        public List<CartCategoryResponceViewModel> CategoryData { get; set; }
    }
}
