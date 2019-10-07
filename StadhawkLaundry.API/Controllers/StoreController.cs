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
using StadhawkLaundry.ViewModel.ResponseModel;
using Utility;

namespace StadhawkLaundry.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly IUnitOfWork _unit;
        public StoreController(IUnitOfWork unit)
        {
            _unit = unit;
        }

        [HttpGet("getstore")]
        public async Task<IActionResult> Get([FromQuery]int addressId)
        {
            var response = new ListResponse<StoreResponseViewModel>();
            var data = await _unit.IStore.GetStoreByAddress(addressId);
            if (data.HasSuccess)
            {
                response.Data = data.UserObject;
                response.Status = true;
            }
            else
            {
                response.Message = "Data not found";
                response.Data = null;
                response.Status = false;
            }
            return response.ToHttpResponse();
        }
    }
}