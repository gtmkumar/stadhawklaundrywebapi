using StadhawkCoreApi;
using StadhawkLaundry.DataModel.Models;
using StadhawkLaundry.ViewModel;
using StadhawkLaundry.ViewModel.RequestModel;
using StadhawkLaundry.ViewModel.ResponseModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StadhawkLaundry.BAL.Core.IRepositories
{
    public interface ICategoryRepository : IRepository<TblCategoryMaster>
    {
        Task<ApiResult<IEnumerable<CategoryResponseViewModel>>> GetCategoryWithServiceData(CategoryFilterRequest filter);
        Task<ApiResult<IEnumerable<CategoryResponseViewModel>>> GetCategoryByServiceId(int serviceId);
    }
}
