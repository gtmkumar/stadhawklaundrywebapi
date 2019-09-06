using Newtonsoft.Json;
using Stadhawk.Laundry.Utility.GoogleResponseViewModel;
using Stadhawk.Laundry.Utility.IHandler;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Stadhawk.Laundry.Utility.Handler
{
    public class DirectionHandler : IDirectionHandler
    {
        public async System.Threading.Tasks.Task<GoogleDirectionResponse> GetGoogleDirectionResponseAsync(string sLatitude, string sLongitude, string dLatitude, string dLongitude)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = "https://maps.googleapis.com/maps/api/directions/json?origin=" + sLatitude + "," + sLongitude + "&destination=" + dLatitude + "," + dLongitude + "&key=AIzaSyDm2xoE54QcDbJzWFt-C4YkMcyKg8idJ50";
                GoogleDirectionResponse dynObj = null;
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage Res = await client.GetAsync(url);
                    if (Res.IsSuccessStatusCode)
                    {
                        string GeoResponse = Res.Content.ReadAsStringAsync().Result;
                        dynObj = JsonConvert.DeserializeObject<GoogleDirectionResponse>(GeoResponse);
                        if (dynObj.Status == "OK")
                        {
                            return dynObj;
                        }
                    }
                }
                catch (Exception ex)
                {

                    return dynObj;
                }
                return dynObj;
            }
        }
    }
}
