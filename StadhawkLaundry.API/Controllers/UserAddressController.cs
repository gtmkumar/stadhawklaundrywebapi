using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Stadhawk.Laundry.Utility.GoogleResponseViewModel;
using Stadhawk.Laundry.Utility.IHandler;
using Stadhawk.Laundry.Utility.ResponseUtility;
using StadhawkCoreApi;
using StadhawkCoreApi.Logger;
using StadhawkLaundry.API.Common;
using StadhawkLaundry.API.Data;
using StadhawkLaundry.API.Models;
using StadhawkLaundry.BAL.Core;
using StadhawkLaundry.DataModel.Models;
using StadhawkLaundry.ViewModel;
using StadhawkLaundry.ViewModel.RequestModel;
using StadhawkLaundry.ViewModel.ResponseModel;
using Utility;

namespace StadhawkLaundry.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserAddressController : ControllerBase
    {
        private readonly IDirectionHandler _direction;
        private readonly IUnitOfWork _unit;
        public UserAddressController(IUnitOfWork unit, IDirectionHandler direction)
        {
            _unit = unit;
            _direction = direction;
        }

        [HttpPost("addupdateaddress")]
        public async Task<IActionResult> Post([FromForm] UserAddressRequestViewModel value)
        {
            int? userId = 0;
            var struserId = this.User.FindFirstValue(ClaimTypes.Name);
            if (!string.IsNullOrWhiteSpace(struserId))
                userId = Convert.ToInt32(struserId);


            var response = new ListResponse<UserAddressResponseViewModel>();
            try
            {
                var result = new ApiResult<bool>();
                if (value.AddressId != null && value.AddressId > 0)
                {
                    result = await _unit.IUserAddress.UpdateUserAddress(userAddress: value, userId: userId.Value);
                    response.Data = (await _unit.IUserAddress.UserAddress(userId: userId.Value, addressId: value.AddressId)).UserObject;
                    response.Message = "Success";
                    response.Status = true;
                    return response.ToHttpResponse();
                }
                else
                {
                    result = await _unit.IUserAddress.SaveUserAddress(userAddress: value, userId: userId.Value);
                    if (result.HasSuccess)
                    {
                        response.Data = null;
                        response.Message = "Success";
                        response.Status = true;
                    }
                    else
                    {
                        response.Data = null;
                        response.Message = "Error";
                        response.Status = false;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "There was an internal error, please contact to technical support.";
                ErrorTrace.Logger(LogArea.ApplicationTier, ex);
            }
            return response.ToHttpResponse();
        }

        [HttpGet("getaddresses")]
        public async Task<IActionResult> Get()
        {
            int userId = 0;
            var userStrId = this.User.FindFirstValue(ClaimTypes.Name);
            if (!string.IsNullOrWhiteSpace(userStrId))
                userId = Convert.ToInt32(userStrId);

            var ownResponse = new ListResponse<UserAddressResponseViewModel>();
            ownResponse.Data = (await _unit.IUserAddress.UserAddress(userId: userId)).UserObject;
            ownResponse.Message = "Success";
            ownResponse.Status = true;
            return ownResponse.ToHttpResponse();
        }

        [AllowAnonymous]
        [HttpGet("getgeoaddress")]
        public async Task<IActionResult> GetGeoAddress([FromQuery]string latitude, string longitude)
        {
            var response = new SingleResponse<GeoAddressResponseViewModel>();
            GeoAddressResponseViewModel resResuslt = new GeoAddressResponseViewModel();
            string url = "https://maps.googleapis.com/maps/api/geocode/json?latlng=" + latitude + "," + longitude + "&key=AIzaSyDm2xoE54QcDbJzWFt-C4YkMcyKg8idJ50";
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage Res = await client.GetAsync(url);
                    if (Res.IsSuccessStatusCode)
                    {
                        string GeoResponse = Res.Content.ReadAsStringAsync().Result;
                        GoogleGeoResponse dynObj = JsonConvert.DeserializeObject<GoogleGeoResponse>(GeoResponse);

                        response.Message = "Success";
                        response.Status = true;
                        if (dynObj.results[0] != null && !string.IsNullOrEmpty(dynObj.results[0].formatted_address))
                        {
                            resResuslt.formatted_address = dynObj.results[0].formatted_address;
                        }
                        response.Data = resResuslt;
                        return response.ToHttpResponse();
                    }
                    else
                    {
                        response.Status = false;
                        response.Message = "There was an internal error, please contact technical support.";
                        return response.ToHttpResponse();
                    }
                }
                catch (Exception ex)
                {
                    response.Status = false;
                    response.Message = "There was an internal error, please contact technical support.";
                    return response.ToHttpResponse();
                }

            }
        }

        [HttpGet("getdirection")]
        public async Task<IActionResult> GetDirection([FromQuery]string sLatitude, string sLongitude, string dLatitude, string dLongitude)
        {
            var response = new SingleResponse<GoogleDirectionResponse>();
            response.Message = "Success";
            response.Status = true;
            response.Data = await _direction.GetGoogleDirectionResponseAsync(sLatitude, sLongitude, dLatitude, dLongitude);
            return response.ToHttpResponse();
        }

        [HttpPost("deleteaddress")]
        public async Task<IActionResult> DeleteAddress([FromForm] int addressId)
        {
            int? userId = 0;
            string mobileNo = string.Empty;
            var struserId = this.User.FindFirstValue(ClaimTypes.Name);
            if (!string.IsNullOrWhiteSpace(struserId))
                userId = Convert.ToInt32(userId);

            var response = new Response();
            var result = await _unit.IUserAddress.DeleteAddress(userId, addressId);
            if (result.HasSuccess)
            {
                response.Message = "Success";
                response.Status = true;
            }
            return response.ToHttpResponse();
        }
    }
}