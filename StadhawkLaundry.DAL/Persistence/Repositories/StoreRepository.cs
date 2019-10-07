using StadhawkCoreApi;
using StadhawkCoreApi.Logger;
using StadhawkLaundry.BAL.Core.IRepositories;
using StadhawkLaundry.DataModel;
using StadhawkLaundry.DataModel.Models;
using StadhawkLaundry.ViewModel.ResponseModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
namespace StadhawkLaundry.BAL.Persistence.Repositories
{
    public class StoreRepository : Repository<TblStore>, IStoreRepository
    {
        private readonly LaundryContext _context;
        public StoreRepository(LaundryContext context) : base(context)
        {
            _context = context;
        }
        public async Task<ApiResult<IEnumerable<StoreResponseViewModel>>> GetStoreByAddress(int addressId)
        {
            List<StoreResponseViewModel> listitems = new List<StoreResponseViewModel>();
            try
            {
                SqlParameter AddressId = new SqlParameter("@Address_Id", System.Data.SqlDbType.Int) { Value = addressId };
                var ietms = _context.ExecuteStoreProcedure("[Usp_GetStore]", AddressId);

                if (ietms.Tables[0].Rows.Count > 0)
                {
                    listitems = (from DataRow dr in ietms.Tables[0].Rows
                                 select new StoreResponseViewModel()
                                 {
                                     StoreId = (dr["StoreId"] != DBNull.Value) ? Convert.ToInt32(dr["StoreId"]) : 0,
                                     StoreName = (dr["StoreName"] != DBNull.Value) ? Convert.ToString(dr["StoreName"]) : string.Empty,
                                     FullAddress = (dr["FullAddress"] != DBNull.Value) ? Convert.ToString(dr["FullAddress"]) : string.Empty
                                 }).ToList();
                }
                return listitems.Count < 0
                            ? new ApiResult<IEnumerable<StoreResponseViewModel>>(new ApiResultCode(ApiResultType.Error, 1, "No data in given request"))
                            : new ApiResult<IEnumerable<StoreResponseViewModel>>(new ApiResultCode(ApiResultType.Success), listitems);
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.BusinessTier, ex);
                return new ApiResult<IEnumerable<StoreResponseViewModel>>(new ApiResultCode(ApiResultType.ExceptionDuringOpration, 3, "Please contact system administrator"));
            }
        }
    }
}
