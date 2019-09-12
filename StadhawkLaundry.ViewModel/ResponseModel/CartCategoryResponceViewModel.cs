using System;
using System.Collections.Generic;
using System.Text;

namespace StadhawkLaundry.ViewModel.ResponseModel
{
   public class CartCategoryResponceViewModel
    {
        public CartCategoryResponceViewModel()
        {
            ItemsData = new List<CartItemResponseDetailViewModel>();
        }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<CartItemResponseDetailViewModel> ItemsData { get; set; }
    }
}
