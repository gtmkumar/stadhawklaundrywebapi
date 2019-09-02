using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StadhawkLaundry.BAL.Core;
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

        [HttpGet("itemdetail")]
        public async Task<OrderViewModel> GetAsync()
        {
            return (await _unit.IOrder.GetItemDetails()).UserObject;
        }
    }
}