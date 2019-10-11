using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stadhawk.Laundry.Utility.Enums;
using Stadhawk.Laundry.Utility.ResponseUtility;
using StadhawkLaundry.API.Models;
using StadhawkLaundry.BAL.Core;
using StadhawkLaundry.ViewModel.ResponseModel;
using Utility;

namespace StadhawkLaundry.API.Controllers
{
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
                strStatus = "4,5,6,7";

            if (orderType == (int)EnumType.OrderTypeEnum.HISTORY)
                strStatus = "2,3,8,3";

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
    }
}