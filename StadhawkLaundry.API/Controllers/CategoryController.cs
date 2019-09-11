using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stadhawk.Laundry.Utility.ResponseUtility;
using StadhawkCoreApi;
using StadhawkCoreApi.Logger;
using StadhawkLaundry.BAL.Core;
using StadhawkLaundry.BAL.Core.IRepositories;
using StadhawkLaundry.DataModel.Models;
using StadhawkLaundry.ViewModel;
using StadhawkLaundry.ViewModel.RequestModel;
using StadhawkLaundry.ViewModel.ResponseModel;
using Utility;

namespace StadhawkLaundry.API.Controllers
{
    //[Authorize]
    [Route("api/[Controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unit;
        public CategoryController(IUnitOfWork unit)
        {
            _unit = unit;
        }


        [HttpGet("categorybyserviceid/{id}")]
        public async Task<IEnumerable<CategoryViewModel>> GetCategoryByServiceId(int Id)
        {
            return (await _unit.ICategory.GetSelectedDataAsync(t => t.Id == Id, d => new CategoryViewModel { CategoryId = d.Id, Name = d.CategoryName })).UserObject;
        }
        // GET: api/Category/5
        [HttpGet("edit/{id}")]
        public async Task<CategoryViewModel> Get(int id)
        {
            var result = (await _unit.ICategory.GetByID(Id: id)).UserObject;
            var data = AutoMapper.Mapper.Map<CategoryViewModel>(result);
            data.CategoryId = result.Id;
            return data;
        }

        [HttpPost("add")]
        public async Task<HttpResponseMessage> Post([FromBody]CategoryViewModel value)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isEdit = true;
                    Guid CategoryGuid = Guid.Empty;
                    isEdit = value.CategoryId > 0 ? true : false;
                    if ((await _unit.ICategory.Exists(t => t.CategoryName == value.Name && t.Id == value.ServiceId)).UserObject)
                    {
                        var response = new HttpResponseMessage()
                        {
                            StatusCode = HttpStatusCode.Conflict
                        };
                        return response;
                    }
                    else
                    {
                        var data = AutoMapper.Mapper.Map<TblCategoryMaster>(value);
                        if (!isEdit)
                        {
                            data.Id = value.CategoryId;
                            data.CreatedDate = DateTime.Now;
                            data.IsDeleted = false;
                            await _unit.ICategory.Add(data);
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
                            data = (await _unit.ICategory.GetByID(Id: CategoryGuid)).UserObject;
                            data.CategoryName = value.Name;
                            data.Id = value.ServiceId;
                            data.ModifiedDate = DateTime.Now;
                            data.ModifiedBy = null;
                            _unit.ICategory.Update(data);
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

        [HttpDelete("delete/{id}")]
        public async Task<HttpResponseMessage> Delete(int id)
        {
            if (id != null)
            {
                await _unit.ICategory.Remove(id);
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

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategory(CategoryFilterRequest filter)
        {
            int userId = 0;
            var userstr = this.User.FindFirstValue(ClaimTypes.Name);
            if (!string.IsNullOrWhiteSpace(userstr))
                userId = Convert.ToInt32(userId);

            var ownResponse = new ListResponse<CategoryResponseViewModel>();
            var dataResult = await _unit.ICategory.GetCategoryWithServiceData(filter);

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

        [HttpGet("getcategory")]
        public async Task<IActionResult> GetCategoryByService(CategoryFilterRequest filter)
        {
            int userId = 0;
            var userstr = this.User.FindFirstValue(ClaimTypes.Name);

            if (!string.IsNullOrWhiteSpace(userstr))
                userId = Convert.ToInt32(userId);

            var ownResponse = new ListResponse<CategoryResponseViewModel>();
            var dataResult = await _unit.ICategory.GetCategoryByServiceId(filter);
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
