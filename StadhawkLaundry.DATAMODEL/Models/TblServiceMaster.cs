﻿using System;
using System.Collections.Generic;

namespace StadhawkLaundry.DataModel.Models
{
    public partial class TblServiceMaster
    {
        public TblServiceMaster()
        {
            TblStoreItems = new HashSet<TblStoreItems>();
            TblSubServiceMaster = new HashSet<TblSubServiceMaster>();
        }

        public int Id { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public int? Label { get; set; }
        public int? DisplayOrder { get; set; }
        public string ServiceImage { get; set; }

        public virtual ICollection<TblStoreItems> TblStoreItems { get; set; }
        public virtual ICollection<TblSubServiceMaster> TblSubServiceMaster { get; set; }
    }
}
