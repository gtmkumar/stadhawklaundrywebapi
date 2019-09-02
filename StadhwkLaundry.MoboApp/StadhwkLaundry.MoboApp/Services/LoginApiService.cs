using Newtonsoft.Json;
using StadhwkLaundry.MoboApp.Models;
using StadhwkLaundry.MoboApp.Views;
using StadhwkLaundry.MoboApp.Views.Account;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace StadhwkLaundry.MoboApp.Services
{
    public class LoginApiService
    {
        public async Task<LoginResponseModel> userLoginAsync(string email, string password)
        {
            string str = string.Empty;
            var client = new HttpClient();
            LoginModel model = new LoginModel
            {
                UserName = email,
                Password = password
            };

            var json = JsonConvert.SerializeObject(model);
            HttpContent content = new StringContent(json);
            content.Headers.ContentType.MediaType = "application/json";
            try
            {
                var response = await client.PostAsync("https://stadhawklaundry-api.conveyor.cloud/api/authenticate/login", content);
                if (response.IsSuccessStatusCode)
                {
                    App.Current.MainPage = new NavigationPage(new MainPage());
                }
                //else
                //{
                //    App.Current.MainPage = new NavigationPage(new LoginPage());
                //}
            }
            catch (Exception ex)
            {
                return null;
                throw;
            }
            return null;
        }
    }
}
