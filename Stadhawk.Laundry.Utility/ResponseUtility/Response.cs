using Stadhawk.Laundry.Utility.IResponseUtility;

namespace Stadhawk.Laundry.Utility.ResponseUtility
{
    public class Response : IResponse
    {
        public string Message { get; set; }
        public bool Status { get; set; }
        public int ErrorTypeCode { get; set; }
    }
}
