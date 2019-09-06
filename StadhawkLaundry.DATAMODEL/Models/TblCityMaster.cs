using System;
using System.Collections.Generic;

namespace StadhawkLaundry.DataModel.Models
{
    public partial class TblCityMaster
    {
        public TblCityMaster()
        {
            TblStore = new HashSet<TblStore>();
        }

        public int Id { get; set; }
        public int? StateId { get; set; }
        public string DistrictName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual TblStateMaster State { get; set; }
        public virtual ICollection<TblStore> TblStore { get; set; }
    }
}
