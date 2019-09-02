using System;
using System.Collections.Generic;

namespace StadhawkLaundry.DataModel.Models
{
    public partial class TblItemPrinceMappingByStore
    {
        public Guid Id { get; set; }
        public Guid? ItemId { get; set; }
        public Guid? StoreId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public decimal? Price { get; set; }

        public virtual TblItem Item { get; set; }
        public virtual TblStore Store { get; set; }
    }
}
