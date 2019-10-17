using System;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stadhawk.Laundry.Utility.IHandler;
using Stadhawk.Laundry.Utility.Model;
using Stadhawk.Laundry.Utility.ResponseUtility;
using StadhawkCoreApi.Logger;
using StadhawkLaundry.API.Data;
using StadhawkLaundry.API.Models;
using StadhawkLaundry.BAL.Core;
using StadhawkLaundry.ViewModel;
using StadhawkLaundry.ViewModel.RequestModel;
using Utility;

namespace StadhawkLaundry.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISmsHandler<SmsResponseModel> _sms;
        private readonly AppSettings _appSettings;
        private readonly IUnitOfWork _unit;
        public UserController(IUnitOfWork unit, IOptions<AppSettings> appSettings, UserManager<ApplicationUser> userManager, ISmsHandler<SmsResponseModel> sms)
        {
            _unit = unit;
            _appSettings = appSettings.Value;
            _userManager = userManager;
            _sms = sms;
        }
        [HttpPost("registration")]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromForm] UsersViewModel users)
        {
            var response = new Response();
            int userId = 0;
            string strUserId = this.User.FindFirstValue(ClaimTypes.Name);

            if (ModelState.IsValid)
            {
                bool isEdit = true;
                isEdit = userId > 0 ? true : false;
                try
                {
                    if (!(await _unit.IUser.Exists(t => t.PhoneNumber.Equals(users.ContactNo) && t.UserType == 1)).UserObject)
                    {
                        ModelState.AddModelError("PPhone", "Your Phone no. is not register with us");
                        response.Message = "Your Phone no. is not register with us";
                        response.Status = false;
                        response.ErrorTypeCode = (int)ErrorMessage.PhoneOrEmailNotRegistor;
                    }
                    if ((await _unit.IUser.Exists(t => t.Email.Equals(users.EmailId) && t.UserType == 1)).UserObject)
                    {
                        response.Message = "Email already in use.";
                        response.Status = false;
                        response.ErrorTypeCode = (int)ErrorMessage.Email;
                    }
                    else
                    {
                        var tempUsers = AutoMapper.Mapper.Map<ApplicationUser>(users);
                        ApplicationRole userInRole = new ApplicationRole();
                        if (!isEdit)
                        {
                            tempUsers.CreatedDate = DateTime.Now;
                            tempUsers.CreatedBy = tempUsers.Id;
                            tempUsers.Status = true;
                            tempUsers.UserName = users.EmailId;
                            tempUsers.UserType = 1;
                            var result = await _userManager.CreateAsync(tempUsers);
                            await _userManager.AddToRoleAsync(tempUsers, "USER");
                            if (result.Succeeded)
                            {
                                string strPhone = ("91" + users.ContactNo);
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
                            tempUsers = (await _userManager.FindByIdAsync(Convert.ToString(userId)));
                            tempUsers.PhoneNumber = users.ContactNo;
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
        public async Task<HttpResponseMessage> Delete(int id)
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
        [HttpPost("userregistration")]
        [AllowAnonymous]
        public async Task<IActionResult> PostUser([FromForm] WebUsersRegistrationViewModel users)
        {
            var response = new Response();
            int userId = 0;
            string strUserId = this.User.FindFirstValue(ClaimTypes.Name);

            if (ModelState.IsValid)
            {
                bool isEdit = true;
                isEdit = userId > 0 ? true : false;
                try
                {
                    if ((await _unit.IUser.Exists(t => t.Email.Equals(users.EmailId) && t.UserType == 1)).UserObject)
                    {
                        ModelState.AddModelError("PPhone", "Your Phone no. is not register with us");
                        response.Message = "Your Phone no. is not register with us";
                        response.Status = false;
                        response.ErrorTypeCode = (int)ErrorMessage.PhoneOrEmailNotRegistor;
                    }
                    if ((await _unit.IUser.Exists(t => t.Email.Equals(users.EmailId) && t.UserType == 1)).UserObject)
                    {
                        response.Message = "Email already in use.";
                        response.Status = false;
                        response.ErrorTypeCode = (int)ErrorMessage.Email;
                    }
                    else
                    {
                        var tempUsers = AutoMapper.Mapper.Map<ApplicationUser>(users);
                        ApplicationRole userInRole = new ApplicationRole();
                        if (!isEdit)
                        {
                            tempUsers.PasswordHash = users.Password;
                            tempUsers.CreatedDate = DateTime.Now;
                            tempUsers.CreatedBy = tempUsers.Id;
                            tempUsers.Status = true;
                            tempUsers.UserName = users.EmailId;
                            tempUsers.UserType = 1;
                            var result = await _userManager.CreateAsync(tempUsers);
                            await _userManager.AddToRoleAsync(tempUsers, "USER");
                            if (result.Succeeded)
                            {
                                string strPhone = ("91" + users.ContactNo);
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
                            tempUsers = (await _userManager.FindByIdAsync(Convert.ToString(userId)));
                            tempUsers.PhoneNumber = users.ContactNo;
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
    }
}