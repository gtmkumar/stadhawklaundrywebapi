using System;
using System.Collections.Generic;

namespace StadhawkLaundry.DataModel.Models
{
    public partial class TblStateMaster
    {
        public TblStateMaster()
        {
            TblCityMaster = new HashSet<TblCityMaster>();
        }

        public int Id { get; set; }
        public string Country { get; set; }
        public string StateName { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual ICollection<TblCityMaster> TblCityMaster { get; set; }
    }
}
