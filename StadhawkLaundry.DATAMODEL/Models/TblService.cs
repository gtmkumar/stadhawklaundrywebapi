using System;
using System.Collections.Generic;

namespace StadhawkLaundry.DataModel.Models
{
    public partial class TblService
    {
        public TblService()
        {
            TblCategory = new HashSet<TblCategory>();
            TblItem = new HashSet<TblItem>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public string ServiceImage { get; set; }

        public virtual ICollection<TblCategory> TblCategory { get; set; }
        public virtual ICollection<TblItem> TblItem { get; set; }
    }
}
