using System;
using System.Collections.Generic;

namespace StadhawkLaundry.DataModel.Models
{
    public partial class TblUserAddress
    {
        public int UserAddressId { get; set; }
        public string UserId { get; set; }
        public int? AddressId { get; set; }
        public int? AddressTypeId { get; set; }
        public bool? IsDefaultDeliveryLocation { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? IsDelete { get; set; }

        public virtual TblAddressMaster Address { get; set; }
        public virtual TblAddressTypeMaster AddressType { get; set; }
        public virtual AspNetUsers User { get; set; }
    }
}
