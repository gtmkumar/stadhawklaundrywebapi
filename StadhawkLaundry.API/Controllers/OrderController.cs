using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stadhawk.Laundry.Utility.Enums;
using Stadhawk.Laundry.Utility.ResponseUtility;
using StadhawkLaundry.API.Common;
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
    public class OrderController : ControllerBase
    {
        private readonly IUnitOfWork _unit;
        public OrderController(IUnitOfWork unit)
        {
            _unit = unit;
        }
        [HttpPost("createorder")]
        public async Task<IActionResult> PostCreateAsync([FromForm]OrderRequestViewModel model)
        {
            int? userId = 0;
            var userStrId = this.User.FindFirstValue(ClaimTypes.Name);
            if (!string.IsNullOrWhiteSpace(userStrId))
                userId = Convert.ToInt32(userStrId);

            var response = new SingleResponse<OrderResponseViewModel>();
            var result = await _unit.IOrder.CreateOrder(userId.Value,model);
            if (result.HasSuccess)
            {
                response.Data = result.UserObject;
                response.Message = "Success";
                response.Status = true;
            }
            else
            {
                response.Data = null;
                response.Message = "Error";
                response.Status = false;
            }

            return response.ToHttpResponse();
        }

        [HttpGet("pickslotDetails")]
        public async Task<IActionResult> GetSlot()
        {
            var ownResponse = new ListResponse<TimeSlotViewModel>();
            var dataResult = await _unit.IOrder.GetAvailableSlots();
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

        [HttpGet("dropslotdetails")]
        public async Task<IActionResult> GetDropSlot([FromQuery]SlotsRequestViewModel model)
        {
            var ownResponse = new ListResponse<TimeSlotViewModel>();
            string fullDate;
            try
            {
                var sDate = model.FullDate.Split(' ');
                fullDate = Common.DateTimeConverter.DatetimeConverterfromString(sDate[0], format: DateTimeConverter.DateFormat.YYMMDD);
                fullDate = fullDate + " " + sDate[1];
                DateTime dd = Convert.ToDateTime(fullDate);
            }
            catch (Exception ex)
            {
                throw;
            }
          
            var dataResult = await _unit.IOrder.GetAvailableDropSlots(Convert.ToDateTime(model.FullDate));
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


        [HttpGet("orderdetails")]
        public async Task<IActionResult> GetOrderDetail([FromQuery]int orderType)
        {
            int? userId = 0;
            string strStatus = string.Empty;
            var userStrId = this.User.FindFirstValue(ClaimTypes.Name);
            if (!string.IsNullOrWhiteSpace(userStrId))
                userId = Convert.ToInt32(userStrId);

            if (orderType == (int)EnumType.OrderTypeEnum.UPCOMING)
                strStatus = "2,3,8";

            if (orderType == (int)EnumType.OrderTypeEnum.HISTORY)
                strStatus = "1,4,5,6,7";

            var ownResponse = new ListResponse<OrderDetailResponseViewModel>();
            var dataResult = await _unit.IOrder.GetOrderByUser(userId.Value, strStatus);
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