using System;
using System.Collections.Generic;

namespace StadhawkLaundry.DataModel.Models
{
    public partial class TblPayment
    {
        public Guid Id { get; set; }
        public Guid? OrderId { get; set; }
        public int? PaymentMode { get; set; }
        public decimal? DeuAmount { get; set; }
        public int? PaymentStatus { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal? PaidAmmount { get; set; }
        public decimal? PaymentedAmount { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
