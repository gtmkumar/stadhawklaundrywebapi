using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stadhawk.Laundry.Utility.Enums;
using Stadhawk.Laundry.Utility.ResponseUtility;
using StadhawkLaundry.API.Common;
using StadhawkLaundry.API.Models;
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
        private readonly AppSettings _appSettings;
        public OrderController(IOptions<AppSettings> appSettings, IUnitOfWork unit)
        {
            _appSettings = appSettings.Value;
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
            var result = await _unit.IOrder.CreateOrder(userId.Value, model);
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
                var dd = Convert.ToDateTime(fullDate);
            }
            catch (Exception ex)
            {
                throw;
            }

            var dataResult = await _unit.IOrder.GetAvailableDropSlots(Convert.ToDateTime(fullDate));
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
                strStatus = "4,5,6,7";

            if (orderType == (int)EnumType.OrderTypeEnum.HISTORY)
                strStatus = "2,3,8,3";

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
        [HttpGet("getvisibility")]
        public async Task<IActionResult> GetVisibility()
        {
            int? customerId = 0;
            string userId = User.FindFirstValue(ClaimTypes.Name);
            if (!string.IsNullOrWhiteSpace(userId))
                customerId = Convert.ToInt32(userId);

            var pgVisibility = new PgVisibilityResponseViewModel();
            pgVisibility.PgVisibility = _appSettings.PgVisibility == "true" ? true : false;
            pgVisibility.PaytmVisibility = _appSettings.PaytmVisibility == "true" ? true : false;
            pgVisibility.CODVisibility = _appSettings.CODVisibility == "true" ? true : false;
            var ownResponse = new SingleResponse<PgVisibilityResponseViewModel>
            {
                Data = pgVisibility,
                Message = "Get Pg Visibility",
                Status = true
            };
            return ownResponse.ToHttpResponse();
        }

    }
}