using System;
using System.Collections.Generic;

namespace StadhawkLaundry.DataModel.Models
{
    public partial class TblStoreEmployees
    {
        public long Id { get; set; }
        public long? StoreId { get; set; }
        public int? EmployeeId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual TblStore Store { get; set; }
    }
}
