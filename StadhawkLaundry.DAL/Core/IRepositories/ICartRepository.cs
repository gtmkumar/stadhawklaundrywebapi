using StadhawkCoreApi;
using StadhawkLaundry.DataModel.Models;
using StadhawkLaundry.ViewModel.RequestModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StadhawkLaundry.BAL.Core.IRepositories
{
    public interface ICartRepository : IRepository<TblCart>
    {
        Task<ApiResult<bool>> AddToCartAsync(AddCartRequestViewModel customerAddToCart, int userId);
    }
}
