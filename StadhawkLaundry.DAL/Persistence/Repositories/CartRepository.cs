﻿using StadhawkCoreApi;
using StadhawkCoreApi.Logger;
using StadhawkLaundry.BAL.Core.IRepositories;
using StadhawkLaundry.DataModel;
using StadhawkLaundry.DataModel.Models;
using StadhawkLaundry.ViewModel.RequestModel;
using StadhawkLaundry.ViewModel.ResponseModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
            bool iSFromDiffrentService = false;
            try
            {
                SqlParameter CartId = new SqlParameter("@CartId", System.Data.SqlDbType.Int) { Value = customerAddToCart.CartId.HasValue ? customerAddToCart.CartId.Value : 0 };
                SqlParameter StoreId = new SqlParameter("@StoreId", System.Data.SqlDbType.Int) { Value = customerAddToCart.StoreId };
                SqlParameter ServiceId = new SqlParameter("@ServiceId", System.Data.SqlDbType.Int) { Value = customerAddToCart.ServiceId };
                SqlParameter StoreItemId = new SqlParameter("@StoreItemId", System.Data.SqlDbType.Int) { Value = customerAddToCart.StoreItemId };
                SqlParameter IsCartRemoved = new SqlParameter("@IsCartRemoved", System.Data.SqlDbType.Bit) { Value = customerAddToCart.IsCartRemoved };
                SqlParameter AddressId = new SqlParameter("@AddressId", System.Data.SqlDbType.Int) { Value = customerAddToCart.AddressId };
                SqlParameter UserId = new SqlParameter("@UserId", System.Data.SqlDbType.Int) { Value = userId };
                SqlParameter Quantity = new SqlParameter("@Quantity", System.Data.SqlDbType.Int) { Value = customerAddToCart.Quantity ?? (object)DBNull.Value };
                var result = _context.ExecuteStoreProcedure("dbo.usp_AddCart", CartId, StoreId, ServiceId, StoreItemId, Quantity, IsCartRemoved, AddressId, UserId);
                if (result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
                {
                    foreach (System.Data.DataRow row in result.Tables[0].Rows)
                    {
                        if (((row["ErrorMessage"] != DBNull.Value) ? Convert.ToString(row["ErrorMessage"]) : string.Empty) == "1")
                        {
                            iSFromDiffrentService = true;
                        }
                    }
                }
                return new ApiResult<bool>(new ApiResultCode(ApiResultType.Success), iSFromDiffrentService);
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.ApplicationTier, ex);
            }
            return new ApiResult<bool>(new ApiResultCode(ApiResultType.Success), false);
        }

        public async Task<ApiResult<CartCountResponseViewModel>> CartCountAndPrice(int userId)
        {
            CartCountResponseViewModel model = new CartCountResponseViewModel();
            try
            {
                SqlParameter UserId = new SqlParameter("@UserId", System.Data.SqlDbType.Int) { Value = userId };
                var result = _context.ExecuteStoreProcedure("dbo.usp_GetCartItemCountAndPrice", UserId);
                if (result.Tables[0].Rows.Count > 0)
                {
                    model = (from DataRow dr in result.Tables[0].Rows
                             select new CartCountResponseViewModel()
                             {
                                 CartCount = (dr["CartCount"] != DBNull.Value) ? Convert.ToInt32(dr["CartCount"]) : 0,
                                 CartPrice = (dr["CartPrice"] != DBNull.Value) ? Convert.ToDecimal(dr["CartPrice"]) : 0,
                                 KgCount = (dr["KgCount"] != DBNull.Value) ? Convert.ToDecimal(dr["KgCount"]) : 0,
                                 CartId = (dr["CartId"] != DBNull.Value) ? Convert.ToInt32(dr["CartId"]) : 0,
                                 IsKg = (dr["IsKg"] != DBNull.Value) ? Convert.ToBoolean(dr["IsKg"]) : false
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

        public async Task<ApiResult<CartPriceDetail>> GetCartDetail(int userId, int addressId)
        {
            CartPriceDetail priceDetail = null;
            CartDetailResponseViewModel model = null;
            CartCategoryResponceViewModel categoryModel = null;
            ServiceByKgResponseViewModel ServiceByKg = null;
            try
            {
                SqlParameter UserId = new SqlParameter("@userId", System.Data.SqlDbType.Int) { Value = userId };
                SqlParameter AddressId = new SqlParameter("@AddressId", System.Data.SqlDbType.Int) { Value = addressId };
                var result = _context.ExecuteStoreProcedure("dbo.usp_GetCartDetail", UserId, AddressId);
                if (result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
                {
                    foreach (System.Data.DataRow row in result.Tables[0].Rows)
                    {
                        priceDetail = new CartPriceDetail
                        {
                            CartCount = (row["CartCount"] != DBNull.Value) ? Convert.ToInt32(row["CartCount"]) : 0,
                            CartPrice = (row["CartPrice"] != DBNull.Value) ? Convert.ToDecimal(row["CartPrice"]) : 0,
                            KgCount = (row["KgCount"] != DBNull.Value) ? Convert.ToDecimal(row["KgCount"]) : 0,
                            IsKg = (row["IsKg"] != DBNull.Value) ? Convert.ToBoolean(row["IsKg"]) : false,
                            TaxAmount = (row["TaxAmount"] != DBNull.Value) ? Convert.ToDecimal(row["TaxAmount"]) : 0,
                            TotalPrice = (row["TotalAmout"] != DBNull.Value) ? Convert.ToDecimal(row["TotalAmout"]) : 0,
                            IsValidShipment = (row["IsValidShipment"] != DBNull.Value) ? Convert.ToBoolean(row["IsValidShipment"]) : false
                        };
                    }
                    foreach (System.Data.DataRow row in result.Tables[1].Rows)
                    {
                        model = new CartDetailResponseViewModel()
                        {
                            StoreId = (row["StoreId"] != DBNull.Value) ? Convert.ToInt32(row["StoreId"]) : 0,
                            ServiceId = (row["ServiceId"] != DBNull.Value) ? Convert.ToInt32(row["ServiceId"]) : 0,
                            ServiceName = (row["ServiceName"] != DBNull.Value) ? Convert.ToString(row["ServiceName"]) : string.Empty,
                            ServiceUrl = (row["ServiceImage"] != DBNull.Value) ? Convert.ToString(row["ServiceImage"]) : string.Empty,
                        };
                        priceDetail.ServiceData.Add(model);
                        if (result.Tables.Count > 0 && result.Tables[2].Rows.Count > 0)
                        {
                            foreach (System.Data.DataRow catrow in result.Tables[2].Rows)
                            {
                                if (((row["ServiceId"] != DBNull.Value) ? Convert.ToInt32(row["ServiceId"]) : 0) == ((catrow["ServiceId"] != DBNull.Value) ? Convert.ToInt32(catrow["ServiceId"]) : 0))
                                {
                                    categoryModel = new CartCategoryResponceViewModel
                                    {
                                        CategoryId = (catrow["categoryId"] != DBNull.Value) ? Convert.ToInt32(catrow["categoryId"]) : 0,
                                        CategoryName = (catrow["CategoryName"] != DBNull.Value) ? Convert.ToString(catrow["CategoryName"]) : string.Empty
                                    };
                                    model.CategoryData.Add(categoryModel);
                                    if (result.Tables.Count > 0 && result.Tables[3].Rows.Count > 0)
                                    {
                                        foreach (System.Data.DataRow itmrow in result.Tables[3].Rows)
                                        {
                                            if ((Convert.ToInt32(catrow["ServiceId"]) == Convert.ToInt32(itmrow["ServiceId"])) && Convert.ToInt32(catrow["categoryId"]) == Convert.ToInt32(itmrow["categoryId"]))
                                            {
                                                categoryModel.ItemsData.Add(new CartItemResponseDetailViewModel
                                                {
                                                    CartId = (itmrow["CartId"] != DBNull.Value) ? Convert.ToInt32(itmrow["CartId"]) : 0,
                                                    ItemId = (itmrow["ItemId"] != DBNull.Value) ? Convert.ToInt32(itmrow["ItemId"]) : 0,
                                                    ItemName = (itmrow["ItemName"] != DBNull.Value) ? Convert.ToString(itmrow["ItemName"]) : string.Empty,
                                                    Quantity = (itmrow["Quantity"] != DBNull.Value) ? Convert.ToInt32(itmrow["Quantity"]) : 0,
                                                    TotalPrice = (itmrow["totalprice"] != DBNull.Value) ? Convert.ToDecimal(itmrow["totalprice"]) : 0,
                                                    UnitPrice = (itmrow["Price"] != DBNull.Value) ? Convert.ToDecimal(itmrow["Price"]) : 0
                                                });
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (result.Tables.Count > 0 && result.Tables[4].Rows.Count > 0)
                    {
                        foreach (System.Data.DataRow kgdata in result.Tables[4].Rows)
                        {
                            ServiceByKg = new ServiceByKgResponseViewModel()
                            {
                                StoreId = (kgdata["StoreId"] != DBNull.Value) ? Convert.ToInt32(kgdata["StoreId"]) : 0,
                                ServiceId = (kgdata["ServiceId"] != DBNull.Value) ? Convert.ToInt32(kgdata["ServiceId"]) : 0,
                                ServiceName = (kgdata["ServiceName"] != DBNull.Value) ? Convert.ToString(kgdata["ServiceName"]) : string.Empty,
                                pricePerKG = (kgdata["pricePerKG"] != DBNull.Value) ? Convert.ToDecimal(kgdata["pricePerKG"]) : 0,
                                QuantityInKG = (kgdata["QuantityInKG"] != DBNull.Value) ? Convert.ToDecimal(kgdata["QuantityInKG"]) : 0,
                                ServiceImageUrl = (kgdata["ServiceImageUrl"] != DBNull.Value) ? Convert.ToString(kgdata["ServiceImageUrl"]) : string.Empty,
                                CartId = (kgdata["CartId"] != DBNull.Value) ? Convert.ToInt32(kgdata["CartId"]) : 0
                            };
                            priceDetail.ServiceByKg.Add(ServiceByKg);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.ApplicationTier, ex);
                return new ApiResult<CartPriceDetail>(new ApiResultCode(ApiResultType.Error, 0, "No data in given request"));
            }
            return new ApiResult<CartPriceDetail>(new ApiResultCode(ApiResultType.Success), priceDetail);
        }

        public async Task<ApiResult<CartOrderDetailResponseViewModel>> GetCartDetails(int userId, int addressId)
        {
            CartOrderDetailResponseViewModel priceDetail = null;
            try
            {
                SqlParameter UserId = new SqlParameter("@userId", System.Data.SqlDbType.Int) { Value = userId };
                SqlParameter AddressId = new SqlParameter("@AddressId", System.Data.SqlDbType.Int) { Value = addressId };
                var result = _context.ExecuteStoreProcedure("[usp_GetCarOrdertDetail]", UserId, AddressId);
                if (result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
                {
                    foreach (System.Data.DataRow row in result.Tables[0].Rows)
                    {
                        priceDetail = new CartOrderDetailResponseViewModel
                        {
                            CartCount = (row["CartCount"] != DBNull.Value) ? Convert.ToInt32(row["CartCount"]) : 0,
                            CartPrice = (row["CartPrice"] != DBNull.Value) ? Convert.ToDecimal(row["CartPrice"]) : 0,
                            KgCount = (row["KgCount"] != DBNull.Value) ? Convert.ToDecimal(row["KgCount"]) : 0,
                            IsKg = (row["IsKg"] != DBNull.Value) ? Convert.ToBoolean(row["IsKg"]) : false,
                            TaxAmount = (row["TaxAmount"] != DBNull.Value) ? Convert.ToDecimal(row["TaxAmount"]) : 0,
                            TotalPrice = (row["TotalAmout"] != DBNull.Value) ? Convert.ToDecimal(row["TotalAmout"]) : 0,
                            IsValidShipment = (row["IsValidShipment"] != DBNull.Value) ? Convert.ToBoolean(row["IsValidShipment"]) : false
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.ApplicationTier, ex);
                return new ApiResult<CartOrderDetailResponseViewModel>(new ApiResultCode(ApiResultType.Error, 0, "No data in given request"));
            }
            return new ApiResult<CartOrderDetailResponseViewModel>(new ApiResultCode(ApiResultType.Success), priceDetail);
        }

        public async Task<ApiResult<ExistingCheckResponseViewModel>> IsCartFromDiffrentService(int storeItemId, int userId)
        {
            ExistingCheckResponseViewModel model = new ExistingCheckResponseViewModel();
            try
            {
                var data = await (from c in _context.TblCart
                                  join stritm in _context.TblStoreItems on c.StoreItemId.Value equals stritm.Id
                                  where c.UserId == userId && c.IsDeleted == false
                                  select new { stritm.UnitId }).FirstOrDefaultAsync();

                if (data != null)
                    model.isDifferent = data.UnitId == 1 ? true : false;

            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.ApplicationTier, ex);
                return new ApiResult<ExistingCheckResponseViewModel>(new ApiResultCode(ApiResultType.Error, 0, "No data in given request"));
            }
            return new ApiResult<ExistingCheckResponseViewModel>(new ApiResultCode(ApiResultType.Success), model);
        }

        public async Task<ApiResult<bool>> CartRemove(int CartId)
        {
            bool removed = true;
            try
            {
                SqlParameter cartId = new SqlParameter("@CartId", System.Data.SqlDbType.Int) { Value = CartId };
                var result = _context.ExecuteStoreProcedure("dbo.usp_RemoveCart", cartId);
                return new ApiResult<bool>(new ApiResultCode(ApiResultType.Success), removed);
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.ApplicationTier, ex);
                removed = false;
                return new ApiResult<bool>(new ApiResultCode(ApiResultType.ExceptionDuringOpration), removed);
            }
        }

        public async Task<ApiResult<bool>> AddToServiceCartAsync(AddServiceCartRequestViewModel model, int userId)
        {
            bool iSFromDiffrentService = false;
            try
            {
                SqlParameter CartId = new SqlParameter("@CartId", System.Data.SqlDbType.Int) { Value = model.CartId.HasValue ? model.CartId.Value : 0 };
                SqlParameter StoreId = new SqlParameter("@StoreId", System.Data.SqlDbType.Int) { Value = model.StoreId };
                SqlParameter ServiceId = new SqlParameter("@ServiceId", System.Data.SqlDbType.Int) { Value = model.ServiceId };
                SqlParameter Quantity = new SqlParameter("@Quantity", System.Data.SqlDbType.Decimal) { Value = model.Quantity ?? 0 };
                SqlParameter IsCartRemoved = new SqlParameter("@IsCartRemoved", System.Data.SqlDbType.Bit) { Value = model.IsCartRemoved };
                SqlParameter AddressId = new SqlParameter("@AddressId", System.Data.SqlDbType.Int) { Value = model.AddressId };
                SqlParameter UserId = new SqlParameter("@UserId", System.Data.SqlDbType.Int) { Value = userId };
                var result = _context.ExecuteStoreProcedure("dbo.[usp_AddServiceCart]", CartId, StoreId, ServiceId, Quantity, IsCartRemoved, AddressId, UserId);
                if (result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
                {
                    foreach (System.Data.DataRow row in result.Tables[0].Rows)
                    {
                        if (((row["ErrorMessage"] != DBNull.Value) ? Convert.ToString(row["ErrorMessage"]) : string.Empty) == "1")
                        {
                            iSFromDiffrentService = true;
                        }
                    }
                }
                return new ApiResult<bool>(new ApiResultCode(ApiResultType.Success), iSFromDiffrentService);
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.ApplicationTier, ex);
            }
            return new ApiResult<bool>(new ApiResultCode(ApiResultType.Success), false);
        }
        public async Task<ApiResult<CartServiceCountResponseViewModel>> CartServiceCountAndPrice(int userId)
        {
            CartServiceCountResponseViewModel model = new CartServiceCountResponseViewModel();
            try
            {
                SqlParameter UserId = new SqlParameter("@UserId", System.Data.SqlDbType.Int) { Value = userId };
                var result = _context.ExecuteStoreProcedure("dbo.usp_GetServiceCartItemCountAndPrice", UserId);
                if (result.Tables[0].Rows.Count > 0)
                {
                    model = (from DataRow dr in result.Tables[0].Rows
                             select new CartServiceCountResponseViewModel()
                             {
                                 CartCount = (dr["CartCount"] != DBNull.Value) ? Convert.ToInt32(dr["CartCount"]) : 0,
                                 CartPrice = (dr["CartPrice"] != DBNull.Value) ? Convert.ToDecimal(dr["CartPrice"]) : 0,
                                 CartId = (dr["CartId"] != DBNull.Value) ? Convert.ToInt32(dr["CartId"]) : 0,
                                 IsKg = (dr["IsKg"] != DBNull.Value) ? Convert.ToBoolean(dr["IsKg"]) : false,
                                 KgCount = (dr["CartCount"] != DBNull.Value) ? Convert.ToDecimal(dr["CartCount"]) : 0,
                             }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.ApplicationTier, ex);
                return new ApiResult<CartServiceCountResponseViewModel>(new ApiResultCode(ApiResultType.Error, 0, "No data in given request"));
            }
            return new ApiResult<CartServiceCountResponseViewModel>(new ApiResultCode(ApiResultType.Success), model);
        }

        public async Task<ApiResult<CartPriceDetail>> GetServiceCartDetail(int userId, int addressId)
        {
            CartPriceDetail priceDetail = null;
            CartDetailResponseViewModel model = null;
            CartCategoryResponceViewModel categoryModel = null;
            try
            {
                SqlParameter UserId = new SqlParameter("@userId", System.Data.SqlDbType.Int) { Value = userId };
                SqlParameter AddressId = new SqlParameter("@AddressId", System.Data.SqlDbType.Int) { Value = addressId };
                var result = _context.ExecuteStoreProcedure("dbo.usp_GetCartDetail", UserId, AddressId);
                if (result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
                {
                    foreach (System.Data.DataRow row in result.Tables[0].Rows)
                    {
                        priceDetail = new CartPriceDetail
                        {
                            CartCount = (row["CartCount"] != DBNull.Value) ? Convert.ToInt32(row["CartCount"]) : 0,
                            CartPrice = (row["CartPrice"] != DBNull.Value) ? Convert.ToDecimal(row["CartPrice"]) : 0,
                            IsKg = ((row["CartPrice"] != DBNull.Value) ? Convert.ToInt32(row["CartPrice"]) : 0) > 0 ? false : true,
                            TaxAmount = (row["TaxAmount"] != DBNull.Value) ? Convert.ToDecimal(row["TaxAmount"]) : 0,
                            TotalPrice = (row["TotalAmout"] != DBNull.Value) ? Convert.ToDecimal(row["TotalAmout"]) : 0,
                            IsValidShipment = (row["IsValidShipment"] != DBNull.Value) ? Convert.ToBoolean(row["IsValidShipment"]) : false
                        };
                    }
                    foreach (System.Data.DataRow row in result.Tables[1].Rows)
                    {
                        model = new CartDetailResponseViewModel()
                        {
                            StoreId = (row["StoreId"] != DBNull.Value) ? Convert.ToInt32(row["StoreId"]) : 0,
                            ServiceId = (row["ServiceId"] != DBNull.Value) ? Convert.ToInt32(row["ServiceId"]) : 0,
                            ServiceName = (row["ServiceName"] != DBNull.Value) ? Convert.ToString(row["ServiceName"]) : string.Empty
                        };
                        priceDetail.ServiceData.Add(model);
                        if (result.Tables.Count > 0 && result.Tables[2].Rows.Count > 0)
                        {
                            foreach (System.Data.DataRow catrow in result.Tables[2].Rows)
                            {
                                if (((row["ServiceId"] != DBNull.Value) ? Convert.ToInt32(row["ServiceId"]) : 0) == ((catrow["ServiceId"] != DBNull.Value) ? Convert.ToInt32(catrow["ServiceId"]) : 0))
                                {
                                    categoryModel = new CartCategoryResponceViewModel
                                    {
                                        CategoryId = (catrow["categoryId"] != DBNull.Value) ? Convert.ToInt32(catrow["categoryId"]) : 0,
                                        CategoryName = (catrow["CategoryName"] != DBNull.Value) ? Convert.ToString(catrow["CategoryName"]) : string.Empty
                                    };
                                    model.CategoryData.Add(categoryModel);
                                    if (result.Tables.Count > 0 && result.Tables[3].Rows.Count > 0)
                                    {
                                        foreach (System.Data.DataRow itmrow in result.Tables[3].Rows)
                                        {
                                            if ((Convert.ToInt32(catrow["ServiceId"]) == Convert.ToInt32(itmrow["ServiceId"])) && Convert.ToInt32(catrow["categoryId"]) == Convert.ToInt32(itmrow["categoryId"]))
                                            {
                                                categoryModel.ItemsData.Add(new CartItemResponseDetailViewModel
                                                {
                                                    CartId = (itmrow["CartId"] != DBNull.Value) ? Convert.ToInt32(itmrow["CartId"]) : 0,
                                                    ItemId = (itmrow["ItemId"] != DBNull.Value) ? Convert.ToInt32(itmrow["ItemId"]) : 0,
                                                    ItemName = (itmrow["ItemName"] != DBNull.Value) ? Convert.ToString(itmrow["ItemName"]) : string.Empty,
                                                    Quantity = (itmrow["Quantity"] != DBNull.Value) ? Convert.ToInt32(itmrow["Quantity"]) : 0,
                                                    TotalPrice = (itmrow["totalprice"] != DBNull.Value) ? Convert.ToDecimal(itmrow["totalprice"]) : 0,
                                                    UnitPrice = (itmrow["Price"] != DBNull.Value) ? Convert.ToDecimal(itmrow["Price"]) : 0
                                                });
                                            }
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
                return new ApiResult<CartPriceDetail>(new ApiResultCode(ApiResultType.Error, 0, "No data in given request"));
            }
            return new ApiResult<CartPriceDetail>(new ApiResultCode(ApiResultType.Success), priceDetail);
        }

        public async Task<ApiResult<bool>> CartServiceRemove(int CartId)
        {
            bool removed = true;
            try
            {
                SqlParameter cartId = new SqlParameter("@CartId", System.Data.SqlDbType.Int) { Value = CartId };
                var result = _context.ExecuteStoreProcedure("dbo.[usp_RemoveServiceCart]", cartId);
                return new ApiResult<bool>(new ApiResultCode(ApiResultType.Success), removed);
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.ApplicationTier, ex);
                removed = false;
                return new ApiResult<bool>(new ApiResultCode(ApiResultType.ExceptionDuringOpration), removed);
            }
        }
    }
}
