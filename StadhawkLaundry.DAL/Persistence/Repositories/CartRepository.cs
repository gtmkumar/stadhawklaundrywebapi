using StadhawkCoreApi;
using StadhawkCoreApi.Logger;
using StadhawkLaundry.BAL.Core.IRepositories;
using StadhawkLaundry.DataModel;
using StadhawkLaundry.DataModel.Models;
using StadhawkLaundry.ViewModel.RequestModel;
using StadhawkLaundry.ViewModel.ResponseModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Data;

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

                return new ApiResult<bool>(new ApiResultCode(ApiResultType.Success), true);
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.ApplicationTier, ex);
            }
            return new ApiResult<bool>(new ApiResultCode(ApiResultType.Success), false);
        }

        public async Task<ApiResult<CartCountResponseViewModel>> CartCountAndPrice(AddCartRequestViewModel customerAddToCart, int userId, string databseCon)
        {
            CartCountResponseViewModel model = new CartCountResponseViewModel();
            try
            {
                SqlParameter StoreItemId = new SqlParameter("@StoreItemId", System.Data.SqlDbType.Int) { Value = customerAddToCart.StoreItemId };
                SqlParameter UserId = new SqlParameter("@UserId", System.Data.SqlDbType.Int) { Value = userId };
                var result = _context.ExecuteStoreProcedure(databseCon, "usp_GetCartItemCountAndPrice", UserId, StoreItemId);
                if (result.Tables[0].Rows.Count > 0)
                {
                    model = (from DataRow dr in result.Tables[0].Rows
                             select new CartCountResponseViewModel()
                             {
                                 CartCount = (dr["CartCount"] != DBNull.Value) ? Convert.ToInt32(dr["CartCount"]) : 0,
                                 CartPrice = (dr["CartPrice"] != DBNull.Value) ? Convert.ToInt32(dr["CartPrice"]) : 0,
                                 CartId = (dr["CartId"] != DBNull.Value) ? Convert.ToInt32(dr["CartId"]) : 0
                             }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.ApplicationTier, ex);
                return new ApiResult<CartCountResponseViewModel>(new ApiResultCode(ApiResultType.Error, 0, "No data in given request"));
            }
            return new ApiResult<CartCountResponseViewModel>(new ApiResultCode(ApiResultType.Success), model);
        }

        public async Task<ApiResult<CartDetailResponseViewModel>> GetCartDetail(int userId, int addressId)
        {
            CartDetailResponseViewModel model = null;
            try
            {
                SqlParameter UserId = new SqlParameter("@userId", System.Data.SqlDbType.Int) { Value = userId };
                SqlParameter AddressId = new SqlParameter("@AddressId", System.Data.SqlDbType.Int) { Value = addressId };
                var result = _context.ExecuteStoreProcedure("GetCartDetail", UserId, AddressId);
                if (result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
                {
                    foreach (System.Data.DataRow row in result.Tables[0].Rows)
                    {
                        model = new CartDetailResponseViewModel()
                        {
                            ServiceId = (row["ServiceId"] != DBNull.Value) ? Convert.ToInt32(row["ServiceId"]) : 0,
                            ServiceName = (row["ServiceName"] != DBNull.Value) ? Convert.ToString(row["ServiceName"]) : string.Empty
                        };
                        if (result.Tables.Count > 0 && result.Tables[1].Rows.Count > 0)
                        {
                            foreach (System.Data.DataRow catrow in result.Tables[1].Rows)
                            {
                                if (((row["ServiceId"] != DBNull.Value) ? Convert.ToInt32(catrow["ServiceId"]) : 0) == ((catrow["ServiceId"] != DBNull.Value) ? Convert.ToInt32(catrow["ServiceId"]) : 0))
                                {
                                    model.CategoryData.Add(new CartCategoryResponceViewModel
                                    {
                                        CategoryId = (catrow["categoryId"] != DBNull.Value) ? Convert.ToInt32(catrow["categoryId"]) : 0,
                                        CategoryName = (catrow["CategoryName"] != DBNull.Value) ? Convert.ToString(catrow["CategoryName"]) : string.Empty
                                    });
                                }
                                if (result.Tables.Count > 0 && result.Tables[2].Rows.Count > 0)
                                {
                                    foreach (System.Data.DataRow itmrow in result.Tables[2].Rows)
                                    {
                                        if (((catrow["ServiceId"] != DBNull.Value) ? Convert.ToInt32(catrow["ServiceId"]) : 0) == ((catrow["itmrow"] != DBNull.Value) ? Convert.ToInt32(itmrow["ServiceId"]) : 0))
                                        {
                                            model.CategoryData.Add(new CartCategoryResponceViewModel
                                            {
                                                CategoryId = (catrow["categoryId"] != DBNull.Value) ? Convert.ToInt32(catrow["categoryId"]) : 0,
                                                CategoryName = (catrow["CategoryName"] != DBNull.Value) ? Convert.ToString(catrow["CategoryName"]) : string.Empty
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.ApplicationTier, ex);
                return new ApiResult<CartDetailResponseViewModel>(new ApiResultCode(ApiResultType.Error, 0, "No data in given request"));
            }
            return new ApiResult<CartDetailResponseViewModel>(new ApiResultCode(ApiResultType.Success), model);
        }
    }
}
