using System;
using System.Collections.Generic;
using System.Text;

namespace StadhawkLaundry.ViewModel
{
    public class OrderViewModel
    {
        public OrderViewModel()
        {
            Services = new List<ServicesViewModel>();
            Categories = new List<CategoryViewModel>();
            Subcategories = new List<SubcategoryViewModel>();
            Items = new List<ItemViewModel>();
        }

        public List<ServicesViewModel> Services { get; set; }
        public List<CategoryViewModel> Categories { get; set; }
        public List<SubcategoryViewModel> Subcategories { get; set; }
        public List<ItemViewModel> Items { get; set; }
    }
}
