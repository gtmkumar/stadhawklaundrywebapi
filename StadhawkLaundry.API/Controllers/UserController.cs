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
using Stadhawk.Laundry.Utility.IHandler;
using Stadhawk.Laundry.Utility.Model;
using Stadhawk.Laundry.Utility.ResponseUtility;
using StadhawkCoreApi.Logger;
using StadhawkLaundry.API.Common;
using StadhawkLaundry.API.Data;
using StadhawkLaundry.API.Models;
using StadhawkLaundry.BAL.Core;
using StadhawkLaundry.DataModel.Models;
using StadhawkLaundry.ViewModel;
using Utility;

namespace StadhawkLaundry.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISmsHandler<SmsResponseModel> _sms;
        //private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly AppSettings _appSettings;
        private readonly IUnitOfWork _unit;
        public UserController(IUnitOfWork unit, IOptions<AppSettings> appSettings, UserManager<ApplicationUser> userManager, ISmsHandler<SmsResponseModel> sms)
        {
            _unit = unit;
            _appSettings = appSettings.Value;
            _userManager = userManager;
            _sms = sms;
            //_emailSender = emailSender;
        }
        // POST: api/User
        [HttpPost("registration")]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromForm] UsersViewModel users)
        {
            var response = new Response();

            var userId = this.User.FindFirstValue(ClaimTypes.Name);

            if (ModelState.IsValid)
            {
                bool isEdit = true;
                Guid userGuid = Guid.Empty;
                isEdit = !string.IsNullOrEmpty(users.Id) ? Guid.TryParse(users.Id, out userGuid) : false;
                try
                {
                    if ((await _unit.IUser.Exists(t => t.Email.Equals(users.EmailId))).UserObject)
                    {
                        response.Message = "Email already in use.";
                        response.Status = false;
                        response.ErrorTypeCode = (int)ErrorMessage.Email;
                    }
                    else
                    {
                        var tempUsers = AutoMapper.Mapper.Map<ApplicationUser>(users);
                        AspNetUserRoles userInRole = new AspNetUserRoles();
                        if (!isEdit)
                        {
                            tempUsers.CreatedDate = DateTime.Now;
                            tempUsers.CreatedBy = tempUsers.Id;
                            tempUsers.Status = true;
                            var result = await _userManager.CreateAsync(tempUsers);
                            await _userManager.AddToRoleAsync(tempUsers, "USER");
                            if (result.Succeeded)
                            {
                                string strPhone = ("91" + users.Contactno);
                                response.Message = "user registered.";
                                response.Status = true;
                                var otpresposedata = await _sms.SendOtpAsync(SmsVendorUrl: _appSettings.SmsVendorUrl, strHasKey: _appSettings.SmsHasKey, mobile: strPhone);
                                if (otpresposedata.type == "success")
                                {
                                    return response.ToHttpResponse();
                                }
                            }
                        }
                        else
                        {
                            tempUsers = (await _userManager.FindByIdAsync(Convert.ToString(userGuid)));
                            tempUsers.PhoneNumber = users.Contactno;
                            tempUsers.UserName = users.EmailId;
                            tempUsers.ModifiedDate = DateTime.Now;
                            tempUsers.ModifiedBy = userId;
                            var result = await _userManager.UpdateAsync(tempUsers);
                        }
                        response.Message = "user Added.";
                        response.Status = true;
                        return response.ToHttpResponse();
                    }
                }
                catch (Exception ex)
                {
                    response.Status = false;
                    response.Message = "There was an internal error, please contact technical support.";
                    ErrorTrace.Logger(LogArea.ApplicationTier, ex);
                }
            }
            return response.ToHttpResponse();
        }

        [HttpDelete("{id}")]
        public async Task<HttpResponseMessage> Delete(Guid id)
        {
            await _unit.IUser.Remove(id);
            var result = await _unit.Complete();
            if (result.ResultType == StadhawkCoreApi.ApiResultType.Success)
            {
                var response = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK
                };
                return response;
            }
            else
            {
                var response = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest
                };
                return response;
            }
        }

    }
}