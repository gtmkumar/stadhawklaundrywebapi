using System;
using System.Collections.Generic;

namespace StadhawkLaundry.DataModel.Models
{
    public partial class TblServiceCategoryMapping
    {
        public int ServiceId { get; set; }
        public int CategoryId { get; set; }

        public virtual TblCategoryMaster Category { get; set; }
        public virtual TblServiceMaster Service { get; set; }
    }
}
