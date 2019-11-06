using System;
using System.Collections.Generic;
using System.Text;

namespace Stadhawk.Laundry.Utility.Enums
{
    public class EnumType
    {
        public enum PaymentGateWayType
        {
            RAZORPAY=2,
            PAYTM=3
        }
        public enum OrderTypeEnum
        {
            UPCOMING = 1,
            HISTORY = 2
        }
    }    
}
