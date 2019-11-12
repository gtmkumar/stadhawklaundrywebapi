using StadhawkCoreApi;
using StadhawkLaundry.DataModel.Models;
using StadhawkLaundry.ViewModel.RequestModel;
using StadhawkLaundry.ViewModel.ResponseModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StadhawkLaundry.BAL.Core.IRepositories
{
    public interface ICartRepository : IRepository<TblCart>
    {
        Task<ApiResult<bool>> AddToCartAsync(AddCartRequestViewModel customerAddToCart, int userId);
        Task<ApiResult<CartCountResponseViewModel>> CartCountAndPrice(int userId, string con);
        Task<ApiResult<CartPriceDetail>> GetCartDetail(int userId, int addressId);
        Task<ApiResult<CartOrderDetailResponseViewModel>> GetCartDetails(int userId, int addressId);
        Task<ApiResult<ExistingCheckResponseViewModel>> IsCartFromDiffrentService(int storeItemId, int userId);
        Task<ApiResult<bool>> CartRemove(int CartId);
        Task<ApiResult<bool>> AddToServiceCartAsync(AddServiceCartRequestViewModel model, int userId);
        Task<ApiResult<CartServiceCountResponseViewModel>> CartServiceCountAndPrice(int userId);
        Task<ApiResult<bool>> CartServiceRemove(int CartId);
    }
}
