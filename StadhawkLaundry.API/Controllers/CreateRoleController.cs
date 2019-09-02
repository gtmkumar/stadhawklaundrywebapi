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
using StadhawkLaundry.DataModel.Models;
using StadhawkLaundry.ViewModel;

namespace StadhawkLaundry.API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CreateRoleController : ControllerBase
    {
        private readonly IUnitOfWork _unit;
        public CreateRoleController(IUnitOfWork unit)
        {
            _unit = unit;
        }
        [HttpGet]
        public async Task<IEnumerable<Roles>> Get()
        {
            try
            {
                return (await _unit.IRole.GetAll()).UserObject;
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.ApplicationTier, ex);
                return null;
            }
        }
        [HttpGet("edit/{id}")]
        public async Task<Roles> Get(Guid id)
        {
            try
            {
                return (await _unit.IRole.GetByID(id)).UserObject;
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.ApplicationTier, ex);
                return null;
            }
        }

        [HttpPost("add")]
        public async Task<HttpResponseMessage> Post([FromBody] RoleViewModel roleViewModel)
        {

            try
            {
                bool isEdit = true;
                Guid roleGuid = Guid.Empty;
                isEdit = !string.IsNullOrEmpty(roleViewModel.RoleId) ? Guid.TryParse(roleViewModel.RoleId, out roleGuid) : false;


                if (ModelState.IsValid)
                {
                    if ((await _unit.IRole.Exists(t => t.RoleName == roleViewModel.RoleName)).UserObject)
                    {
                        var response = new HttpResponseMessage()
                        {
                            StatusCode = HttpStatusCode.Conflict
                        };
                        return response;
                    }
                    else
                    {
                        var data = AutoMapper.Mapper.Map<Roles>(roleViewModel);
                        if (!isEdit)
                        {
                            await _unit.IRole.Add(data);
                            data.CreatedDate = DateTime.Now;
                            data.CreatedBy = null;
                            data.IsDeleted = false;
                            await _unit.Complete();
                            var response = new HttpResponseMessage()
                            {
                                StatusCode = HttpStatusCode.OK
                            };

                            return response;
                        }
                        else
                        {
                            data = (await _unit.IRole.GetByID(Id: roleGuid)).UserObject;
                            data.RoleName = roleViewModel.RoleName;
                            data.ModifiedDate = DateTime.Now;
                            data.ModifiedBy = null;
                            _unit.IRole.Update(data);
                            await _unit.Complete();
                            var response = new HttpResponseMessage()
                            {
                                StatusCode = HttpStatusCode.OK
                            };
                            return response;
                        }
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
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.ApplicationTier, ex);
                var response = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest
                };
                return response;
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<HttpResponseMessage> Delete(Guid id)
        {
            try
            {

                await _unit.IRole.Remove(id);
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
            catch (Exception)
            {
                throw;
            }
        }
    }
}