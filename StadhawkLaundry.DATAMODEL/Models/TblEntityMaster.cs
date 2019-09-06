using System;
using System.Collections.Generic;

namespace StadhawkLaundry.DataModel.Models
{
    public partial class TblEntityMaster
    {
        public int EntityId { get; set; }
        public string EntityName { get; set; }
        public string EntityDescription { get; set; }
        public DateTime InsertDate { get; set; }
        public int? InsertBy { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public int? LastUpdateBy { get; set; }
    }
}
