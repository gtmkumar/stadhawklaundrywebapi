using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stadhawk.Laundry.Utility.ResponseUtility;
using StadhawkLaundry.BAL.Core;
using StadhawkLaundry.ViewModel.ResponseModel;
using Utility;

namespace StadhawkLaundry.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly IUnitOfWork _unit;
        public StoreController(IUnitOfWork unit)
        {
            _unit = unit;
        }

        [HttpGet("getservicemaster")]
        public async Task<IActionResult> Get()
        {
            int customerId = 0;
            string userId = User.FindFirstValue(ClaimTypes.Name);
            if (!string.IsNullOrWhiteSpace(userId))
                customerId = Convert.ToInt32(userId);

            var response = new ListResponse<ServiceLabelMasterResponseViewModel>();
            var data = await _unit.IService.GetServiceMaster(customerId);
            if (data.HasSuccess)
            {
                response.Data = data.UserObject;
                response.Status = true;
            }
            else
            {
                response.Data = null;
                response.Status = false;
            }
            return response.ToHttpResponse();
        }
    }
}