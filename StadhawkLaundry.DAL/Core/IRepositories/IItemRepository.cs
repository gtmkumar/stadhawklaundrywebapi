using StadhawkCoreApi;
using StadhawkLaundry.DataModel.Models;
using StadhawkLaundry.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StadhawkLaundry.BAL.Core.IRepositories
{
    public interface IItemRepository : IRepository<TblItemMaster>
    {
       Task<ApiResult<ItemViewModel>> GetItemById(int Id);
       Task<ApiResult<IEnumerable<ItemViewModel>>> GetALLltem();
    }
}
