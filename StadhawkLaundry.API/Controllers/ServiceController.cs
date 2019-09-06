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


        // GET: api/Service
        [HttpGet("services")]
        public async Task<IEnumerable<TblService>> GetAsync()
        {
            return (await _unit.IService.GetAll()).UserObject;
        }

        // GET: api/Service/5
        [HttpGet("edit/{id}")]
        public async Task<ServicesViewModel> Get(Guid id)
        {
            var result = (await _unit.IService.GetByID(Id: id)).UserObject;
            var data = AutoMapper.Mapper.Map<ServicesViewModel>(result);
            data.ServiceId = result.Id.ToString();
            return data;
        }

        [HttpPost("add")]
        public async Task<HttpResponseMessage> Post([FromForm] ServicesViewModel value)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isEdit = true;
                    Guid ServiceGuid = Guid.Empty;
                    isEdit = !string.IsNullOrEmpty(value.ServiceId) ? Guid.TryParse(value.ServiceId, out ServiceGuid) : false;
                    if ((await _unit.IService.Exists(t => t.Name == value.Name)).UserObject)
                    {
                        var response = new HttpResponseMessage()
                        {
                            StatusCode = HttpStatusCode.Conflict
                        };
                        return response;
                    }
                    else
                    {
                        var data = AutoMapper.Mapper.Map<TblService>(value);
                        if (!isEdit)
                        {
                            await _imageHandler.UploadImage(value.ServiceImage);

                            data.Id = string.IsNullOrEmpty(value.ServiceId) ? Guid.NewGuid() : ServiceGuid;
                            data.CreatedDate = DateTime.Now;
                            data.ServiceImage = value.ServiceImage.FileName;
                            data.IsDeleted = false;
                            await _unit.IService.Add(data);
                            var result = await _unit.Complete();
                            if (result.ResultType == ApiResultType.Success)
                            {
                                var response = new HttpResponseMessage()
                                {
                                    StatusCode = HttpStatusCode.OK
                                };
                                return response;
                            }
                        }
                        else
                        {
                            data = (await _unit.IService.GetByID(Id: ServiceGuid)).UserObject;
                            data.Name = value.Name;
                            data.ModifiedDate = DateTime.Now;
                            data.ModifiedBy = null;
                            _unit.IService.Update(data);
                            var result = await _unit.Complete();
                            if (result.ResultType == ApiResultType.Success)
                            {
                                var response = new HttpResponseMessage()
                                {
                                    StatusCode = HttpStatusCode.OK
                                };
                                return response;
                            }
                        }
                    }

                }
                else
                {
                    var response = new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.ApplicationTier, ex);
                var response = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest
                };
                return response;
            }
            var response1 = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.BadRequest
            };
            return response1;
        }


        // DELETE: api/ApiWithActions/5
        [HttpDelete("delete/{id}")]
        public async Task<HttpResponseMessage> Delete(Guid id)
        {
            if (id != null)
            {
                await _unit.IService.Remove(id);
                var result = await _unit.Complete();

                if (result.ResultType == ApiResultType.Success)
                {
                    var response = new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.OK
                    };
                    return response;
                }
                else
                {
                    var response = new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.BadRequest
                    };
                    return response;
                }

            }
            else
            {
                var response = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest
                };
                return response;
            }
        }
        /// <summary>
        /// get meal pass detail by customer id
        /// </summary>
        /// <returns></returns>
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
