using System;
using System.Collections.Generic;

namespace StadhawkLaundry.DataModel.Models
{
    public partial class TblAddressTypeMaster
    {
        public TblAddressTypeMaster()
        {
            TblUserAddress = new HashSet<TblUserAddress>();
        }

        public int AddressTypeId { get; set; }
        public string AddressTypeDescription { get; set; }

        public virtual ICollection<TblUserAddress> TblUserAddress { get; set; }
    }
}
