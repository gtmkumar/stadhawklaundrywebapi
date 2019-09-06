using System;
using System.Collections.Generic;

namespace StadhawkLaundry.DataModel.Models
{
    public partial class TblAccessRight
    {
        public int AccessId { get; set; }
        public string Name { get; set; }
        public int? OrderBy { get; set; }
    }
}
