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
    public interface IOrderRepository : IRepository<TblOrder>
    {
        Task<ApiResult<OrderViewModel>> GetItemDetails();
        Task<ApiResult<OrderResponseViewModel>> CreateOrder(int userId, OrderRequestViewModel model);
        Task<ApiResult<List<TimeSlotViewModel>>> GetAvailableSlots();
        Task<ApiResult<List<TimeSlotViewModel>>> GetAvailableDropSlots(DateTime dateTime);
        Task<ApiResult<IEnumerable<OrderDetailResponseViewModel>>> GetOrderByUser(int userId,string orderTypeFilter);
        Task<ApiResult<OrderDetailResponseModel>> GetOrderByOrderId(int userId, int orderId);
        ApiResult<PaymentOrderResponceViewModel> GetOrderDetails(int orderId);
        ApiResult<bool> IsOrderRefExist(string invoiceNo, string pgType);
        ApiResult<string> SaveCustomerPaymentInfo(PaymetIfoRequestViewModel model);
        Task<ApiResult<IEnumerable<OrderDetailResponseViewModel>>> GetOrderByDeliveryBoyId(int userId, string orderTypeFilter);
    }
}
