using System;
using System.Collections.Generic;
using System.Text;

namespace Stadhawk.Laundry.Utility.IResponseUtility
{
    public interface ISingleResponse<TModel> : IResponse
    {
        TModel Data { get; set; }
    }
}
