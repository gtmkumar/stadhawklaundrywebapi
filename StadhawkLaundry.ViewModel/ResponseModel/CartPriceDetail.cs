﻿using System;
using System.Collections.Generic;
using System.Text;

namespace StadhawkLaundry.ViewModel.ResponseModel
{
   public class CartPriceDetail
    {
        public CartPriceDetail()
        {
            ServiceData = new List<CartDetailResponseViewModel>();
            ServiceByKg = new List<ServiceByKgResponseViewModel>();
        }
        public int CartCount { get; set; }
        public decimal CartPrice { get; set; }
        public decimal KgCount { get; set; }
        public bool IsKg { get; set; }
        public bool IsValidShipment { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalPrice { get; set; }
        public List<CartDetailResponseViewModel> ServiceData { get; set; }
        public List<ServiceByKgResponseViewModel> ServiceByKg { get; set; }
    }
}
