using System;
using System.Collections.Generic;

namespace StadhawkLaundry.DataModel.Models
{
    public partial class TblStorePckages
    {
        public TblStorePckages()
        {
            TblStorePackagesAndCategoryMapping = new HashSet<TblStorePackagesAndCategoryMapping>();
        }

        public long Id { get; set; }
        public long? StoreId { get; set; }
        public string PackageName { get; set; }
        public decimal? Price { get; set; }
        public int? ValidityInDays { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }

        public virtual TblStore Store { get; set; }
        public virtual ICollection<TblStorePackagesAndCategoryMapping> TblStorePackagesAndCategoryMapping { get; set; }
    }
}
