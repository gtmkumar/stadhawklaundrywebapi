using StadhawkCoreApi;
using StadhawkLaundry.BAL.Core.IRepositories;
using StadhawkLaundry.DataModel;
using StadhawkLaundry.DataModel.Models;
using StadhawkLaundry.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StadhawkLaundry.ViewModel.ResponseModel;
using System.Data.SqlClient;
using StadhawkCoreApi.Logger;
using StadhawkLaundry.ViewModel.RequestModel;
using System.Data;

namespace StadhawkLaundry.BAL.Persistence.Repositories
{
    public class OrderRepository : Repository<TblOrder>, IOrderRepository
    {
        private readonly LaundryContext _context;
        public OrderRepository(LaundryContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ApiResult<OrderViewModel>> GetItemDetails()
        {
            OrderViewModel orderView = new OrderViewModel();
            var services = await _context.TblServiceMaster.ToListAsync();
            var categories = await _context.TblCategoryMaster.ToListAsync();
            var subcategories = await _context.TblSubServiceMaster.ToListAsync();
            var items = await _context.TblItemMaster.ToListAsync();

            foreach (var item in services)
            {
                orderView.Services.Add(new ServicesViewModel()
                {
                    ServiceId = item.Id,
                    Name = item.ServiceName
                });
            }

            foreach (var item in categories)
            {
                orderView.Categories.Add(new CategoryViewModel()
                {
                    CategoryId = item.Id,
                    Name = item.CategoryName
                });
            }

            foreach (var item in subcategories)
            {
                orderView.Subcategories.Add(new SubcategoryViewModel()
                {
                    SubcategoryId = item.Id.ToString(),
                    Name = item.SubServiceName
                });
            }

            foreach (var item in items)
            {
                orderView.Items.Add(new ItemViewModel()
                {
                    ItemId = item.Id,
                    Name = item.ItemName
                });
            }
            return orderView == null
                    ? new ApiResult<OrderViewModel>(new ApiResultCode(ApiResultType.Error, 1, "No data in given request"))
                    : new ApiResult<OrderViewModel>(new ApiResultCode(ApiResultType.Success), orderView);
        }


        public async Task<ApiResult<OrderResponseViewModel>> CreateOrder(int userId, OrderRequestViewModel orderModel)
        {
            {
                OrderResponseViewModel model = null;
                try
                {
                    SqlParameter UserId = new SqlParameter("@userId", System.Data.SqlDbType.Int) { Value = userId };
                    SqlParameter AddressId = new SqlParameter("@AddressId", System.Data.SqlDbType.Int) { Value = orderModel.AddressId };
                    SqlParameter PickUpSlotId = new SqlParameter("@PickUpSlotId", System.Data.SqlDbType.Int) { Value = orderModel.PickUpSlotId };
                    SqlParameter PickUpDate = new SqlParameter("@PickUpDate", System.Data.SqlDbType.DateTime) { Value = orderModel.PickUpDate };
                    SqlParameter DeliverDate = new SqlParameter("@DeliverDate", System.Data.SqlDbType.DateTime) { Value = orderModel.DeliverDate };
                    SqlParameter DeliverSlotId = new SqlParameter("@DeliverSlotId", System.Data.SqlDbType.Int) { Value = orderModel.DeliverSlotId };
                    SqlParameter DeliveryNote = new SqlParameter("@DeliveryNote", System.Data.SqlDbType.VarChar) { Value = orderModel.DeliveryNote ?? (object)DBNull.Value };
                    var result = _context.ExecuteStoreProcedure("[CreateOrder]", UserId, AddressId, PickUpSlotId, PickUpDate, DeliverDate, DeliverSlotId, DeliveryNote);
                    if (result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
                    {
                        foreach (System.Data.DataRow row in result.Tables[0].Rows)
                        {
                            model = new OrderResponseViewModel()
                            {
                                OrderId = (row["OrderId"] != DBNull.Value) ? Convert.ToInt32(row["OrderId"]) : 0
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorTrace.Logger(LogArea.ApplicationTier, ex);
                    return new ApiResult<OrderResponseViewModel>(new ApiResultCode(ApiResultType.Error, 0, "No data in given request"));
                }
                return new ApiResult<OrderResponseViewModel>(new ApiResultCode(ApiResultType.Success), model);
            }
        }

        public async Task<ApiResult<List<TimeSlotViewModel>>> GetAvailableSlots()
        {
            List<TimeSlotViewModel> TimeSlot = new List<TimeSlotViewModel>();
            TimeSlotViewModel timeSlotViewModel;
            List<TimeSlots> slots;
            TimeSlots timeSlots;
            SqlParameter NoOfDays = new SqlParameter("@NoOfDays", System.Data.SqlDbType.VarChar) { Value = 10 };
            var data = _context.ExecuteStoreProcedure("usp_GetAvailableSlots", NoOfDays);
            string Date = "";
            for (int i = 0; i < data.Tables[0].Rows.Count; i++)
            {
                if (Date != data.Tables[0].Rows[i]["FullDate"].ToString())
                {
                    Date = data.Tables[0].Rows[i]["FullDate"].ToString();
                    timeSlotViewModel = new TimeSlotViewModel();
                    timeSlotViewModel.Key = i;
                    timeSlotViewModel.FullDate= data.Tables[0].Rows[i]["FullDate"].ToString();
                    timeSlotViewModel.Date = data.Tables[0].Rows[i]["Date"].ToString();
                    timeSlotViewModel.Day = data.Tables[0].Rows[i]["Day"].ToString();
                    timeSlotViewModel.Month = data.Tables[0].Rows[i]["Month"].ToString();
                    timeSlotViewModel.ShortMonth = data.Tables[0].Rows[i]["ShortMonth"].ToString();

                    slots = new List<TimeSlots>();
                    for (int j = 0; j < data.Tables[0].Rows.Count; j++)
                    {
                        if (Date == data.Tables[0].Rows[j]["FullDate"].ToString())
                        {
                            timeSlots = new TimeSlots();
                            timeSlots.SlotId = Convert.ToInt32(data.Tables[0].Rows[j]["SlotId"].ToString());
                            timeSlots.Label = data.Tables[0].Rows[j]["Label"].ToString();
                            timeSlots.Icon = data.Tables[0].Rows[j]["Icon"].ToString();
                            timeSlots.SlotRange = data.Tables[0].Rows[j]["SlotRange"].ToString();
                            timeSlots.IsSlotAvailable = true;
                            slots.Add(timeSlots);
                        }
                    }

                    timeSlotViewModel.timeSlots = slots;
                    TimeSlot.Add(timeSlotViewModel);
                }

            }

            return TimeSlot == null
                    ? new ApiResult<List<TimeSlotViewModel>>(new ApiResultCode(ApiResultType.Error, 1, "No data in given request"))
                    : new ApiResult<List<TimeSlotViewModel>>(new ApiResultCode(ApiResultType.Success), TimeSlot);
        }

        public async Task<ApiResult<List<TimeSlotViewModel>>> GetAvailableDropSlots(DateTime dateTime)
        {
            List<TimeSlotViewModel> TimeSlot = new List<TimeSlotViewModel>();
            TimeSlotViewModel timeSlotViewModel;
            List<TimeSlots> slots;
            TimeSlots timeSlots;
            SqlParameter NoOfDays = new SqlParameter("@NoOfDays", System.Data.SqlDbType.VarChar) { Value = 10 };
            SqlParameter FullDate = new SqlParameter("@FullDate", System.Data.SqlDbType.DateTime) { Value = dateTime };
            try
            {
                var data = _context.ExecuteStoreProcedure("[usp_GetAvailableDropSlots]", NoOfDays, FullDate);
                string Date = "";
                for (int i = 0; i < data.Tables[0].Rows.Count; i++)
                {
                    if (Date != data.Tables[0].Rows[i]["FullDate"].ToString())
                    {
                        Date = data.Tables[0].Rows[i]["FullDate"].ToString();
                        timeSlotViewModel = new TimeSlotViewModel();
                        timeSlotViewModel.Key = i;
                        timeSlotViewModel.FullDate = data.Tables[0].Rows[i]["FullDate"].ToString();
                        timeSlotViewModel.Date = data.Tables[0].Rows[i]["Date"].ToString();
                        timeSlotViewModel.Day = data.Tables[0].Rows[i]["Day"].ToString();
                        timeSlotViewModel.Month = data.Tables[0].Rows[i]["Month"].ToString();
                        timeSlotViewModel.ShortMonth = data.Tables[0].Rows[i]["ShortMonth"].ToString();

                        slots = new List<TimeSlots>();
                        for (int j = 0; j < data.Tables[0].Rows.Count; j++)
                        {
                            if (Date == data.Tables[0].Rows[j]["FullDate"].ToString())
                            {
                                timeSlots = new TimeSlots();
                                timeSlots.SlotId = Convert.ToInt32(data.Tables[0].Rows[j]["SlotId"].ToString());
                                timeSlots.Label = data.Tables[0].Rows[j]["Label"].ToString();
                                timeSlots.Icon = data.Tables[0].Rows[j]["Icon"].ToString();
                                timeSlots.SlotRange = data.Tables[0].Rows[j]["SlotRange"].ToString();
                                timeSlots.IsSlotAvailable = true;
                                slots.Add(timeSlots);
                            }
                        }

                        timeSlotViewModel.timeSlots = slots;
                        TimeSlot.Add(timeSlotViewModel);
                    }

                }

                return TimeSlot == null
                        ? new ApiResult<List<TimeSlotViewModel>>(new ApiResultCode(ApiResultType.Error, 1, "No data in given request"))
                        : new ApiResult<List<TimeSlotViewModel>>(new ApiResultCode(ApiResultType.Success), TimeSlot);
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.ApplicationTier, ex);
                return new ApiResult<List<TimeSlotViewModel>>(new ApiResultCode(ApiResultType.ExceptionDuringOpration, 1, "No data in given request"));
            }
        }

        public async Task<ApiResult<IEnumerable<OrderDetailResponseViewModel>>> GetOrderByUser(int userId)
        {
            List<OrderDetailResponseViewModel> listitems = new List<OrderDetailResponseViewModel>();
            try
            {
                SqlParameter Userid = new SqlParameter("@Userid", System.Data.SqlDbType.Int) { Value = userId };
                var ietms = _context.ExecuteStoreProcedure("[usp_getOrderDetailList]", Userid);

                if (ietms.Tables[0].Rows.Count > 0)
                {
                    listitems = (from DataRow dr in ietms.Tables[0].Rows
                                 select new OrderDetailResponseViewModel()
                                 {
                                     OrderId = (dr["OrderId"] != DBNull.Value) ? Convert.ToInt32(dr["OrderId"]) : 0,
                                     OrderDate = (dr["OrderDate"] != DBNull.Value) ? Convert.ToString(dr["OrderDate"]) : string.Empty,
                                     TotalKg = (dr["TotalKg"] != DBNull.Value) ? Convert.ToInt32(dr["TotalKg"]) : 0,
                                     TotalPrice = (dr["TotalPrice"] != DBNull.Value) ? Convert.ToDecimal(dr["TotalPrice"]) : 0,
                                     IsKG = false,
                                     isRepeatOrder = false,
                                     ItemCount = (dr["ItemCount"] != DBNull.Value) ? Convert.ToInt32(dr["ItemCount"]) : 0,
                                     OrderStatus = (dr["OrderStatus"] != DBNull.Value) ? Convert.ToInt32(dr["OrderStatus"]) : 0
                                 }).ToList();
                }
                return listitems.Count < 0
                            ? new ApiResult<IEnumerable<OrderDetailResponseViewModel>>(new ApiResultCode(ApiResultType.Error, 1, "No data in given request"))
                            : new ApiResult<IEnumerable<OrderDetailResponseViewModel>>(new ApiResultCode(ApiResultType.Success), listitems);
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.BusinessTier, ex);
                return new ApiResult<IEnumerable<OrderDetailResponseViewModel>>(new ApiResultCode(ApiResultType.ExceptionDuringOpration, 3, "Please contact system administrator"));
            }
        }

    }
}
