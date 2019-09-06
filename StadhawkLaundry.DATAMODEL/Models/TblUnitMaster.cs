using System;
using System.Collections.Generic;

namespace StadhawkLaundry.DataModel.Models
{
    public partial class TblUnitMaster
    {
        public TblUnitMaster()
        {
            TblStoreItems = new HashSet<TblStoreItems>();
        }

        public int Id { get; set; }
        public string UnitName { get; set; }
        public string Description { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual ICollection<TblStoreItems> TblStoreItems { get; set; }
    }
}
