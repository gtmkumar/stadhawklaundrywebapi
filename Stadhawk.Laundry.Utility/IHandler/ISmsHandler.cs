using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stadhawk.Laundry.Utility.IHandler
{
    public interface ISmsHandler<TEntity> where TEntity : class
    {
        Task<TEntity> SendOtpAsync(string SmsVendorUrl, string strHasKey, string mobile = null, string email = null);
        Task<TEntity> ResendOtpAsync(string mobile = null, string email = null);
        Task<TEntity> VerifyOtpAsync(string mobile = null, string OTP = null);
    }
}
