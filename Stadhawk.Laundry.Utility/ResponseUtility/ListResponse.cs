using Stadhawk.Laundry.Utility.IResponseUtility;
using System.Collections.Generic;

namespace Stadhawk.Laundry.Utility.ResponseUtility
{
    public class ListResponse<TModel> : IListResponse<TModel>
    {
        public string Message { get; set; }
        public bool Status { get; set; }
        public int ErrorTypeCode { get; set; }
        public IEnumerable<TModel> Data { get; set; }
    }
}
