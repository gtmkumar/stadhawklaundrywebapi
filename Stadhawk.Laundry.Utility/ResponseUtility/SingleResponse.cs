using Stadhawk.Laundry.Utility.IResponseUtility;

namespace Stadhawk.Laundry.Utility.ResponseUtility
{
    public class SingleResponse<TModel> : ISingleResponse<TModel>
    {
        public string Message { get; set; }
        public bool Status { get; set; }
        public int ErrorTypeCode { get; set; }
        public TModel Data { get; set; }
    }
}
