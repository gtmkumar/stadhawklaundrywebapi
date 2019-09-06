using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityAuth.Core
{
    public enum IdentityStatus
    {
        Successfull,
        UserNotFound,
        FatalException,
        UserAlreadyExists,
    }
}
