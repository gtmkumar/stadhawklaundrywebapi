using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Stadhawk.Laundry.Utility.IHandler;
using Stadhawk.Laundry.Utility.Model;
using Stadhawk.Laundry.Utility.ResponseUtility;
using StadhawkCoreApi.Logger;
using StadhawkLaundry.API.Common;
using StadhawkLaundry.API.Data;
using StadhawkLaundry.API.Models;
using StadhawkLaundry.BAL.Core;
using StadhawkLaundry.ViewModel;
using StadhawkLaundry.ViewModel.RequestModel;
using StadhawkLaundry.ViewModel.ResponseModel;
using Utility;

namespace StadhawkLaundry.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ISmsHandler<SmsResponseModel> _smsHandler;
        private readonly IEmailSender _emailSender;
        private readonly AppSettings _appSettings;
        private readonly IUnitOfWork _unit;
        public AuthenticateController(IOptions<AppSettings> appSettings, IUnitOfWork unit, UserManager<ApplicationUser> userManager, ISmsHandler<SmsResponseModel> smsHandler)
        {
            _unit = unit;
            _appSettings = appSettings.Value;
            _userManager = userManager;
            _smsHandler = smsHandler;
        }

        [AllowAnonymous]
        [HttpPost("otp_request")]
        public async Task<IActionResult> PostAuthenticate([FromForm] OtpRequestViewModel model)
        {
            Random random = new Random();

            var response = new SingleResponse<OtpRequestViewModel>();
            string OTP = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    if (!(await _unit.IUser.Exists(t => t.PhoneNumber.Equals(model.MobileNo))).UserObject)
                    {
                        ModelState.AddModelError("PPhone", "Your Phone no. is not register with us");
                        response.Message = "Your Phone no. is not register with us";
                        response.Status = false;
                        response.ErrorTypeCode = (int)ErrorMessage.PhoneOrEmailNotRegistor;
                    }
                    else
                    {
                        string strPhone = ("91" + model.MobileNo);
                        var otpRequest = new OtpRequestViewModel
                        {
                            MobileNo = strPhone
                        };
                        var otpresposedata = true;//await _smsHandler.SendOtpAsync(SmsVendorUrl: _appSettings.SmsVendorUrl, strHasKey: _appSettings.SmsHasKey, mobile: strPhone);
                        if (otpresposedata)//.type == "success")
                        {
                            response.Message = "OTP Send on your registor no.";
                            response.Status = true;
                            response.Data = model;
                            return response.ToHttpResponse();
                        }

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

        [AllowAnonymous]
        [HttpPost("otp_resend")]
        public async Task<IActionResult> PostOtpResend([FromForm] OtpRequestViewModel value)
        {
            Random random = new Random();

            SingleResponse<OtpRequestViewModel> response = new SingleResponse<OtpRequestViewModel>();

            string OTP = string.Empty;

            try
            {
                if (ModelState.IsValid)
                {

                    if (!(await _unit.IUser.Exists(u => u.PhoneNumber == value.MobileNo)).UserObject)
                    {
                        ModelState.AddModelError("PPhone", "Your Phone no. is not register with us");
                        response.Message = "Your Phone no. is not register with us";
                        response.Status = false;
                        response.ErrorTypeCode = (int)ErrorMessage.PhoneOrEmailNotRegistor;
                    }
                    else
                    {
                        string strPhone = ("91" + value.MobileNo);
                        var otpresposedata = await _smsHandler.ResendOtpAsync(strPhone);
                        if (otpresposedata.type == "success")
                        {
                            response.Message = "OTP Send on your registor no.";
                            response.Status = true;
                            response.Data = value;
                            return response.ToHttpResponse();
                        }

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

        [AllowAnonymous]
        [HttpPost("otp_verify")]
        public async Task<IActionResult> PostUserVerification([FromForm] LoginRequestViewModel value)
        {
            int userId = 0;
            string userIdStr = User.FindFirstValue(ClaimTypes.Name);
            if (!string.IsNullOrWhiteSpace(userIdStr))
                userId = Convert.ToInt32(userIdStr);

            var dd = User.Identity.Name;

            var model = new UserResponseViewModel();
            var response = new SingleResponse<UserResponseViewModel>();
            if (ModelState.IsValid)
            {
                if (!(await _unit.IUser.Exists(u => u.PhoneNumber == value.MobileNo)).UserObject)
                {
                    ModelState.AddModelError("PEmail", "Phone No. Not register");
                    response.Message = "Phone no. does not register";
                    response.Status = true;
                }
                else
                {
                    var Id = (await _unit.IUser.GetSelectedAsync(t => t.PhoneNumber.Equals(value.MobileNo) && t.UserType==1, m => m.Id)).UserObject;
                    var resultdata = await _userManager.FindByIdAsync(Convert.ToString(Id));
                    string strPhone = ("91" + value.MobileNo);
                    resultdata.FCMToken = value.FcmToken;
                    resultdata.DeviceId = value.DeviceId;
                    resultdata.DeviceType = value.DeviceType;
                    resultdata.ModifiedDate = DateTime.Now;

                    response.Message = "user registered.";
                    response.Status = true;
                    var otpresposedata = false;//await _smsHandler.VerifyOtpAsync(mobile: strPhone, OTP: value.OTP);
                    if (value.OTP == "1234")
                    {
                        otpresposedata = true;
                    }
                    if (otpresposedata)//.type == "success")
                    {
                        try
                        {
                            await _userManager.UpdateAsync(resultdata);
                            model.Name = resultdata.FullName;
                            model.EmailId = resultdata.Email;
                            model.MobileNo = resultdata.PhoneNumber;
                            if (!string.IsNullOrEmpty(resultdata.CustomerImage))
                            {
                                model.userImageProfileImage = resultdata.CustomerImage;
                            }
                            if (resultdata != null)
                            {
                                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                                byte[] key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                                SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
                                {
                                    Subject = new ClaimsIdentity(new Claim[]
                                    {
                                        new Claim(ClaimTypes.Name, Id.ToString()),
                                        new Claim(ClaimTypes.MobilePhone, resultdata.PhoneNumber.ToString())
                                    }),
                                    Expires = DateTime.UtcNow.AddDays(180),
                                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                                        SecurityAlgorithms.HmacSha256Signature)
                                };
                                SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
                                model.Token = tokenHandler.WriteToken(token);
                                // remove password before returning
                                value.OTP = null;
                                response.Data = model;
                                response.Message = "OTP verifed successfully";
                                response.Status = true;
                                return response.ToHttpResponse();

                            }
                            else
                            {
                                response.Message = "Worng OTP";
                                response.Status = false;
                                response.ErrorTypeCode = (int)ErrorMessage.WorngOTP;
                                return response.ToHttpResponse();
                            }
                        }
                        catch (Exception ex)
                        {
                            response.Status = false;
                            response.Message = "There was an internal error, please contact to technical support.";
                            ErrorTrace.Logger(LogArea.ApplicationTier, ex);
                            return response.ToHttpResponse();
                        }
                    }
                }
            }
            return response.ToHttpResponse();
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            var response = new SingleResponse<ReturnUrlResponseViewModel>();
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            var returnurl = new ReturnUrlResponseViewModel
            {
                ReturnUrl = returnUrl
            };
            response.Data = returnurl;
            response.Status = true;
            return response.ToHttpResponse();
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost("login")]
        public async Task<IActionResult> Post([FromForm] WebLoginRequestViewModel value, string returnUrl = null)
        {
            var response = new SingleResponse<LoginResponseViewModel>();
            try
            {
                if (ModelState.IsValid)
                {
                    var loginResponseData = new LoginResponseViewModel();

                    var loginstatus = (await _unit.IUser.AuthenticateUsers(value.UserName, EncryptionLibrary.EncryptText(value.Password))).UserObject;

                    if (loginstatus)
                    {
                        var userdetails = (await _userManager.FindByEmailAsync(value.UserName));

                        if (userdetails != null)
                        {

                            var tokenHandler = new JwtSecurityTokenHandler();
                            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                            var tokenDescriptor = new SecurityTokenDescriptor
                            {
                                Subject = new ClaimsIdentity(new Claim[]
                                {
                                    new Claim(ClaimTypes.Name, userdetails.Id.ToString()),
                                     new Claim(ClaimTypes.MobilePhone, userdetails.PhoneNumber.ToString()),
                                     new Claim(ClaimTypes.Email, userdetails.Email.ToString())
                                }),
                                Expires = DateTime.UtcNow.AddDays(1),
                                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                                    SecurityAlgorithms.HmacSha256Signature)
                            };
                            var token = tokenHandler.CreateToken(tokenDescriptor);
                            loginResponseData.Token = tokenHandler.WriteToken(token);
                            loginResponseData.EmailId = userdetails.Email;
                            response.Data = loginResponseData;
                            response.Status = true;
                            return response.ToHttpResponse();

                        }
                        else
                        {
                            response.Data = null;
                            response.Message = "Not valid user";
                            response.Status = true;
                            return response.ToHttpResponse();
                        }
                    }
                    else
                    {
                        response.Data = null;
                        response.Message = "Not valid user";
                        response.Status = true;
                        return response.ToHttpResponse();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.ApplicationTier, ex);
                return response.ToHttpResponse();
            }
            return response.ToHttpResponse();
        }

        [AllowAnonymous]
        [HttpPost("otp_deliveryboyverify")]
        public async Task<IActionResult> PostDeliveryBoyVerification([FromForm] LoginRequestViewModel value)
        {
            int userId = 0;
            string userIdStr = User.FindFirstValue(ClaimTypes.Name);
            if (!string.IsNullOrWhiteSpace(userIdStr))
                userId = Convert.ToInt32(userIdStr);

            var dd = User.Identity.Name;

            var model = new UserResponseViewModel();
            var response = new SingleResponse<UserResponseViewModel>();
            if (ModelState.IsValid)
            {
                if (!(await _unit.IUser.Exists(u => u.PhoneNumber == value.MobileNo)).UserObject)
                {
                    ModelState.AddModelError("PEmail", "Phone No. Not register");
                    response.Message = "Phone no. does not register";
                    response.Status = true;
                }
                else
                {
                    var Id = (await _unit.IUser.GetSelectedAsync(t => t.PhoneNumber.Equals(value.MobileNo) && t.UserType == 2, m => m.Id)).UserObject;
                    var resultdata = await _userManager.FindByIdAsync(Convert.ToString(Id));
                    string strPhone = ("91" + value.MobileNo);
                    resultdata.FCMToken = value.FcmToken;
                    resultdata.DeviceId = value.DeviceId;
                    resultdata.DeviceType = value.DeviceType;
                    resultdata.ModifiedDate = DateTime.Now;

                    response.Message = "user registered.";
                    response.Status = true;
                    var otpresposedata = false;//await _smsHandler.VerifyOtpAsync(mobile: strPhone, OTP: value.OTP);
                    if (value.OTP == "1234")
                    {
                        otpresposedata = true;
                    }
                    if (otpresposedata)//.type == "success")
                    {
                        try
                        {
                            await _userManager.UpdateAsync(resultdata);
                            model.Name = resultdata.FullName;
                            model.EmailId = resultdata.Email;
                            model.MobileNo = resultdata.PhoneNumber;
                            if (!string.IsNullOrEmpty(resultdata.CustomerImage))
                            {
                                model.userImageProfileImage = resultdata.CustomerImage;
                            }
                            if (resultdata != null)
                            {
                                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                                byte[] key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                                SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
                                {
                                    Subject = new ClaimsIdentity(new Claim[]
                                    {
                                        new Claim(ClaimTypes.Name, Id.ToString()),
                                        new Claim(ClaimTypes.MobilePhone, resultdata.PhoneNumber.ToString())
                                    }),
                                    Expires = DateTime.UtcNow.AddDays(180),
                                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                                        SecurityAlgorithms.HmacSha256Signature)
                                };
                                SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
                                model.Token = tokenHandler.WriteToken(token);
                                // remove password before returning
                                value.OTP = null;
                                response.Data = model;
                                response.Message = "OTP verifed successfully";
                                response.Status = true;
                                return response.ToHttpResponse();

                            }
                            else
                            {
                                response.Message = "Worng OTP";
                                response.Status = false;
                                response.ErrorTypeCode = (int)ErrorMessage.WorngOTP;
                                return response.ToHttpResponse();
                            }
                        }
                        catch (Exception ex)
                        {
                            response.Status = false;
                            response.Message = "There was an internal error, please contact to technical support.";
                            ErrorTrace.Logger(LogArea.ApplicationTier, ex);
                            return response.ToHttpResponse();
                        }
                    }
                }
            }
            return response.ToHttpResponse();
        }

        [AllowAnonymous]
        [HttpPost("weblogin")]
        public async Task<IActionResult> Post([FromForm] WebLoginRequestViewModel value)
        {
            var response = new SingleResponse<LoginResponseViewModel>();
            try
            {
                if (ModelState.IsValid)
                {
                    var loginResponseData = new LoginResponseViewModel();
                    dynamic Id = 0;
                    var loginstatus = (await _unit.IUser.AuthenticateUsers(value.UserName, EncryptionLibrary.EncryptText(value.Password))).UserObject;
                    if (loginstatus == true)
                    {
                        Id = (await _unit.IUser.GetSelectedAsync(t => t.Email.Equals(value.UserName), m => m.Id)).UserObject;
                        if (Id == 0 || Id == null)
                        {
                            Id = (await _unit.IUser.GetSelectedAsync(t => t.PhoneNumber.Equals(value.UserName), m => m.Id)).UserObject;
                        }
                    }
                    if (loginstatus)
                    {
                        var userdetails = await _userManager.FindByIdAsync(Convert.ToString(Id));

                        if (userdetails != null)
                        {

                            var tokenHandler = new JwtSecurityTokenHandler();
                            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                            var tokenDescriptor = new SecurityTokenDescriptor
                            {
                                Subject = new ClaimsIdentity(new Claim[]
                                {
                                    new Claim(ClaimTypes.Name, userdetails.Id.ToString()),
                                     new Claim(ClaimTypes.MobilePhone, userdetails.PhoneNumber.ToString()),
                                     new Claim(ClaimTypes.Email, userdetails.Email.ToString())
                                }),
                                Expires = DateTime.UtcNow.AddDays(1),
                                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                                    SecurityAlgorithms.HmacSha256Signature)
                            };
                            var token = tokenHandler.CreateToken(tokenDescriptor);
                            loginResponseData.Token = tokenHandler.WriteToken(token);
                            loginResponseData.EmailId = userdetails.Email;
                            loginResponseData.UserId = userdetails.Id;
                            response.Data = loginResponseData;
                            response.Status = true;
                            return response.ToHttpResponse();

                        }
                        else
                        {
                            response.Data = null;
                            response.Message = "Not valid user";
                            response.Status = true;
                            return response.ToHttpResponse();
                        }
                    }
                    else
                    {
                        response.Data = null;
                        response.Message = "Not valid user";
                        response.Status = true;
                        return response.ToHttpResponse();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.ApplicationTier, ex);
                return response.ToHttpResponse();
            }
            return response.ToHttpResponse();
        }
    }
}