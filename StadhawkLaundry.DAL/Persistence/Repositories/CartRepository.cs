using StadhawkCoreApi;
using StadhawkCoreApi.Logger;
using StadhawkLaundry.BAL.Core.IRepositories;
using StadhawkLaundry.DataModel;
using StadhawkLaundry.DataModel.Models;
using StadhawkLaundry.ViewModel.RequestModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace StadhawkLaundry.BAL.Persistence.Repositories
{
    public class CartRepository : Repository<TblCart>, ICartRepository
    {
        private readonly LaundryContext _context;
        public CartRepository(LaundryContext context) : base(context)
        {
            _context = context;
        }
        public async Task<ApiResult<bool>> AddToCartAsync(AddCartRequestViewModel customerAddToCart, int userId)
        {
            try
            {
                customerAddToCart.Quantity = 1;
                SqlParameter CartId = new SqlParameter("@CartId", System.Data.SqlDbType.Int) { Value = customerAddToCart.CartId.HasValue ? customerAddToCart.CartId.Value : (object)DBNull.Value };
                SqlParameter Itemid = new SqlParameter("@StoreItemId", System.Data.SqlDbType.Int) { Value = customerAddToCart.StoreItemId };
                SqlParameter quantity = new SqlParameter("@Quantity", System.Data.SqlDbType.Int) { Value = customerAddToCart.Quantity.HasValue ? customerAddToCart.Quantity.Value : (object)DBNull.Value };
                SqlParameter IsCartRemoved = new SqlParameter("@IsRemove", System.Data.SqlDbType.Bit) { Value = customerAddToCart.IsRemove };
                SqlParameter AddressId = new SqlParameter("@AddressId", System.Data.SqlDbType.Int) { Value = customerAddToCart.AddressId };
                SqlParameter UserId = new SqlParameter("@UserId", System.Data.SqlDbType.Int) { Value = userId };
                var result = _context.ExecuteStoreProcedure("usp_SaveCartItemDetail", CartId, Itemid, quantity, quantity, IsCartRemoved, AddressId, UserId);

                return new ApiResult<bool>(new ApiResultCode(ApiResultType.Success), true)
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.ApplicationTier, ex);
            }
            return new ApiResult<bool>(new ApiResultCode(ApiResultType.Success), false);
        }
    }
}
