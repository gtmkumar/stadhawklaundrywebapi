using StadhawkCoreApi;
using StadhawkCoreApi.Logger;
using StadhawkLaundry.BAL.Core.IRepositories;
using StadhawkLaundry.DataModel;
using StadhawkLaundry.DataModel.Models;
using StadhawkLaundry.ViewModel.ResponseModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace StadhawkLaundry.BAL.Persistence.Repositories
{
    public class BannerRepository : Repository<TblServiceMaster>, IBannerRepository
    {
        private LaundryContext _context;
        public BannerRepository(LaundryContext context) : base(context)
        {
            _context = context;
        }
        public async Task<ApiResult<IEnumerable<BannerResponseViewModel>>> GetBanner(int storeId)
        {
            BannerResponseViewModel banner = null;
            List<BannerResponseViewModel> bannerList = new List<BannerResponseViewModel>();
            try
            {
                SqlParameter Storedid = new SqlParameter("@StoreId", System.Data.SqlDbType.Int) { Value = storeId };
                var result = _context.ExecuteStoreProcedure("usp_GetBanner", Storedid);
                if (result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
                {
                    foreach (System.Data.DataRow row in result.Tables[0].Rows)
                    {
                        
                            bannerList.Add(new BannerResponseViewModel
                            {
                                BannerId = (row["BannerId"] != DBNull.Value) ? Convert.ToInt32(row["BannerId"]) : 0,
                                BannerName = (row["BannerName"] != DBNull.Value) ? Convert.ToString(row["BannerName"]) : string.Empty,
                                BannerImage = (row["BannerImage"] != DBNull.Value) ? Convert.ToString(row["BannerImage"]) : string.Empty,
                                BannerCategoryId = (row["BannerCategoryId"] != DBNull.Value) ? Convert.ToInt32(row["BannerCategoryId"]) : 0,
                                BannerCategoryName = (row["BannerCategoryName"] != DBNull.Value) ? Convert.ToString(row["BannerCategoryName"]) : string.Empty
                            });

                       
                    }
                }

                if (bannerList.Count>0)
                return new ApiResult<IEnumerable<BannerResponseViewModel>>(new ApiResultCode(ApiResultType.Success), bannerList);
            }
            catch (Exception ex)
            {
                StadhawkCoreApi.Logger.ErrorTrace.Logger(LogArea.ApplicationTier, ex);
            }
            return new ApiResult<IEnumerable<BannerResponseViewModel>>(new ApiResultCode(ApiResultType.Error), bannerList);
        }
    }
}
