using System;
using System.Collections.Generic;

namespace StadhawkLaundry.DataModel.Models
{
    public partial class TblStore
    {
        public TblStore()
        {
            TblBanner = new HashSet<TblBanner>();
            TblStoreEmployees = new HashSet<TblStoreEmployees>();
            TblStoreItems = new HashSet<TblStoreItems>();
            TblStorePackagesAndCategoryMapping = new HashSet<TblStorePackagesAndCategoryMapping>();
            TblStorePckages = new HashSet<TblStorePckages>();
        }

        public long Id { get; set; }
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
        public string Image { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public int? DistrictId { get; set; }
        public string CityName { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNo { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public string Gstno { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CretedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual TblCityMaster District { get; set; }
        public virtual ICollection<TblBanner> TblBanner { get; set; }
        public virtual ICollection<TblStoreEmployees> TblStoreEmployees { get; set; }
        public virtual ICollection<TblStoreItems> TblStoreItems { get; set; }
        public virtual ICollection<TblStorePackagesAndCategoryMapping> TblStorePackagesAndCategoryMapping { get; set; }
        public virtual ICollection<TblStorePckages> TblStorePckages { get; set; }
    }
}
