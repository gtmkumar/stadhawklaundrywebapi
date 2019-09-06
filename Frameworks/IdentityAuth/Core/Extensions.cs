using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityAuth.Core
{
    public static class Extensions
    {
        public static T ParseEnum<T>(string value)
        {
            try
            {
                return (T)Enum.Parse(typeof(T), value, true);
            }
            catch
            {
                return default(T);
            }
        }
    }
}
