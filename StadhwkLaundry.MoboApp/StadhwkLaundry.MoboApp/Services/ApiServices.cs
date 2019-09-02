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
    public class ApiServices
    {
        public async Task RegistorUserAsync(string email, string password, string contactNo)
        {
            string str = string.Empty;
            var client = new HttpClient();
            RegistorBindingModel model = new RegistorBindingModel
            {
                Id = "",
                EmailId = email,
                Password = password,
                Contactno = contactNo,
                UserName = email,
                FullName = "Goutam Kumar"
            };

            var json = JsonConvert.SerializeObject(model);
            HttpContent content = new StringContent(json,Encoding.UTF8,"application/json");
            try
            {
                var result = await client.PostAsync("https://stadhawklaundry-api.conveyor.cloud/api/User/registration", content);

                var response = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.Conflict
                };
                if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    App.Current.MainPage = new NavigationPage(new RegistorPage());

                }
                else
                {
                    if (result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        App.Current.MainPage = new NavigationPage(new MainPage());
                    }
                    else
                    {
                        str = "Something Went Wrong";
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}
