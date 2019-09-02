using System;
using System.Collections.Generic;
using System.Text;

namespace Stadhawk.Laundry.Utility.IResponseUtility
{
    public interface IListResponse<TModel> : IResponse
    {
        IEnumerable<TModel> Data { get; set; }
    }
}
