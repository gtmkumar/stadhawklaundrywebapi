using StadhawkCoreApi;
using StadhawkLaundry.DataModel.Models;
using StadhawkLaundry.ViewModel;
using StadhawkLaundry.ViewModel.ResponseModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StadhawkLaundry.BAL.Core.IRepositories
{
    public interface IOrderRepository : IRepository<TblOrder>
    {
        Task<ApiResult<OrderViewModel>> GetItemDetails();
        Task<ApiResult<OrderResponseViewModel>> CreateOrder(int userId);
        Task<ApiResult<List<TimeSlotViewModel>>> GetAvailableSlots();
    }
}
