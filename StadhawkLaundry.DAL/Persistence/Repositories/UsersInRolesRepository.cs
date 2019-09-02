using StadhawkCoreApi;
using StadhawkLaundry.BAL.Core.IRepositories;
using StadhawkLaundry.DataModel;
using StadhawkLaundry.DataModel.Models;
using StadhawkLaundry.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StadhawkCoreApi.Logger;

namespace StadhawkLaundry.BAL.Persistence.Repositories
{
    public class UsersInRolesRepository : Repository<AspNetUserRoles>, IUsersInRolesRepository
    {
        private readonly LaundryContext _context;
        public UsersInRolesRepository(LaundryContext context) : base(context)
        {
            _context = context;
        }

        public Task<ApiResult<bool>> AssignRole(UsersInRoles usersInRoles)
        {
            
            throw new NotImplementedException();
        }

        public Task<ApiResult<bool>> CheckRoleExists(UsersInRoles usersInRoles)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<IEnumerable<AssignRolesViewModel>>> GetAssignRoles()
        {
            try
            {
                var result = await (from usertb in _context.AspNetUserRoles
                                    join role in _context.AspNetRoles on usertb.RoleId equals role.Id
                                    join user in _context.AspNetUsers on usertb.UserId equals user.Id
                                    select new AssignRolesViewModel()
                                    {
                                        //RoleName = role.ne,
                                        //RoleId = usertb.RoleId,
                                        //UserName = role.RoleName,
                                        //UserId = usertb.UserId,
                                        //UserRolesId = usertb.UserRolesId
                                    }).ToListAsync();

                return result == null
                            ? new ApiResult<IEnumerable<AssignRolesViewModel>>(new ApiResultCode(ApiResultType.Error, 1, "No data in given request"))
                            : new ApiResult<IEnumerable<AssignRolesViewModel>>(new ApiResultCode(ApiResultType.Success), result);
            }
            catch (Exception ex) {
                ErrorTrace.Logger(LogArea.BusinessTier, ex);
                return new ApiResult<IEnumerable<AssignRolesViewModel>>(new ApiResultCode(ApiResultType.ExceptionDuringOpration, 3, "Please contact system administrator"));
            }

        }

        public Task<ApiResult<bool>> RemoveRole(UsersInRoles usersInRoles)
        {
            throw new NotImplementedException();
        }
    }
}
