﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stadhawk.Laundry.Utility.ResponseUtility;
using StadhawkCoreApi.Logger;
using StadhawkLaundry.API.Models;
using StadhawkLaundry.BAL.Core;
using StadhawkLaundry.DataModel.Models;
using StadhawkLaundry.ViewModel.RequestModel;
using StadhawkLaundry.ViewModel.ResponseModel;
using Utility;

namespace StadhawkLaundry.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IUnitOfWork _unit;
        private readonly AppSettings _appSettings;
        public CartController(IOptions<AppSettings> appSettings, IUnitOfWork unit)
        {
            _unit = unit;
            _appSettings = appSettings.Value;
        }
        [HttpPost("addcart")]
        public async Task<IActionResult> AddCart([FromForm]AddCartRequestViewModel model)
        {
            int? userId = 0;
            var userStrId = this.User.FindFirstValue(ClaimTypes.Name);
            if (!string.IsNullOrWhiteSpace(userStrId))
                userId = Convert.ToInt32(userStrId);

            var response = new SingleResponse<CartCountResponseViewModel>();
            try
            {
                var result = (await _unit.ICart.AddToCartAsync(model, userId: userId.Value)).UserObject;
                if (result)
                {
                    response.Data = null;
                    response.Message = "Cart from different service";
                    response.Status = true;
                    response.ErrorTypeCode = (int)ErrorMessage.CartRemoverd;
                    return response.ToHttpResponse();
                }
                else
                {
                    var dataResult = (await _unit.ICart.CartCountAndPrice(userId: userId.Value));
                    response.Data = dataResult.HasSuccess ? dataResult.UserObject : null;
                    response.Message = "Cart added";
                    response.Status = true;
                    return response.ToHttpResponse();
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

        [HttpPost("removecart")]
        public async Task<IActionResult> DeleteCart([FromForm] AddCartRequestViewModel model)
        {
            int? userId = 0;
            var userStrId = this.User.FindFirstValue(ClaimTypes.Name);
            if (!string.IsNullOrWhiteSpace(userStrId))
                userId = Convert.ToInt32(userStrId);

            var response = new SingleResponse<CartCountResponseViewModel>();
            try
            {
               var isDataRemoved = await _unit.ICart.CartRemove(model.CartId.Value);
                if (isDataRemoved.UserObject)
                {
                    var dataResult = (await _unit.ICart.CartCountAndPrice(userId: userId.Value));
                    response.Data = dataResult.HasSuccess ? dataResult.UserObject : null;
                    response.Message = "Cart added";
                    response.Status = true;
                    return response.ToHttpResponse();
                }
                else
                {
                    response.Data = null;
                    response.Message = "no cart added";
                    response.Status = true;
                    return response.ToHttpResponse();
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

        [HttpGet("getcart")]
        public async Task<IActionResult> GetCart([FromQuery]int addressId)
        {
            int? userId = 0;
            var userStrId = this.User.FindFirstValue(ClaimTypes.Name);
            if (!string.IsNullOrWhiteSpace(userStrId))
                userId = Convert.ToInt32(userStrId);

            var response = new SingleResponse<CartPriceDetail>();
            try
            {
                response.Data = (await _unit.ICart.GetCartDetail(userId.Value, addressId)).UserObject;
                response.Status = true;
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "There was an internal error, please contact to technical support.";
                ErrorTrace.Logger(LogArea.ApplicationTier, ex);
            }
            return response.ToHttpResponse();
        }

        [HttpGet("getcartdetail")]
        public async Task<IActionResult> GetCartDetail([FromQuery]int addressId)
        {
            int? userId = 0;
            var userStrId = this.User.FindFirstValue(ClaimTypes.Name);
            if (!string.IsNullOrWhiteSpace(userStrId))
                userId = Convert.ToInt32(userStrId);

            var response = new SingleResponse<CartOrderDetailResponseViewModel>();
            try
            {
                response.Data = (await _unit.ICart.GetCartDetails(userId.Value, addressId)).UserObject;
                response.Status = true;
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "There was an internal error, please contact to technical support.";
                ErrorTrace.Logger(LogArea.ApplicationTier, ex);
            }
            return response.ToHttpResponse();
        }

        [HttpGet("getcartcount")]
        public async Task<IActionResult> Getcartcount()
        {
            int? userId = 0;
            var userStrId = this.User.FindFirstValue(ClaimTypes.Name);
            if (!string.IsNullOrWhiteSpace(userStrId))
                userId = Convert.ToInt32(userStrId);

            var response = new SingleResponse<CartCountResponseViewModel>();
            try
            {
                var dataResult = (await _unit.ICart.CartCountAndPrice(userId: userId.Value));
                response.Data = dataResult.HasSuccess ? dataResult.UserObject : null;
                response.Message = "Cart added";
                response.Status = true;
                return response.ToHttpResponse();
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "There was an internal error, please contact to technical support.";
                ErrorTrace.Logger(LogArea.ApplicationTier, ex);
            }
            return response.ToHttpResponse();
        }

        [HttpPost("addservicecart")]
        public async Task<IActionResult> AddServiceCart([FromForm]AddServiceCartRequestViewModel model)
        {
            int? userId = 0;
            var userStrId = this.User.FindFirstValue(ClaimTypes.Name);
            if (!string.IsNullOrWhiteSpace(userStrId))
                userId = Convert.ToInt32(userStrId);

            var response = new SingleResponse<CartCountResponseViewModel>();
            try
            {
                var result = (await _unit.ICart.AddToServiceCartAsync(model, userId: userId.Value)).UserObject;
                if (result)
                {
                    response.Data = null;
                    response.Message = "Cart from different service";
                    response.Status = true;
                    response.ErrorTypeCode = (int)ErrorMessage.CartRemoverd;
                    return response.ToHttpResponse();
                }
                else
                {
                    var dataResult = (await _unit.ICart.CartCountAndPrice(userId: userId.Value));
                    response.Data = dataResult.HasSuccess ? dataResult.UserObject : null;
                    response.Message = "Cart added";
                    response.Status = true;
                    return response.ToHttpResponse();
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

        [HttpGet("getServicecart")]
        public async Task<IActionResult> GetServiceCart([FromQuery]int addressId)
        {
            int? userId = 0;
            var userStrId = this.User.FindFirstValue(ClaimTypes.Name);
            if (!string.IsNullOrWhiteSpace(userStrId))
                userId = Convert.ToInt32(userStrId);

            var response = new SingleResponse<CartPriceDetail>();
            try
            {
                response.Data = (await _unit.ICart.GetCartDetail(userId.Value, addressId)).UserObject;
                response.Status = true;
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "There was an internal error, please contact to technical support.";
                ErrorTrace.Logger(LogArea.ApplicationTier, ex);
            }
            return response.ToHttpResponse();
        }

        [HttpPost("removeservicecart")]
        public async Task<IActionResult> DeleteServiceCart([FromForm] AddCartRequestViewModel model)
        {
            int? userId = 0;
            var userStrId = this.User.FindFirstValue(ClaimTypes.Name);
            if (!string.IsNullOrWhiteSpace(userStrId))
                userId = Convert.ToInt32(userStrId);


            var response = new SingleResponse<CartCountResponseViewModel>();
            try
            {
                var isDataRemoved = await _unit.ICart.CartServiceRemove(model.CartId.Value);
                if (isDataRemoved.UserObject)
                {
                    var dataResult = (await _unit.ICart.CartCountAndPrice(userId: userId.Value));
                    response.Data = dataResult.HasSuccess ? dataResult.UserObject : null;
                    response.Message = "Cart added";
                    response.Status = true;
                    return response.ToHttpResponse();
                }
                else
                {
                    response.Data = null;
                    response.Message = "no cart added";
                    response.Status = true;
                    return response.ToHttpResponse();
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
        
        [HttpGet("getservicecartcount")]
        public async Task<IActionResult> GetServiceCartcount()
        {
            int? userId = 0;
            var userStrId = this.User.FindFirstValue(ClaimTypes.Name);
            if (!string.IsNullOrWhiteSpace(userStrId))
                userId = Convert.ToInt32(userStrId);

            var response = new SingleResponse<CartCountResponseViewModel>();
            try
            {
                var dataResult = (await _unit.ICart.CartCountAndPrice(userId: userId.Value));
                response.Data = dataResult.HasSuccess ? dataResult.UserObject : null;
                response.Message = "Cart added";
                response.Status = true;
                return response.ToHttpResponse();
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "There was an internal error, please contact to technical support.";
                ErrorTrace.Logger(LogArea.ApplicationTier, ex);
            }
            return response.ToHttpResponse();
        }
    }
}
