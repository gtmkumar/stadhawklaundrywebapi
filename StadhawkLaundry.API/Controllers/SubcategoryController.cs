using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StadhawkCoreApi;
using StadhawkCoreApi.Logger;
using StadhawkLaundry.BAL.Core;
using StadhawkLaundry.DataModel.Models;
using StadhawkLaundry.ViewModel;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.AspNetCore.Authorization;

namespace StadhawkLaundry.API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SubcategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unit;
        public SubcategoryController(IUnitOfWork unit)
        {
            _unit = unit;
        }
        // GET: api/Subcategory
        [HttpGet("subcategories")]
        public async Task<IEnumerable<SubcategoryViewModel>> Get()
        {
            return (await _unit.ISubcategory.GetJoinedData()).UserObject;
        }
        [HttpGet("subcategorybycategoryid/{id}")]
        public async Task<IEnumerable<SubcategoryViewModel>> GetSubcategoryByCategoryId(Guid Id)
        {
            return (await _unit.ISubcategory.GetSelectedDataAsync(t => t.CategoryId == Id,d => new SubcategoryViewModel {SubcategoryId= d.Id.ToString(), Name = d.Name,Price = d.Price})).UserObject;
        }
        // GET: api/Subcategory/5
        [HttpGet("edit/{id}")]
        public async Task<SubcategoryViewModel> Get(Guid id)
        {
           return (await _unit.ISubcategory.GetSubcategoryById(id: id)).UserObject;
        }

        // POST: api/Subcategory
        [HttpPost("add")]
        public async Task<HttpResponseMessage> Post([FromBody]SubcategoryViewModel value)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isEdit = true;
                    Guid SubcategoryGuid = Guid.Empty;
                    isEdit = !string.IsNullOrEmpty(value.SubcategoryId) ? Guid.TryParse(value.SubcategoryId, out SubcategoryGuid) : false;
                    var isExist = await _unit.ISubcategory.Exists(t => t.Name == value.Name && t.CategoryId == value.CategoryId);
                    if (isExist.UserObject)
                    {
                        var response = new HttpResponseMessage()
                        {
                            StatusCode = HttpStatusCode.Conflict
                        };
                        return response;
                    }
                    else
                    {
                        var data = AutoMapper.Mapper.Map<TblSubcategory>(value);
                        if (!isEdit)
                        {
                            data.Id = string.IsNullOrEmpty(value.SubcategoryId) ? Guid.NewGuid() : SubcategoryGuid;
                            data.CreatedDate = DateTime.Now;
                            data.IsDeleted = false;
                            await _unit.ISubcategory.Add(data);
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
                            data = (await _unit.ISubcategory.GetByID(Id: SubcategoryGuid)).UserObject;
                            data.Name = value.Name;
                            data.Price = value.Price;
                            data.CategoryId = value.CategoryId.Value;
                            data.ModifiedDate = DateTime.Now;
                            data.ModifiedBy = null;
                            _unit.ISubcategory.Update(data);
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
                await _unit.ISubcategory.Remove(id);
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
