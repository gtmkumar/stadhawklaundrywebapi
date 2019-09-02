using StadhawkCoreApi;
using StadhawkLaundry.DataModel.Models;
using StadhawkLaundry.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StadhawkLaundry.BAL.Core.IRepositories
{
    public interface IUsersInRolesRepository : IRepository<AspNetUserRoles>
    {
        Task<ApiResult<bool>> AssignRole(UsersInRoles usersInRoles);
        Task<ApiResult<bool>> CheckRoleExists(UsersInRoles usersInRoles);
        Task<ApiResult<bool>> RemoveRole(UsersInRoles usersInRoles);
        Task<ApiResult<IEnumerable<AssignRolesViewModel>>> GetAssignRoles();
    }
}
