using Microsoft.EntityFrameworkCore;
using StadhawkCoreApi;
using StadhawkCoreApi.Logger;
using StadhawkLaundry.BAL.Core.IRepositories;
using StadhawkLaundry.DataModel;
using StadhawkLaundry.DataModel.Models;
using StadhawkLaundry.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using StadhawkLaundry.ViewModel.ResponseModel;
using StadhawkLaundry.ViewModel.RequestModel;
using System.Data.SqlClient;

namespace StadhawkLaundry.BAL.Persistence.Repositories
{
    public class ItemRepository : Repository<TblItemMaster>, IItemRepository
    {
        private readonly LaundryContext _context;
        public ItemRepository(LaundryContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ApiResult<ItemViewModel>> GetItemById(int Id)
        {
            try
            {
                var result = await (from itm in _context.TblItemMaster
                                    join subcat in _context.TblSubServiceMaster on itm.Id equals subcat.Id
                                    join cat in _context.TblCategoryMaster on subcat.Id equals cat.Id
                                    join ser in _context.TblServiceMaster on cat.Id equals ser.Id
                                    select new ItemViewModel
                                    {
                                        Name = itm.ItemName,
                                        ServiceId = ser.Id,
                                        CategoryId = cat.Id,
                                        SubcategoryId = itm.Id,
                                        ItemId = itm.Id,
                                        Price = Convert.ToDecimal(ser.Id),
                                    }
                                ).Where(t => t.ItemId == Id).FirstOrDefaultAsync();

                return result == null
                        ? new ApiResult<ItemViewModel>(new ApiResultCode(ApiResultType.Error, 1, "No data in given request"))
                        : new ApiResult<ItemViewModel>(new ApiResultCode(ApiResultType.Success), result);
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.BusinessTier, ex);
                return new ApiResult<ItemViewModel>(new ApiResultCode(ApiResultType.ExceptionDuringOpration, 3, "Please contact system administrator"));
            }

        }

        public async Task<ApiResult<IEnumerable<ItemViewModel>>> GetALLltem()
        {
            try
            {
                var result = await (from itm in _context.TblItemMaster
                                    join subcat in _context.TblSubServiceMaster on itm.Id equals subcat.Id
                                    join cat in _context.TblCategoryMaster on subcat.Id equals cat.Id
                                    join ser in _context.TblServiceMaster on cat.Id equals ser.Id
                                    select new ItemViewModel
                                    {
                                        Name = itm.ItemName,
                                        ServiceId = ser.Id,
                                        CategoryId = cat.Id,
                                        SubcategoryId = itm.Id,
                                        ItemId = itm.Id,
                                        Price = Convert.ToDecimal(itm.Id),
                                        ServiceName = ser.ServiceName,
                                        CategoryName = cat.CategoryName,
                                        SubcategoryName = subcat.SubServiceName
                                    }
                                ).ToListAsync();

                return result == null
                        ? new ApiResult<IEnumerable<ItemViewModel>>(new ApiResultCode(ApiResultType.Error, 1, "No data in given request"))
                        : new ApiResult<IEnumerable<ItemViewModel>>(new ApiResultCode(ApiResultType.Success), result);
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.BusinessTier, ex);
                return new ApiResult<IEnumerable<ItemViewModel>>(new ApiResultCode(ApiResultType.ExceptionDuringOpration, 3, "Please contact system administrator"));
            }
        }

        public async Task<ApiResult<IEnumerable<ItemResponseViewModel>>> GetItemByStore(CategoryItemFilterRequest filterRequest)
        {
            List<ItemResponseViewModel> listitems = new List<ItemResponseViewModel>();
            try
            {
                SqlParameter CategoryId = new SqlParameter("@CategoryId", System.Data.SqlDbType.Int) { Value = filterRequest.CategoryId };
                SqlParameter StoreId = new SqlParameter("@StoreId", System.Data.SqlDbType.Int) { Value = filterRequest.StoreId };
                SqlParameter ServiceId = new SqlParameter("@ServiceId", System.Data.SqlDbType.Int) { Value = filterRequest.ServiceId };
                SqlParameter Userid = new SqlParameter("@Userid", System.Data.SqlDbType.Int) { Value = filterRequest.UserId };
                var ietms = _context.ExecuteStoreProcedure("[Usp_GetItemByStore]", ServiceId,CategoryId, StoreId, Userid);

                if (ietms.Tables[0].Rows.Count > 0)
                {
                    listitems = (from DataRow dr in ietms.Tables[0].Rows
                                 select new ItemResponseViewModel()
                                 {
                                     ServiceId = (dr["ServiceId"] != DBNull.Value) ? Convert.ToInt32(dr["ServiceId"]) : 0,
                                     StoreId = (dr["StoreId"] != DBNull.Value) ? Convert.ToInt32(dr["StoreId"]) : 0,
                                     CategoryId = (dr["CategoryId"] != DBNull.Value) ? Convert.ToInt32(dr["CategoryId"]) : 0,
                                     StoreItemId=(dr["StoreItemId"] != DBNull.Value) ? Convert.ToInt32(dr["StoreItemId"]) : 0,
                                     Name = (dr["ItemName"] != DBNull.Value) ? Convert.ToString(dr["ItemName"]) : string.Empty,
                                     Image = (dr["Image"] != DBNull.Value) ? Convert.ToString(dr["Image"]) : string.Empty,
                                     CartId = (dr["CartId"] != DBNull.Value) ? Convert.ToInt32(dr["CartId"]) : 0,
                                     Price = (dr["Price"] != DBNull.Value) ? Convert.ToDecimal(dr["Price"]) : 0,
                                     Quantity = (dr["CartCount"] != DBNull.Value) ? Convert.ToInt32(dr["CartCount"]) : 0,
                                     IsKg = (dr["IsKg"] != DBNull.Value) ? Convert.ToBoolean(dr["IsKg"]) : false,
                                 }).ToList();
                }
                return listitems.Count < 0
                            ? new ApiResult<IEnumerable<ItemResponseViewModel>>(new ApiResultCode(ApiResultType.Error, 1, "No data in given request"))
                            : new ApiResult<IEnumerable<ItemResponseViewModel>>(new ApiResultCode(ApiResultType.Success), listitems);
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.BusinessTier, ex);
                return new ApiResult<IEnumerable<ItemResponseViewModel>>(new ApiResultCode(ApiResultType.ExceptionDuringOpration, 3, "Please contact system administrator"));
            }
        }
    }
}
