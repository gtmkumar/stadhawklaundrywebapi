using System;
using System.Collections.Generic;

namespace StadhawkLaundry.DataModel.Models
{
    public partial class TblOrderItems
    {
        public long Id { get; set; }
        public long? OrderId { get; set; }
        public long? StoreItemId { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public bool? IsOrderPicked { get; set; }

        public virtual TblOrder Order { get; set; }
        public virtual TblStoreItems StoreItem { get; set; }
    }
}
