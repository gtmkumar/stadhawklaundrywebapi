using System;
using System.Collections.Generic;

namespace StadhawkLaundry.DataModel.Models
{
    public partial class TblOrderDetail
    {
        public Guid Id { get; set; }
        public Guid? OrderId { get; set; }
        public Guid? ItemId { get; set; }
        public int? NoOfItem { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
