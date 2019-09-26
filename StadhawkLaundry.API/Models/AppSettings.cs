using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StadhawkLaundry.API.Models
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string SmsUserUrl { get; set; }
        public string SmsHasKey { get; set; }
        public string SmsVendorUrl { get; set; }
        public string DataBaseCon { get; set; }
        public string PGType { get; set; }
        public string ReturnUrl { get; set; }
        public string TransactionCurrency { get; set; }
        public string RazorpayKey { get; set; }
        public string RazorpaySecret { get; set; }
        public string Company { get; set; }
        public string RazorPayDescription { get; set; }
        public string RazorPayLogo { get; set; }
        public string RazorPayColor { get; set; }
        public string PAYTM_MID { get; set; }
        public string PAYTM_WEBSITE { get; set; }
        public string PAYTM_INDUSTRY_TYPE_ID { get; set; }
        public string PAYTM_CHANNEL_ID { get; set; }
        public string PAYTM_MERCHANT_KEY { get; set; }
        public string IsLivePayment { get; set; }
        public string PAYTM_LIVE_URL { get; set; }
        public string PAYTM_STAGING_URL { get; set; }
        public string RespHashKey { get; set; }
        public string RazorPayReturnUrl { get; set; }
        public string PAYTM_TRANSACTION_STATUS_LIVE_URL { get; set; }
        public string PAYTM_TRANSACTION_STATUS_STAGING_URL { get; set; }
        public string PgVisibility { get; set; }
        public string PaytmVisibility { get; set; }
        public string CODVisibility { get; set; }
        public string CODVisibilityForMealPass { get; set; }
    }
}
