using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StadhawkCoreApi;
using StadhawkCoreApi.Logger;
using StadhawkLaundry.API.Handler;
using StadhawkLaundry.BAL.Core;
using StadhawkLaundry.DataModel.Models;
using StadhawkLaundry.ViewModel;
using System.Security.Claims;
using Stadhawk.Laundry.Utility.ResponseUtility;
using Utility;
using StadhawkLaundry.ViewModel.ResponseModel;

namespace StadhawkLaundry.API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IImageHandler _imageHandler;
        private readonly IUnitOfWork _unit;
        public ServiceController(IUnitOfWork unit, IImageHandler imageHandler)
        {
            _imageHandler = imageHandler;
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

        [HttpGet("getservicebystore")]
        public async Task<IActionResult> GetServiceByStore([FromQuery]int storeId)
        {
            int customerId = 0;
            string userId = User.FindFirstValue(ClaimTypes.Name);
            if (!string.IsNullOrWhiteSpace(userId))
                customerId = Convert.ToInt32(userId);

            var response = new ListResponse<ServiceMasterResponseViewModel>();
            var data = await _unit.IService.GetServiceByStore(storeId);
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
