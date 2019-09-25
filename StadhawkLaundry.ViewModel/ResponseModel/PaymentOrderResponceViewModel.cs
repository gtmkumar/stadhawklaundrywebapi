using System;
using System.Collections.Generic;
using System.Text;

namespace StadhawkLaundry.ViewModel.ResponseModel
{
    public class PaymentOrderResponceViewModel
    {
        public string InvoiceNo { get; set; }
        public decimal Price { get; set; }
        public string EmailId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerId { get; set; }
        public string MobileNo { get; set; }
        public string DeviceType { get; set; }
        public bool IsBookingOn { get; set; }
    }
}
