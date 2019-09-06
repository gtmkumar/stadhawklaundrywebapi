using System;
using System.Collections.Generic;
using System.Text;

namespace StadhawkLaundry.ViewModel.ResponseModel
{
   public class BannerResponseViewModel
    {
        public int BannerId { get; set; }
        public string BannerName { get; set; }
        public string BannerImage { get; set; }
        public int BannerCategoryId { get; set; }
        public string BannerCategoryName { get; set; }
    }
}
