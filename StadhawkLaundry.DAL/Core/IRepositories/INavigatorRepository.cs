using StadhawkCoreApi;
using StadhawkLaundry.DataModel.Models;
using StadhawkLaundry.ViewModel.ResponseModel;
using System;

namespace StadhawkLaundry.BAL.Core.IRepositories
{
    public interface INavigatorRepository : IRepository<NavigatorView>
    {
        ApiResult<NavigatorResponseModel> GetNavigation(int RoleID, string URL);
        //ApiResultCode AddMenuNavigation(string empCode, int EmpId, string roleId, int userId, bool isEdit);
        //ApiResult<bool> IsAuthorize(string id, string URL);
        //ApiResult<string> GetDefaultUrl(Guid RoleID, int selection = 0);
        //ApiResult<NavigatorResponseModel> GetMenuNavigation(Guid RoleID, string employeeCode, string empCode);
    }
}
