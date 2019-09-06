using System;
using System.Collections.Generic;

namespace StadhawkLaundry.DataModel.Models
{
    public partial class TblStoreItems
    {
        public TblStoreItems()
        {
            TblOrderItems = new HashSet<TblOrderItems>();
        }

        public long Id { get; set; }
        public long? StoreId { get; set; }
        public int? ItemId { get; set; }
        public int? ServiceId { get; set; }
        public int? SubServiceId { get; set; }
        public int? Quantity { get; set; }
        public int? UnitId { get; set; }
        public decimal? Price { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }

        public virtual TblItemMaster Item { get; set; }
        public virtual TblServiceMaster Service { get; set; }
        public virtual TblStore Store { get; set; }
        public virtual TblSubServiceMaster SubService { get; set; }
        public virtual TblUnitMaster Unit { get; set; }
        public virtual ICollection<TblOrderItems> TblOrderItems { get; set; }
    }
}
