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
        [Key]
        [Column(Order = 0)]
        public Guid Navigator_Id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(150)]
        public string Display_Text { get; set; }

        public Guid? Parant_Id { get; set; }

        [StringLength(500)]
        public string URL { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        public Guid? Privilege_Id { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Default_Order { get; set; }

        [Key]
        [Column(Order = 3)]
        public byte Nav_Level { get; set; }

        [StringLength(50)]
        public string icon { get; set; }

        public Guid? RoleId { get; set; }

        [Key]
        [Column(Order = 4)]
        public bool IsDefault { get; set; }

        [Key]
        [Column(Order = 5)]
        public DateTime InsertDate { get; set; }

        public Guid? InsertBy { get; set; }

        [StringLength(50)]
        public string Class { get; set; }

        public bool? ishide { get; set; }

        public byte? PortalType { get; set; }
    }
}
