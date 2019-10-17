using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stadhawk.Laundry.Utility.Enums;
using Stadhawk.Laundry.Utility.ResponseUtility;
using StadhawkLaundry.API.Models;
using StadhawkLaundry.BAL.Core;
using StadhawkLaundry.ViewModel.RequestModel;
using StadhawkLaundry.ViewModel.ResponseModel;
using Utility;

namespace StadhawkLaundry.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryOrderController : ControllerBase
    {
        private readonly IUnitOfWork _unit;
        private readonly AppSettings _appSettings;
        public DeliveryOrderController(IOptions<AppSettings> appSettings, IUnitOfWork unit)
        {
            _appSettings = appSettings.Value;
            _unit = unit;
        }
        [HttpGet("orderdetails")]
        public async Task<IActionResult> GetOrderDetail([FromQuery]int orderType)
        {
            int? userId = 0;
            string strStatus = string.Empty;
            var userStrId = this.User.FindFirstValue(ClaimTypes.Name);
            if (!string.IsNullOrWhiteSpace(userStrId))
                userId = Convert.ToInt32(userStrId);

            if (orderType == (int)EnumType.OrderTypeEnum.UPCOMING)
                strStatus = "5,11,12,13";

            if (orderType == (int)EnumType.OrderTypeEnum.HISTORY)
                strStatus = "14,8";

            var ownResponse = new ListResponse<OrderDetailResponseViewModel>();
            var dataResult = await _unit.IOrder.GetOrderByDeliveryBoyId(userId.Value, strStatus);
            if (dataResult.HasSuccess)
            {
                ownResponse.Message = "Success";
                ownResponse.Status = true;
                ownResponse.Data = dataResult.UserObject;
                return ownResponse.ToHttpResponse();
            }
            else
            {
                ownResponse.Message = "No data found";
                ownResponse.Status = true;
                ownResponse.Data = dataResult.UserObject;
                return ownResponse.ToHttpResponse();
            }
        }

        [HttpGet("orderdetail")]
        public async Task<IActionResult> GetOrderDetailByOrderId([FromQuery]int orderId)
        {
            int? userId = 0;
            string strStatus = string.Empty;
            var userStrId = this.User.FindFirstValue(ClaimTypes.Name);
            if (!string.IsNullOrWhiteSpace(userStrId))
                userId = Convert.ToInt32(userStrId);


            var ownResponse = new SingleResponse<OrderDetailResponseModel>();
            var dataResult = await _unit.IOrder.GetOrderByOrderId(userId.Value, orderId);
            if (dataResult.HasSuccess)
            {
                ownResponse.Message = "Success";
                ownResponse.Status = true;
                ownResponse.Data = dataResult.UserObject;
                return ownResponse.ToHttpResponse();
            }
            else
            {
                ownResponse.Message = "No data found";
                ownResponse.Status = true;
                ownResponse.Data = dataResult.UserObject;
                return ownResponse.ToHttpResponse();
            }
        }

        [HttpGet("updateorderstatus")]
        public async Task<IActionResult> PostUpdateOrderSatus([FromForm]OrderStatusUpdateRequestModel model)
        {
            int? userId = 0;
            string strStatus = string.Empty;
            var userStrId = this.User.FindFirstValue(ClaimTypes.Name);
            if (!string.IsNullOrWhiteSpace(userStrId))
                userId = Convert.ToInt32(userStrId);


            var ownResponse = new Response();
            var dataResult = await _unit.IOrder.UpdateOrderStatus(model, userId.Value);
            if (dataResult.HasSuccess)
            {
                ownResponse.Message = "Success";
                ownResponse.Status = true;
                return ownResponse.ToHttpResponse();
            }
            else
            {
                ownResponse.Message = "No data found";
                ownResponse.Status = false;
                return ownResponse.ToHttpResponse();
            }
        }


    }
}