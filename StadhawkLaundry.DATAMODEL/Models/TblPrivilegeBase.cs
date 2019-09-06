using System;
using System.Collections.Generic;

namespace StadhawkLaundry.DataModel.Models
{
    public partial class TblPrivilegeBase
    {
        public int PrivilegeId { get; set; }
        public string PrivilegeName { get; set; }
        public int? EntityId { get; set; }
        public bool? CanBeDeep { get; set; }
        public int? AccessRight { get; set; }
        public bool? CanBeBasic { get; set; }
        public bool? CanBeGlobal { get; set; }
        public bool? IsDisabledWhenIntegrated { get; set; }
        public bool? CanBeLocal { get; set; }
        public DateTime? InsertDate { get; set; }
        public int? InsertBy { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public int? LastUpdateBy { get; set; }
    }
}
