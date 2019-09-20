using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stadhawk.Laundry.Utility.ResponseUtility;
using StadhawkLaundry.BAL.Core;
using StadhawkLaundry.DataModel.Models;
using StadhawkLaundry.ViewModel;
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
        [HttpGet("createorder")]
        public async Task<OrderViewModel> PostCreateAsync()
        {
            int? userId = 0;
            var userStrId = this.User.FindFirstValue(ClaimTypes.Name);
            if (!string.IsNullOrWhiteSpace(userStrId))
                userId = Convert.ToInt32(userStrId);

            var response = new SingleResponse<OrderViewModel>();

            var result = await _unit.IOrder.CreateOrder(userId.Value);
            return (await _unit.IOrder.GetItemDetails()).UserObject;
        }

        [HttpGet("slotDetails")]
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

    }
}