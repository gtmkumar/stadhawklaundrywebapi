using System;
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
            bool isEdit = false;
            int? userId = 0;
            bool cartremovelData = false;
            var userStrId = this.User.FindFirstValue(ClaimTypes.Name);
            if (!string.IsNullOrWhiteSpace(userStrId))
                userId = Convert.ToInt32(userStrId);

            if (model.CartId > 0)
                isEdit = true;

            if (model.IsCartRemoved)
            {
                cartremovelData = (await _unit.ICart.AllCalrtDeleteByUser(userId.Value)).UserObject;
            }

            var response = new SingleResponse<CartCountResponseViewModel>();
            var cartDatCheck = (await _unit.ICart.IsCartFromDiffrentService(model.StoreItemId, userId.Value)).UserObject;
            if (!cartremovelData)
            {
                if (cartDatCheck.isDifferent.HasValue)
                {
                    if (cartDatCheck.isDifferent != model.IsKg)
                    {
                        response.Data = null;
                        response.Message = "Cart from different service";
                        response.Status = true;
                        response.ErrorTypeCode = (int)ErrorMessage.CartRemoverd;
                        return response.ToHttpResponse();
                    }
                }
            }
            try
            {
                var data = AutoMapper.Mapper.Map<TblCart>(model);
                if (!isEdit)
                {
                    data.CreateDate = DateTime.Now;
                    data.ModifyDate = DateTime.Now;
                    data.IsOrderPlaced = false;
                    data.IsDeleted = false;
                    data.UserId = userId;
                    data.Quantity = 1;
                    var result = _unit.ICart.Add(data);
                }
                else
                {
                    data = (await _unit.ICart.GetByID(model.CartId)).UserObject;
                    data.ModifyDate = DateTime.Now;
                    data.Quantity = data.Quantity + 1;

                    _unit.ICart.Update(data);
                }
                var isDataSaved = await _unit.Complete();
                if (isDataSaved.ResultType == StadhawkCoreApi.ApiResultType.Success)
                {
                    var dataResult = (await _unit.ICart.CartCountAndPrice(userId: userId.Value, _appSettings.DataBaseCon));
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

        [HttpPost("removecart")]
        public async Task<IActionResult> DeleteCart([FromForm] AddCartRequestViewModel model)
        {
            int? userId = 0;
            model.IsCartRemoved = false;
            var userStrId = this.User.FindFirstValue(ClaimTypes.Name);
            if (!string.IsNullOrWhiteSpace(userStrId))
                userId = Convert.ToInt32(userStrId);

            var response = new SingleResponse<CartCountResponseViewModel>();
            try
            {
                var data = AutoMapper.Mapper.Map<TblCart>(model);
                data = (await _unit.ICart.GetByID(model.CartId)).UserObject;
                data.ModifyDate = DateTime.Now;
                data.Quantity = data.Quantity - 1;
                if (data.Quantity == 0)
                {
                    data.IsDeleted = true;
                }
                _unit.ICart.Update(data);

                var isDataSaved = await _unit.Complete();
                if (isDataSaved.ResultType == StadhawkCoreApi.ApiResultType.Success)
                {
                    var dataResult = (await _unit.ICart.CartCountAndPrice(userId: userId.Value, _appSettings.DataBaseCon));
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
                var dataResult = (await _unit.ICart.CartCountAndPrice(userId: userId.Value, _appSettings.DataBaseCon));
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