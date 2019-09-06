using System;
using System.Collections.Generic;

namespace StadhawkLaundry.DataModel.Models
{
    public partial class TblCategoryMaster
    {
        public TblCategoryMaster()
        {
            TblItemMaster = new HashSet<TblItemMaster>();
            TblStorePackagesAndCategoryMapping = new HashSet<TblStorePackagesAndCategoryMapping>();
        }

        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual ICollection<TblItemMaster> TblItemMaster { get; set; }
        public virtual ICollection<TblStorePackagesAndCategoryMapping> TblStorePackagesAndCategoryMapping { get; set; }
    }
}
