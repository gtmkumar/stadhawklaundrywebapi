using System;
using System.Collections.Generic;

namespace StadhawkLaundry.DataModel.Models
{
    public partial class TblSubServiceMaster
    {
        public TblSubServiceMaster()
        {
            TblStoreItems = new HashSet<TblStoreItems>();
        }

        public int Id { get; set; }
        public int? ServiceId { get; set; }
        public string SubServiceName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual TblServiceMaster Service { get; set; }
        public virtual ICollection<TblStoreItems> TblStoreItems { get; set; }
    }
}
