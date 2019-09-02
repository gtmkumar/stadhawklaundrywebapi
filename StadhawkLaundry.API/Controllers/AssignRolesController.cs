using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StadhawkCoreApi;
using StadhawkCoreApi.Logger;
using StadhawkLaundry.BAL.Core;
using StadhawkLaundry.DataModel.Models;
using StadhawkLaundry.ViewModel;

namespace StadhawkLaundry.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignRolesController : ControllerBase
    {
        private readonly IUnitOfWork _unit;
        public AssignRolesController(IUnitOfWork unit)
        {
            _unit = unit;
        }
        [HttpGet]
        public async Task<IEnumerable<AssignRolesViewModel>> Get()
        {
            try
            {
                return (await _unit.IUsersInRoles.GetAssignRoles()).UserObject;
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.ApplicationTier, ex);
                return null;
            }
        }

        [HttpPost("add")]
        public async Task<HttpResponseMessage> Post([FromBody] UsersInRoles usersInRoleModel)
        {

            try
            {
                bool isEdit = false;
                Guid userRolesGuid = Guid.Empty;
                isEdit = !usersInRoleModel.UserRolesId.Equals(Guid.Empty)  ? true : false;


                if (ModelState.IsValid)
                {
                    if ((await _unit.IUsersInRoles.Exists(t => t.UserId == usersInRoleModel.UserId && t.RoleId == usersInRoleModel.RoleId)).UserObject)
                    {
                        var response = new HttpResponseMessage()
                        {
                            StatusCode = HttpStatusCode.Conflict
                        };
                        return response;
                    }
                    else
                    {
                        var data = AutoMapper.Mapper.Map<UsersInRoles>(usersInRoleModel);
                        if (!isEdit)
                        {
                            await _unit.IUsersInRoles.Add(data);
                            await _unit.Complete();
                            var response = new HttpResponseMessage()
                            {
                                StatusCode = HttpStatusCode.OK
                            };

                            return response;
                        }
                        else
                        {
                            data = (await _unit.IUsersInRoles.GetByID(Id: usersInRoleModel.UserRolesId)).UserObject;
                            _unit.IUsersInRoles.Update(data);
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