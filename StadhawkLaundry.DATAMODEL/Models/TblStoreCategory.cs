using System;
using System.Collections.Generic;

namespace StadhawkLaundry.DataModel.Models
{
    public partial class TblStoreCategory
    {
        public long StoreId { get; set; }
        public int CategoryId { get; set; }

        public virtual TblCategoryMaster Category { get; set; }
        public virtual TblStore Store { get; set; }
    }
}
