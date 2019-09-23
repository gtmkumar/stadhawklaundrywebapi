﻿using System;
using System.Collections.Generic;
using System.Text;

namespace StadhawkLaundry.ViewModel.ResponseModel
{
    public class OrderDetailResponseViewModel
    {
        public int OrderId { get; set; }
        public string OrderDate { get; set; }
        public int TotalKg { get; set; }
        public decimal TotalPrice { get; set; }
        public string OrderStatus { get; set; }
        public int ItemCount { get; set; }
        public bool IsKG { get; set; }
        public bool isRepeatOrder { get; set; }
    }
}
