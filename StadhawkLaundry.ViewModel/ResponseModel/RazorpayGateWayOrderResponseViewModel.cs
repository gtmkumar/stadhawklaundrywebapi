using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace StadhawkLaundry.ViewModel.ResponseModel
{
    public class RazorpayGateWayOrderResponseViewModel
    {
        public RazorpayGateWayOrderResponseViewModel()
        {
            Attributes = new OrderAttributes();
        }
        [JsonProperty("Attributes")]
        public OrderAttributes Attributes { get; set; }

    }
    public partial class OrderAttributes
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("entity")]
        public string Entity { get; set; }

        [JsonProperty("amount")]
        public long Amount { get; set; }

        [JsonProperty("amount_paid")]
        public long AmountPaid { get; set; }

        [JsonProperty("amount_due")]
        public long AmountDue { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("receipt")]
        public string Receipt { get; set; }

        [JsonProperty("offer_id")]
        public object OfferId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("attempts")]
        public long Attempts { get; set; }

        [JsonProperty("notes")]
        public Hashtable Notes { get; set; }

        [JsonProperty("created_at")]
        public long CreatedAt { get; set; }
    }
}
