using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace StadhawkLaundry.ViewModel.ResponseModel
{
    public class RazorpayGateWayResponseViewModel
    {
        public RazorpayGateWayResponseViewModel()
        {
            Attributes = new PaymentAttributes();
        }
        [JsonProperty("Attributes")]
        public PaymentAttributes Attributes { get; set; }
    }
    public class PaymentAttributes
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("entity")]
        public string Entity { get; set; }

        [JsonProperty("amount")]
        public long Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("order_id")]
        public string OrderId { get; set; }

        [JsonProperty("invoice_id")]
        public object InvoiceId { get; set; }

        [JsonProperty("international")]
        public bool International { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("amount_refunded")]
        public long AmountRefunded { get; set; }

        [JsonProperty("refund_status")]
        public object RefundStatus { get; set; }

        [JsonProperty("captured")]
        public bool Captured { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("card_id")]
        public object CardId { get; set; }

        [JsonProperty("bank")]
        public string Bank { get; set; }

        [JsonProperty("wallet")]
        public object Wallet { get; set; }

        [JsonProperty("vpa")]
        public object Vpa { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("contact")]
        public string Contact { get; set; }

        [JsonProperty("fee")]
        public long Fee { get; set; }

        [JsonProperty("tax")]
        public long Tax { get; set; }

        [JsonProperty("error_code")]
        public object ErrorCode { get; set; }

        [JsonProperty("error_description")]
        public object ErrorDescription { get; set; }

        [JsonProperty("created_at")]
        public long CreatedAt { get; set; }
    }
}
