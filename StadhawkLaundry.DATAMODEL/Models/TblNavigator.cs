using System;
using System.Collections.Generic;

namespace StadhawkLaundry.DataModel.Models
{
    public partial class TblNavigator
    {
        public int NavigatorId { get; set; }
        public string DisplayText { get; set; }
        public int? ParantId { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public int? PrivilegeId { get; set; }
        public int DefaultOrder { get; set; }
        public byte NavLevel { get; set; }
        public string Icon { get; set; }
        public string Class { get; set; }
        public bool IsDefault { get; set; }
        public DateTime InsertDate { get; set; }
        public int? InsertBy { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public int? LastUpdateBy { get; set; }
        public bool? Ishide { get; set; }
        public byte? PortalType { get; set; }
    }
}
