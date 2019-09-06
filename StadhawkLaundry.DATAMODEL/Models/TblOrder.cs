using System;
using System.Collections.Generic;

namespace StadhawkLaundry.DataModel.Models
{
    public partial class TblOrder
    {
        public TblOrder()
        {
            TblOrderItems = new HashSet<TblOrderItems>();
        }

        public long Id { get; set; }
        public int? CustomerId { get; set; }
        public long? StoreId { get; set; }
        public int? OrderStatusId { get; set; }
        public decimal? OrderAmount { get; set; }
        public long? PackageId { get; set; }
        public decimal? PackageDiscount { get; set; }
        public long? CouponId { get; set; }
        public decimal? CouponDiscount { get; set; }
        public decimal? TotalDiscount { get; set; }
        public int? Gst { get; set; }
        public decimal? Igst { get; set; }
        public decimal? Cgst { get; set; }
        public decimal? Sgst { get; set; }
        public decimal? ShippingCharge { get; set; }
        public decimal? GrandTotal { get; set; }
        public DateTime? OrderDate { get; set; }
        public string InvoiceNo { get; set; }
        public decimal? TotalItemKg { get; set; }
        public int? TotalItemPc { get; set; }
        public int? TotalIbags { get; set; }
        public int? PaymentType { get; set; }
        public string CustomerNotes { get; set; }
        public string OnServiceNotes { get; set; }
        public string DeliverNotes { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? PickupBoy { get; set; }
        public int? DeliveryBoy { get; set; }
        public DateTime? PickupDate { get; set; }
        public DateTime? DeliveryDate { get; set; }

        public virtual ICollection<TblOrderItems> TblOrderItems { get; set; }
    }
}
