using System;
using System.Collections.Generic;
using System.Text;

namespace Stadhawk.Laundry.Utility.IResponseUtility
{
    public interface IResponse
    {
        string Message { get; set; }
        bool Status { get; set; }
    }
}
