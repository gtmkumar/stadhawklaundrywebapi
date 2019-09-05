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
using StadhawkLaundry.ViewModel.RequestModel;
using System.Data;
using System.Data.SqlClient;

namespace StadhawkLaundry.BAL.Persistence.Repositories
{
    public class UserAddressRepository : Repository<TblUserAddress>, IUserAddressRepository
    {

        private readonly LaundryContext _context;
        public UserAddressRepository(LaundryContext context) : base(context)
        {
            _context = context;
        }
        public async Task<ApiResult<bool>> SaveUserAddress(UserAddressRequestViewModel userAddress, string userId)
        {
            try
            {
                userAddress.IsDefaultDeliveryLocation = true;
                SqlParameter AddressId = new SqlParameter("@AddressId", System.Data.SqlDbType.Int) { Value = userAddress.AddressId.HasValue ? userAddress.AddressId.Value : (object)DBNull.Value };
                SqlParameter Userid = new SqlParameter("@Userid", System.Data.SqlDbType.VarChar) { Value = !string.IsNullOrEmpty(userId) ? userId : (object)DBNull.Value };
                SqlParameter Longitude = new SqlParameter("@Longitude", System.Data.SqlDbType.Decimal) { Value = userAddress.Longitude };
                SqlParameter Latitude = new SqlParameter("@Latitude", System.Data.SqlDbType.Decimal) { Value = userAddress.Latitude };
                SqlParameter Address1 = new SqlParameter("@Address1", System.Data.SqlDbType.VarChar) { Value = userAddress.Address1 ?? (object)DBNull.Value };
                SqlParameter Address2 = new SqlParameter("@Address2", System.Data.SqlDbType.VarChar) { Value = userAddress.Address2 ?? (object)DBNull.Value };
                SqlParameter LandMark = new SqlParameter("@LandMark", System.Data.SqlDbType.VarChar) { Value = userAddress.LandMark ?? (object)DBNull.Value };
                SqlParameter AddressTypeId = new SqlParameter("@AddressTypeId", System.Data.SqlDbType.Int) { Value = userAddress.AddressTypeId };
                SqlParameter IsDefaultDeliveryLocation = new SqlParameter("@IsDefaultDeliveryLocation", System.Data.SqlDbType.Bit) { Value = userAddress.IsDefaultDeliveryLocation };
                int result = await _context.Database.ExecuteSqlCommandAsync("usp_SaveUserAddress {0},{1},{2},{3},{4},{5},{6},{7},{8}", AddressId, Userid, Longitude, Latitude, Address1, Address2, LandMark, AddressTypeId, IsDefaultDeliveryLocation);

                if (result > 0)
                {
                    return new ApiResult<bool>(new ApiResultCode(ApiResultType.Success), true);
                }
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.ApplicationTier, ex);
            }
            return new ApiResult<bool>(new ApiResultCode(ApiResultType.Success), false);
        }

        //public Task<ApiResult<bool>> UpdateUserAddress(UserAddressRequestViewModel userAddress, int? userId)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<ApiResult<bool>> UpdateUserAddress(UserAddressRequestViewModel userAddress, string userId)
        {
            try
            {
                SqlParameter AddressId = new SqlParameter("@AddressId", System.Data.SqlDbType.Int) { Value = userAddress.AddressId.HasValue ? userAddress.AddressId.Value : (object)DBNull.Value };
                //SqlParameter Userid = new SqlParameter("@Userid", System.Data.SqlDbType.Int) { Value = userId.HasValue ? userId.Value : (object)DBNull.Value };
                SqlParameter Userid = new SqlParameter("@Userid", System.Data.SqlDbType.VarChar) { Value = !string.IsNullOrEmpty(userId) ? userId : (object)DBNull.Value };
                SqlParameter Address2 = new SqlParameter("@Address2", System.Data.SqlDbType.VarChar) { Value = userAddress.Address2 };
                SqlParameter LandMark = new SqlParameter("@LandMark", System.Data.SqlDbType.VarChar) { Value = userAddress.LandMark ?? (object)DBNull.Value };
                // var result = _context.ExecuteStoreProcedure("usp_UpdateUserAddress", AddressId, Userid, Address2, LandMark);
                int result = await _context.Database.ExecuteSqlCommandAsync("usp_UpdateUserAddress {0},{1},{2},{3}", AddressId, Userid, Address2, LandMark);
                ///  if (result >0)
                return new ApiResult<bool>(new ApiResultCode(ApiResultType.Success), true);

                // return new ApiResult<bool>(new ApiResultCode(ApiResultType.Success), true);
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.ApplicationTier, ex);
                return new ApiResult<bool>(new ApiResultCode(ApiResultType.Success), false);
            }
        }

        //public Task<ApiResult<IEnumerable<UserAddressResponseViewModel>>> UserAddress(int? UserId, int? addressId = null)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<ApiResult<IEnumerable<UserAddressResponseViewModel>>> UserAddress(string UserId, int? addressId = null)
        {
            List<UserAddressResponseViewModel> models = new List<UserAddressResponseViewModel>();
            //SqlParameter CustomerId = new SqlParameter("@User_id", System.Data.SqlDbType.Int) { Value = UserId };
            SqlParameter Userid = new SqlParameter("@Userid", System.Data.SqlDbType.VarChar) { Value = !string.IsNullOrEmpty(UserId) ? UserId : (object)DBNull.Value };
            SqlParameter addressIdParam = new SqlParameter("@Address_id", System.Data.SqlDbType.Int) { Value = addressId.HasValue ? addressId.Value : (object)DBNull.Value };
            try
            {
                var result = await _context.AddressDataModels.FromSql("[GetUserAddress] {0},{1}", Userid, addressIdParam).ToListAsync();
                foreach (var item in result)
                {
                    models.Add(new UserAddressResponseViewModel
                    {
                        AddressId = item.AddressId,
                        Latitude = item.Latitude,
                        Longitude = item.Longitude,
                        Address1 = item.Address1,
                        Address2 = item.Address2,
                        LandMark = item.LandMark,
                        AddressTypeId = item.AddressTypeId
                    });
                }


                if (result != null)
                    return new ApiResult<IEnumerable<UserAddressResponseViewModel>>(new ApiResultCode(ApiResultType.Success), models);
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.ApplicationTier, ex);
            }
            return new ApiResult<IEnumerable<UserAddressResponseViewModel>>(new ApiResultCode(ApiResultType.Error), models);
        }

    }
}

