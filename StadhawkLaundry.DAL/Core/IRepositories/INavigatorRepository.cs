using StadhawkCoreApi;
using StadhawkLaundry.ViewModel.ResponseModel;
using System;

namespace StadhawkLaundry.BAL.Core.IRepositories
{
    public interface INavigatorRepository
    {
        ApiResult<bool> IsAuthorize(string strEmpCode, string URL);
        ApiResult<string> GetDefaultUrl(Guid RoleID, int selection = 0);
        ApiResult<NavigatorResponseModel> GetMenuNavigation(Guid RoleID, string employeeCode, string empCode);
    }
}
