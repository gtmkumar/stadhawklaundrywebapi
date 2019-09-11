using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stadhawk.Laundry.Utility.ResponseUtility;
using StadhawkCoreApi.Logger;
using StadhawkLaundry.BAL.Core;
using StadhawkLaundry.ViewModel.RequestModel;
using Utility;

namespace StadhawkLaundry.API.Controllers
{
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unit;
        public CartController(IUnitOfWork unit)
        {
            _unit = unit;
        }
        public async IActionResult AddCart(AddCartRequestViewModel model)
        {
            int? userId = 0;
            var userStrId = this.User.FindFirstValue(ClaimTypes.Name);
            if (!string.IsNullOrWhiteSpace(userStrId))
                userId = Convert.ToInt32(userId);

            var response = new Response();
            try
            {
                if (model.IsRemove.Value)
                {
                    var result = await _unit.ICart.AddToCartAsync(model, userId: userId.Value);
                    //var data = (await _unit.AddtCart.GetCustomerItemCartCountAndPrice(customerId: customerId.Value, itemId: model.Itemid)).UserObject;
                    if (result.HasSuccess)
                    {
                        if (result.UserObject == false)
                        {
                            response.Message = "Item not avilable in selected address";
                            response.Status = false;
                            return response.ToHttpResponse();
                        }
                        //response.Data = data;
                        response.Message = "Success";
                        response.Status = true;
                    }
                    else
                    {
                        //response.Data = null;
                        response.Message = "Error";
                        response.Status = false;
                    }
                }
                else
                {
                   // response.Data = null;
                    response.ErrorTypeCode = (int)ErrorMessage.CartPartnerMatched;
                    response.Status = false;
                }
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

        [HttpPost("removecart")]
        [ProducesResponseType(200)]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteCart([FromForm] AddCartRequestViewModel model)
        {
            int? customerId = 0;
            string mobileNo = string.Empty;
            var userId = this.User.FindFirstValue(ClaimTypes.Name);
            if (!string.IsNullOrWhiteSpace(userId))
                customerId = Convert.ToInt32(userId);

            var response = new Response();
            try
            {

                model.IsRemove = true;
                var result = (await _unit.ICart.Remove(1));
                var data = (await _unit.ICart.GetByID(1));

                if (result.ResultType == StadhawkCoreApi.ApiResultType.Success)
                {
                    //response.Data = data;
                    response.Message = "Success";
                    response.Status = true;
                }
                else
                {
                    //response.Data = null;
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
    }
}