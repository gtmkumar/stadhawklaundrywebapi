using StadhawkCoreApi;
using StadhawkLaundry.DataModel.Models;
using StadhawkLaundry.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StadhawkLaundry.BAL.Core.IRepositories
{
    public interface ISubcategoryRepository : IRepository<TblSubcategory>
    {
        Task<ApiResult<IEnumerable<SubcategoryViewModel>>> GetJoinedData();
        Task<ApiResult<SubcategoryViewModel>> GetSubcategoryById(Guid id);
    }
}
