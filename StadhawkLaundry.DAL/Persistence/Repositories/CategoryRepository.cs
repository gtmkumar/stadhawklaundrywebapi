using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StadhawkCoreApi;
using StadhawkCoreApi.Logger;
using StadhawkLaundry.BAL.Core.IRepositories;
using StadhawkLaundry.DataModel;
using StadhawkLaundry.DataModel.Models;
using StadhawkLaundry.ViewModel;
using StadhawkLaundry.ViewModel.RequestModel;
using StadhawkLaundry.ViewModel.ResponseModel;

namespace StadhawkLaundry.BAL.Persistence.Repositories
{
    public class CategoryRepository : Repository<TblCategoryMaster>, ICategoryRepository
    {
        private readonly LaundryContext _context;
        public CategoryRepository(LaundryContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ApiResult<IEnumerable<CategoryResponseViewModel>>> GetCategoryWithServiceData(CategoryFilterRequest filter)
        {
            try
            {
                SqlParameter AddressId = new SqlParameter("@Address_Id", System.Data.SqlDbType.Int) { Value = filter.AddressId };
                var storeIdTable = _context.ExecuteStoreProcedure("Usp_GetStoreId", AddressId);

                var result = await (_context.TblCategoryMaster.Join(_context.TblServiceMaster, cat => cat.Id, ser => ser.Id, (cat, ser) => new { cat, ser})
                .Select(c => new CategoryResponseViewModel
                {
                    Name = c.cat.CategoryName,
                    CategoryId = c.cat.Id,
                    ServiceId  = c.ser.Id,
                    IconUrl = c.cat.Image
                }).ToListAsync());
                return result == null
                    ? new ApiResult<IEnumerable<CategoryResponseViewModel>>(new ApiResultCode(ApiResultType.Error, 1, "No data in given request"))
                    : new ApiResult<IEnumerable<CategoryResponseViewModel>>(new ApiResultCode(ApiResultType.Success), result);
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.BusinessTier, ex);
                return new ApiResult<IEnumerable<CategoryResponseViewModel>>(new ApiResultCode(ApiResultType.ExceptionDuringOpration, 3, "Please contact system administrator"));
            }
        }


        public async Task<ApiResult<IEnumerable<CategoryResponseViewModel>>> GetCategoryByServiceId(CategoryFilterRequest filter)
        {
            try
            {
                List<CategoryResponseViewModel> categories = new List<CategoryResponseViewModel>();
                SqlParameter AddressId = new SqlParameter("@Address_Id", System.Data.SqlDbType.Int) { Value = filter.AddressId };
                SqlParameter ServiceId = new SqlParameter("@Service_Id", System.Data.SqlDbType.Int) { Value = filter.ServiceId };
                var result = _context.ExecuteStoreProcedure("Usp_GetCategory", ServiceId, AddressId);

                if (result.Tables[0].Rows.Count > 0)
                {
                    categories = (from DataRow dr in result.Tables[0].Rows
                                 select new CategoryResponseViewModel()
                                 {
                                     Name = (dr["CategoryName"] != DBNull.Value) ? Convert.ToString(dr["CategoryName"]) : string.Empty,
                                     CategoryId = (dr["CtegoryID"] != DBNull.Value) ? Convert.ToInt32(dr["CtegoryID"]) : 0,
                                     ServiceId = (dr["ServiceId"] != DBNull.Value) ? Convert.ToInt32(dr["ServiceId"]) : 0,
                                     StoreId = (dr["StoreId"] != DBNull.Value) ? Convert.ToInt32(dr["StoreId"]) : 0,
                                     IconUrl = (dr["IconUrl"] != DBNull.Value) ? Convert.ToString(dr["IconUrl"]) : string.Empty
                                 }).ToList();
                }
                return result == null
                    ? new ApiResult<IEnumerable<CategoryResponseViewModel>>(new ApiResultCode(ApiResultType.Error, 1, "No data in given request"))
                    : new ApiResult<IEnumerable<CategoryResponseViewModel>>(new ApiResultCode(ApiResultType.Success), categories);
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.BusinessTier, ex);
                return new ApiResult<IEnumerable<CategoryResponseViewModel>>(new ApiResultCode(ApiResultType.ExceptionDuringOpration, 3, "Please contact system administrator"));
            }
        }
    }
}
