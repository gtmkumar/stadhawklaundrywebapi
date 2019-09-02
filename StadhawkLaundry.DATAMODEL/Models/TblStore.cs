using System;
using System.Collections.Generic;

namespace StadhawkLaundry.DataModel.Models
{
    public partial class TblStore
    {
        public TblStore()
        {
            TblItemPrinceMappingByStore = new HashSet<TblItemPrinceMappingByStore>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LocationName { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public int? CityId { get; set; }
        public string CityName { get; set; }
        public string PinCode { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public string Image { get; set; }

        public virtual ICollection<TblItemPrinceMappingByStore> TblItemPrinceMappingByStore { get; set; }
    }
}
