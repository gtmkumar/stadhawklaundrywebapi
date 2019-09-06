using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace StadhawkLaundry.DataModel.Models
{
    [Table("NavigatorView")]
    public partial class NavigatorView
    {

        public int Navigator_Id { get; set; }

        public string Display_Text { get; set; }

        public int? Parant_Id { get; set; }

        public string URL { get; set; }

        public string Description { get; set; }

        public int? Privilege_Id { get; set; }

        public int Default_Order { get; set; }

        public byte Nav_Level { get; set; }

        public string icon { get; set; }

        public int RoleId { get; set; }
        public bool IsDefault { get; set; }
        public DateTime InsertDate { get; set; }
        public int? InsertBy { get; set; }
        public string Class { get; set; }
        public bool? ishide { get; set; }
        public byte? PortalType { get; set; }
    }
}