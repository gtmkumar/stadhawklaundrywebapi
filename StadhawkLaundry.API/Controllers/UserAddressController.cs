using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
    [Route("api/[controller]")]
    [ApiController]
    public class UserAddressController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        //private readonly ILogger _logger;
        private readonly AppSettings _appSettings;
        private readonly IUnitOfWork _unit;
        public UserAddressController(IUnitOfWork unit)
        {
            _unit = unit;
        }

        [HttpPost("addupdateaddress")]
        public async Task<IActionResult> Post([FromForm] UserAddressRequestViewModel value)
        {
            string customerId = string.Empty;
            var userId = this.User.FindFirstValue(ClaimTypes.Name);
            //var userId = "b5a800a6-d0db-423b-9320-42aba4b9701c";
            if (!string.IsNullOrWhiteSpace(userId))
                customerId = Convert.ToString(userId);

            var response = new ListResponse<UserAddressResponseViewModel>();
            try
            {
                var result = new ApiResult<bool>();
                if (value.AddressId != null && value.AddressId > 0)
                {
                    result = await _unit.IUserAddress.UpdateUserAddress(userAddress: value, userId: customerId);
                    response.Data = (await _unit.IUserAddress.UserAddress(UserId: customerId, addressId: value.AddressId)).UserObject;
                    response.Message = "Success";
                    response.Status = true;
                    return response.ToHttpResponse();
                }
                else
                {
                    result = await _unit.IUserAddress.SaveUserAddress(userAddress: value, userId: customerId);
                }
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
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "There was an internal error, please contact to technical support.";
                ErrorTrace.Logger(LogArea.ApplicationTier, ex);
            }
            return response.ToHttpResponse();
        }
        [HttpGet("getAddresses")]
        public async Task<IActionResult> Get()
        {
            string customerId = string.Empty;
            var userId = this.User.FindFirstValue(ClaimTypes.Name);
            // var userId = "b5a800a6-d0db-423b-9320-42aba4b9701c";
            if (!string.IsNullOrWhiteSpace(userId))
                customerId = Convert.ToString(userId);

            var ownResponse = new ListResponse<UserAddressResponseViewModel>();
            ownResponse.Data = (await _unit.IUserAddress.UserAddress(UserId: customerId)).UserObject;
            ownResponse.Message = "Success";
            ownResponse.Status = true;
            return ownResponse.ToHttpResponse();
        }
    }
}