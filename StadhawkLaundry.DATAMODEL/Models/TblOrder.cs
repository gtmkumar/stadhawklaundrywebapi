using System;
using System.Collections.Generic;

namespace StadhawkLaundry.DataModel.Models
{
    public partial class TblOrder
    {
        public Guid Id { get; set; }
        public Guid? StoreId { get; set; }
        public Guid? UserId { get; set; }
        public int? OrderStatusId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public decimal? TotalAmount { get; set; }
        public string OrderShipName { get; set; }
        public string OrdrShipAddress { get; set; }
        public string OrdrShipAddress2 { get; set; }
        public string OrderCity { get; set; }
        public string OrderState { get; set; }
        public string OrderZip { get; set; }
        public string OrderPhone { get; set; }
        public decimal? OrderSiping { get; set; }
        public decimal? OrdrGst { get; set; }
        public string OrderEmail { get; set; }
        public DateTime? OrderDate { get; set; }
        public string OrderTrakingNo { get; set; }
        public int? OrderStatus { get; set; }
    }
}
