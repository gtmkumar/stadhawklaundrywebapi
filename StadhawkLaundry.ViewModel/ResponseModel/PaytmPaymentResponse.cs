using System;
using System.Collections.Generic;
using System.Text;

namespace StadhawkLaundry.ViewModel.ResponseModel
{
    public class PaytmPaymentResponse
    {
        public string ORDERID { get; set; }
        public string MID { get; set; }
        public string TXNID { get; set; }
        public string TXNAMOUNT { get; set; }
        public string PAYMENTMODE { get; set; }
        public string CURRENCY { get; set; }
        public DateTime TXNDATE { get; set; }
        public string STATUS { get; set; }
        public string RESPCODE { get; set; }
        public string RESPMSG { get; set; }
        public string GATEWAYNAME { get; set; }
        public string BANKTXNID { get; set; }
        public string BANKNAME { get; set; }
        public string IS_CHECKSUM_VALID { get; set; }
    }
}
