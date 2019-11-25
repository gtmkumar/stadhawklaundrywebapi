using Newtonsoft.Json;
using Stadhawk.Laundry.Utility.IHandler;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Stadhawk.Laundry.Utility.Handler
{
    public class SmsHandler<TEntity> : ISmsHandler<TEntity> where TEntity : class
    {
        public async Task<TEntity> SendOtpAsync(string SmsVendorUrl, string strHasKey, string mobile = null, string email = null)
        {
            Random random = new Random();
            string OTP = random.Next(1001, 9999).ToString();
            #region For iOS Verification 

            if (mobile.Equals("917042275858"))
                OTP = "0305";

            #endregion
            string authkey = "273001AoCttKR7nJtU5cb8520c";
            string strMsgKey = WebUtility.UrlEncode(strHasKey);
            string message = WebUtility.UrlEncode("<#> STADHAWK OTP IS: " + OTP + " " + strMsgKey + "");
            string sender = "STDHWK";
            string Baseurlre = "" + SmsVendorUrl + "" + authkey + "&message=" + message + "&sender=" + sender + "&mobile=" + mobile + "&otp=" + OTP + "";
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurlre);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.PostAsync(Baseurlre, content: null);
                if (Res.IsSuccessStatusCode)
                {
                    string GeoResponse = Res.Content.ReadAsStringAsync().Result;
                    TEntity dynObj = JsonConvert.DeserializeObject<TEntity>(GeoResponse);
                    if (dynObj != null)
                    {
                        return dynObj;
                    }
                }
            }
            return default(TEntity);
        }
        public async Task<TEntity> ResendOtpAsync(string mobile = null, string email = null)
        {
            string authkey = "273001AoCttKR7nJtU5cb8520c";
            string Baseurlre = "http://control.msg91.com/api/retryotp.php?authkey=" + authkey + "&mobile=" + mobile + "";
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurlre);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage Res = await client.PostAsync(Baseurlre, content: null);
                    if (Res.IsSuccessStatusCode)
                    {
                        string GeoResponse = Res.Content.ReadAsStringAsync().Result;
                        TEntity dynObj = JsonConvert.DeserializeObject<TEntity>(GeoResponse);
                        if (dynObj != null)
                        {
                            return dynObj;
                        }
                    }
                    else
                    {
                        return default(TEntity);
                    }

                }
                catch (Exception)
                {
                    return default(TEntity);
                }
                return default(TEntity);
            }
        }
        public async Task<TEntity> VerifyOtpAsync(string mobile = null, string OTP = null)
        {
            string authkey = "273001AoCttKR7nJtU5cb8520c";
            string Baseurlre = "https://control.msg91.com/api/verifyRequestOTP.php?authkey=" + authkey + "&mobile=" + mobile + "&otp=" + OTP + "";
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurlre);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.PostAsync(Baseurlre, content: null);
                if (Res.IsSuccessStatusCode)
                {
                    string GeoResponse = Res.Content.ReadAsStringAsync().Result;
                    TEntity dynObj = JsonConvert.DeserializeObject<TEntity>(GeoResponse);
                    if (dynObj != null)
                    {
                        return dynObj;
                    }
                }
                return default(TEntity);
            }


        }
    }
}
