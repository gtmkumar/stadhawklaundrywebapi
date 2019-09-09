using StadhawkCoreApi;
using StadhawkLaundry.DataModel.Models;
using StadhawkLaundry.ViewModel.ResponseModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StadhawkLaundry.BAL.Core.IRepositories
{
    public interface IBannerRepository : IRepository<TblServiceMaster>
    {
        Task<ApiResult<IEnumerable<BannerResponseViewModel>>> GetBanner(int storeId);
    }
}
