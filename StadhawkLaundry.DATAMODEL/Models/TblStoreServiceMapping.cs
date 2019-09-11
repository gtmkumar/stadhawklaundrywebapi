using System;
using System.Collections.Generic;

namespace StadhawkLaundry.DataModel.Models
{
    public partial class TblStoreServiceMapping
    {
        public long StoreId { get; set; }
        public int ServiceId { get; set; }
        public int UnitId { get; set; }
        public decimal? UnitPrice { get; set; }

        public virtual TblServiceMaster Service { get; set; }
        public virtual TblStore Store { get; set; }
    }
}
