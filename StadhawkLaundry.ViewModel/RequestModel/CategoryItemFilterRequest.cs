namespace StadhawkLaundry.ViewModel.RequestModel
{
    public class CategoryItemFilterRequest
    {
        public int ServiceId { get; set; }
        public int CategoryId { get; set; }
        public int StoreId { get; set; }
        public int UserId { get; set; }
    }
}
