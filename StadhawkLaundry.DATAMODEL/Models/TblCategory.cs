using System;
using System.Collections.Generic;

namespace StadhawkLaundry.DataModel.Models
{
    public partial class TblCategory
    {
        public TblCategory()
        {
            TblSubcategory = new HashSet<TblSubcategory>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public Guid? ServiceId { get; set; }

        public virtual TblService Service { get; set; }
        public virtual ICollection<TblSubcategory> TblSubcategory { get; set; }
    }
}
