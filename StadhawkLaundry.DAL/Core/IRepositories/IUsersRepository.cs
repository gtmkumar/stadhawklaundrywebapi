using StadhawkCoreApi;
using StadhawkLaundry.DataModel.Models;
using StadhawkLaundry.ViewModel.ResponseModel;
using System.Threading.Tasks;

namespace StadhawkLaundry.BAL.Core.IRepositories
{
    public interface IUsersRepository : IRepository<AspNetUsers>
    {
        Task<ApiResult<bool>> AuthenticateUsers(string username, string password);
        Task<ApiResult<LoginResponseViewModel>> GetUserDetailsbyCredentials(string username);
        Task<ApiResult<LoginResponseViewModel>> GetDataFromPhoneNo(string phoneNo);
    }
}
