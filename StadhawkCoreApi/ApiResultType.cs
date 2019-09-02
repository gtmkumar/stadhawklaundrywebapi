using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace StadhawkCoreApi
{
    [DataContract(Namespace = "")]
    public enum ApiResultType
    {
        [EnumMember(Value = "Success")]
        Success,

        [EnumMember(Value = "Error")]
        Error,

        [EnumMember(Value = "Error")]
        ExceptionDuringOpration
    }
}
