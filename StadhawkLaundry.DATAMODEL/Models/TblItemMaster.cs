using System;
using System.Collections.Generic;

namespace StadhawkLaundry.DataModel.Models
{
    public partial class TblItemMaster
    {
        public TblItemMaster()
        {
            TblStoreItems = new HashSet<TblStoreItems>();
        }

        public int Id { get; set; }
        public int? CategoryId { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual TblCategoryMaster Category { get; set; }
        public virtual ICollection<TblStoreItems> TblStoreItems { get; set; }
    }
}
