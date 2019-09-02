using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StadhawkCoreApi;
using StadhawkCoreApi.Logger;
using StadhawkLaundry.BAL.Core;
using StadhawkLaundry.BAL.Core.IRepositories;
using StadhawkLaundry.DataModel.Models;
using StadhawkLaundry.ViewModel;

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
        
        [HttpGet("categories")]
        public async Task<IEnumerable<CategoryViewModel>> Get()
        {
            return (await _unit.ICategory.GetCategoryWithServiceData()).UserObject;
        }

        [HttpGet("categorybyserviceid/{id}")]
        public async Task<IEnumerable<CategoryViewModel>> GetCategoryByServiceId(Guid Id)
        {
            return (await _unit.ICategory.GetSelectedDataAsync(t => t.ServiceId == Id, d => new CategoryViewModel { CategoryId = d.Id.ToString(), Name = d.Name })).UserObject;
        }
        // GET: api/Category/5
        [HttpGet("edit/{id}")]
        public async Task<CategoryViewModel> Get(Guid id)
        {
            var result = (await _unit.ICategory.GetByID(Id: id)).UserObject;
            var data = AutoMapper.Mapper.Map<CategoryViewModel>(result);
            data.CategoryId = result.Id.ToString();
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
                    isEdit = !string.IsNullOrEmpty(value.CategoryId) ? Guid.TryParse(value.CategoryId, out CategoryGuid) : false;
                    if ((await _unit.ICategory.Exists(t => t.Name == value.Name && t.ServiceId == value.ServiceId)).UserObject)
                    {
                        var response = new HttpResponseMessage()
                        {
                            StatusCode = HttpStatusCode.Conflict
                        };
                        return response;
                    }
                    else
                    {
                        var data = AutoMapper.Mapper.Map<TblCategory>(value);
                        if (!isEdit)
                        {
                            data.Id = string.IsNullOrEmpty(value.CategoryId) ? Guid.NewGuid() : CategoryGuid;
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
                            data.Name = value.Name;
                            data.ServiceId = value.ServiceId;
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

        // DELETE: api/ApiWithActions/5
        [HttpDelete("delete/{id}")]
        public async Task<HttpResponseMessage> Delete(Guid id)
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
    }
}
