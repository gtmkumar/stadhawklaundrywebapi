using System;
using System.Collections.Generic;

namespace StadhawkLaundry.DataModel.Models
{
    public partial class TblCart
    {
        public int Id { get; set; }
        public int? StoreItemId { get; set; }
        public int? Quantity { get; set; }
        public int? UserId { get; set; }
        public bool? IsOrderPlaced { get; set; }
        public int? CartPrice { get; set; }
        public byte? GstId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public bool? IsDeleted { get; set; }
        public int AddressId { get; set; }
    }
}
