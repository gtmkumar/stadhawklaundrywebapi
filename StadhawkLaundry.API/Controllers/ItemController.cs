using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StadhawkCoreApi;
using StadhawkCoreApi.Logger;
using StadhawkLaundry.BAL.Core;
using StadhawkLaundry.DataModel.Models;
using StadhawkLaundry.ViewModel;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace StadhawkLaundry.API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IUnitOfWork _unit;
        public ItemController(IUnitOfWork unit)
        {
            _unit = unit;
        }
        // GET: api/Item
        [HttpGet("items")]
        public async Task<IEnumerable<ItemViewModel>> Get()
        {
            return (await _unit.IItem.GetALLltem()).UserObject;
        }

        // GET: api/Item/5
        [HttpGet("edit/{id}")]
        public async Task<ItemViewModel> Get(string id)
        {
            return (await _unit.IItem.GetItemById(Id: id)).UserObject;
        }

        [HttpGet("itembysubcategoryid/{id}")]
        public async Task<IEnumerable<ItemViewModel>> GetItemsBySubcategoryId(Guid Id)
        {
            var result = (await _unit.IItem.GetSelectedDataAsync(t => t.ServiceId == Id, d => new ItemViewModel { ItemId = d.Id.ToString(), Name = d.Name,Price=Convert.ToDecimal(d.Id) })).UserObject;
            return result;

        }

        // POST: api/Item
        [HttpPost("add")]
        public async Task<HttpResponseMessage> Post([FromBody]ItemViewModel value)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isEdit = true;
                    Guid ItemGuid = Guid.Empty;
                    isEdit = !string.IsNullOrEmpty(value.ItemId) ? Guid.TryParse(value.ItemId, out ItemGuid) : false;
                    ApiResult<bool> isExist = await _unit.IItem.Exists(t => t.Name == value.Name && value.SubcategoryId == value.SubcategoryId);
                    if (isExist.UserObject)
                    {
                        HttpResponseMessage response = new HttpResponseMessage()
                        {
                            StatusCode = HttpStatusCode.Conflict
                        };
                        return response;
                    }
                    else
                    {
                        TblItem data = AutoMapper.Mapper.Map<TblItem>(value);
                        if (!isEdit)
                        {
                            data.Id = string.IsNullOrEmpty(value.ItemId) ? Guid.NewGuid() : ItemGuid;
                            data.CreatedDate = DateTime.Now;
                            data.IsDeleted = false;
                            await _unit.IItem.Add(data);
                            ApiResultCode result = await _unit.Complete();
                            if (result.ResultType == ApiResultType.Success)
                            {
                                HttpResponseMessage response = new HttpResponseMessage()
                                {
                                    StatusCode = HttpStatusCode.OK
                                };
                                return response;
                            }
                        }
                        else
                        {
                            data = (await _unit.IItem.GetByID(Id: ItemGuid)).UserObject;
                            data.Name = value.Name;
                            data.ModifiedDate = DateTime.Now;
                            data.ModifiedBy = null;
                            _unit.IItem.Update(data);
                            ApiResultCode result = await _unit.Complete();
                            if (result.ResultType == ApiResultType.Success)
                            {
                                HttpResponseMessage response = new HttpResponseMessage()
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
                    HttpResponseMessage response = new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.ApplicationTier, ex);
                HttpResponseMessage response = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest
                };
                return response;
            }
            HttpResponseMessage response1 = new HttpResponseMessage()
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
                await _unit.IItem.Remove(id);
                ApiResultCode result = await _unit.Complete();

                if (result.ResultType == ApiResultType.Success)
                {
                    HttpResponseMessage response = new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.OK
                    };
                    return response;
                }
                else
                {
                    HttpResponseMessage response = new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.BadRequest
                    };
                    return response;
                }

            }
            else
            {
                HttpResponseMessage response = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest
                };
                return response;
            }
        }
    }
}
