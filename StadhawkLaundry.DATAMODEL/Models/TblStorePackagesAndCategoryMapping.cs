using System;
using System.Collections.Generic;

namespace StadhawkLaundry.DataModel.Models
{
    public partial class TblStorePackagesAndCategoryMapping
    {
        public long Id { get; set; }
        public long StoreId { get; set; }
        public long PackageId { get; set; }
        public int? CategoryId { get; set; }
        public decimal? Price { get; set; }
        public int? Unit { get; set; }
        public int? Quantity { get; set; }
        public string Description { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }

        public virtual TblCategoryMaster Category { get; set; }
        public virtual TblStorePckages Package { get; set; }
        public virtual TblStore Store { get; set; }
    }
}
