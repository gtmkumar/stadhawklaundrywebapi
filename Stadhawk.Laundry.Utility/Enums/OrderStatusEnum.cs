using System;
using System.Collections.Generic;
using System.Text;

namespace Stadhawk.Laundry.Utility.Enums
{
    public class EnumType
    {
        public enum PaymentGateWayType
        {
            RAZORPAY,
            PAYTM
        }
        public enum OrderTypeEnum
        {
            UPCOMING = 1,
            HISTORY = 2
        }
    }    
}
