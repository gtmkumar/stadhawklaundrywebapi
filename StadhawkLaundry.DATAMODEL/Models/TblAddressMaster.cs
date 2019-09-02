using System;
using System.Collections.Generic;

namespace StadhawkLaundry.DataModel.Models
{
    public partial class TblAddressMaster
    {
        public TblAddressMaster()
        {
            TblUserAddress = new HashSet<TblUserAddress>();
        }

        public int AddressId { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string LandMark { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? IsDelete { get; set; }

        public virtual ICollection<TblUserAddress> TblUserAddress { get; set; }
    }
}
