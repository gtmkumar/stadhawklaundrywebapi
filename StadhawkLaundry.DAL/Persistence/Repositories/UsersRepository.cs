using StadhawkCoreApi;
using StadhawkLaundry.BAL.Core.IRepositories;
using StadhawkLaundry.DataModel;
using StadhawkLaundry.DataModel.Models;
using StadhawkLaundry.ViewModel;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Globalization;
using StadhawkCoreApi.Logger;
using StadhawkLaundry.ViewModel.ResponseModel;

namespace StadhawkLaundry.BAL.Persistence.Repositories
{
    public class UsersRepository : Repository<AspNetUsers>, IUsersRepository
    {
        private readonly LaundryContext _context;
        public UsersRepository(LaundryContext context):base(context)
        {
            _context = context;
        }

        public async Task<ApiResult<bool>> AuthenticateUsers(string username, string password)
        {
            var result = await _context.AspNetUsers.AnyAsync(t => (t.UserName == username || t.PhoneNumber == username) && t.PasswordHash == password && t.PhoneNumberConfirmed == true);
            return new ApiResult<bool>(new ApiResultCode(ApiResultType.Success), result);
        }

        public Task<ApiResult<LoginResponseViewModel>> GetDataFromPhoneNo(string phoneNo)
        {

            throw new NotImplementedException();
        }

        public async Task<ApiResult<LoginResponseViewModel>> GetUserDetailsbyCredentials(string username)
        {
            try
            {
                var result = await (from user in _context.AspNetUsers
                              join userinrole in _context.AspNetRoles on user.Id equals userinrole.Id
                              where user.UserName == username
                              select new LoginResponseViewModel
                              {
                                  EmailId = user.Email,
                                  MobileNo = user.PhoneNumber,
                                  UserName = user.UserName
                              }).SingleOrDefaultAsync();

                return result == null
                    ? new ApiResult<LoginResponseViewModel>(new ApiResultCode(ApiResultType.Error, 1, "No data in given request"))
                    : new ApiResult<LoginResponseViewModel>(new ApiResultCode(ApiResultType.Success), result); ;
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.BusinessTier, ex);
                return new ApiResult<LoginResponseViewModel>(new ApiResultCode(ApiResultType.ExceptionDuringOpration, 3, "Please contact system administrator"));
            }
        }
    }
}
