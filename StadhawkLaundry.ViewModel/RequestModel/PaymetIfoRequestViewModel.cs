using System;
using System.Collections.Generic;
using System.Text;

namespace StadhawkLaundry.ViewModel.RequestModel
{
    public class PaymetIfoRequestViewModel
    {
        public string InvoiceNo { get; set; }
        public string PgRequest { get; set; }
        public string PgResponse { get; set; }
        public int? TotalPayment { get; set; }
        public string PgType { get; set; }
        public string PGTransactionId { get; set; }
        public string Status { get; set; }
    }
}
