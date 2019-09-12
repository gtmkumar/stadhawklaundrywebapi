using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StadhawkLaundry.BAL.Core;
using StadhawkLaundry.DataModel.Models;
using StadhawkLaundry.ViewModel;

namespace StadhawkLaundry.API.Controllers
{
   // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IUnitOfWork _unit;
        public OrderController(IUnitOfWork unit)
        {
            _unit = unit;
        }
        //[HttpGet("createorder")]
        //public async Task<OrderViewModel> PostOrderAsync()
        //{
        //    int? userId = 0;
        //    var userStrId = this.User.FindFirstValue(ClaimTypes.Name);
        //    if (!string.IsNullOrWhiteSpace(userStrId))
        //        userId = Convert.ToInt32(userStrId);

        //    var data = await _unit.ICart.SingleOrDefault(t => t.UserId == userId && t.IsDeleted == false);
        //    TblOrder orderModel = 

        //    _unit.IOrder.Add()

        //}

        [HttpGet("itemdetail")]
        public async Task<OrderViewModel> GetAsync()
        {
            return (await _unit.IOrder.GetItemDetails()).UserObject;
        }
    }
}