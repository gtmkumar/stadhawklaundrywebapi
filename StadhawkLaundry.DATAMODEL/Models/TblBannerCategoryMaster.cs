using System;
using System.Collections.Generic;

namespace StadhawkLaundry.DataModel.Models
{
    public partial class TblBannerCategoryMaster
    {
        public TblBannerCategoryMaster()
        {
            TblBanner = new HashSet<TblBanner>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }

        public virtual ICollection<TblBanner> TblBanner { get; set; }
    }
}
