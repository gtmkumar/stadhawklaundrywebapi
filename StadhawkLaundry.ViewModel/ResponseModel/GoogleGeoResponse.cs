using System;
using System.Collections.Generic;
using System.Text;

namespace StadhawkLaundry.ViewModel.ResponseModel
{
    public partial class GoogleGeoResponse
    {
        public List<Result> results { get; set; }
        public string Status { get; set; }
    }
    public partial class Result
    {
        public string formatted_address { get; set; }
    }
}
