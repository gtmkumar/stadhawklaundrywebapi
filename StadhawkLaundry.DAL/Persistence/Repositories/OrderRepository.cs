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

        public async Task<ApiResult<IEnumerable<OrderDetailResponseViewModel>>> GetOrderByUser(int userId, string orderTypeFilter)
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


        public async Task<ApiResult<OrderDetailResponseModel>> GetOrderByOrderId(int userId, int orderId)
        {
            OrderDetailResponseModel model = null;
            OrderServiceResponseViewModel service = new OrderServiceResponseViewModel();
            List<OrderCategoryResponceViewModel> categores = new List<OrderCategoryResponceViewModel>();
            OrderCategoryResponceViewModel category = new OrderCategoryResponceViewModel();
            OrderItemDetailResponseViewModel item = null;

            try
            {
                SqlParameter OrderId = new SqlParameter("@OrderId", System.Data.SqlDbType.Int) { Value = orderId };
                SqlParameter UserId = new SqlParameter("@UserId", System.Data.SqlDbType.Int) { Value = userId };
                var result = _context.ExecuteStoreProcedure("[usp_getOrderDetail]", UserId, OrderId);
                if (result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
                {
                    foreach (System.Data.DataRow dr in result.Tables[0].Rows)
                    {
                        model = new OrderDetailResponseModel
                        {
                            OrderId = (dr["OrderId"] != DBNull.Value) ? Convert.ToInt32(dr["OrderId"]) : 0,
                            OrderDate = (dr["OrderDate"] != DBNull.Value) ? Convert.ToString(dr["OrderDate"]) : string.Empty,
                            OrderStatus = (dr["OrderStatus"] != DBNull.Value) ? Convert.ToInt32(dr["OrderStatus"]) : 0,
                            TotalPrice = (dr["TotalPrice"] != DBNull.Value) ? Convert.ToDecimal(dr["TotalPrice"]) : 0,
                            TotalKG = (dr["TotalKG"] != DBNull.Value) ? Convert.ToDecimal(dr["TotalKG"]) : 0,
                            ItemCount = (dr["ItemCount"] != DBNull.Value) ? Convert.ToInt32(dr["ItemCount"]) : 0,
                            IsKg = (dr["IsKg"] != DBNull.Value) ? Convert.ToBoolean(dr["IsKg"]) : false,
                            OrderAmount = (dr["OrderAmount"] != DBNull.Value) ? Convert.ToDecimal(dr["OrderAmount"]) : 0,
                            TaxAmount = (dr["TaxAmount"] != DBNull.Value) ? Convert.ToDecimal(dr["TaxAmount"]) : 0,
                            DeliveryDateTime = (dr["DeliveryDateTime"] != DBNull.Value) ? Convert.ToString(dr["DeliveryDateTime"]) : string.Empty,
                            PickupDateTime = (dr["PickupDateTime"] != DBNull.Value) ? Convert.ToString(dr["PickupDateTime"]) : string.Empty,
                            PickUpAddress = (dr["PickUpAddress"] != DBNull.Value) ? Convert.ToString(dr["PickUpAddress"]) : string.Empty
                        };
                    }
                    foreach (System.Data.DataRow row in result.Tables[1].Rows)
                    {
                        service = new OrderServiceResponseViewModel()
                        {
                            ServiceId = (row["ServiceId"] != DBNull.Value) ? Convert.ToInt32(row["ServiceId"]) : 0,
                            ServiceName = (row["ServiceName"] != DBNull.Value) ? Convert.ToString(row["ServiceName"]) : string.Empty
                        };
                        model.Services.Add(service);
                        if (result.Tables.Count > 0 && result.Tables[2].Rows.Count > 0)
                        {
                            foreach (System.Data.DataRow catrow in result.Tables[2].Rows)
                            {
                                if (((row["ServiceId"] != DBNull.Value) ? Convert.ToInt32(row["ServiceId"]) : 0) == ((catrow["ServiceId"] != DBNull.Value) ? Convert.ToInt32(catrow["ServiceId"]) : 0))
                                {
                                    category = new OrderCategoryResponceViewModel
                                    {
                                        CategoryId = (catrow["categoryId"] != DBNull.Value) ? Convert.ToInt32(catrow["categoryId"]) : 0,
                                        CategoryName = (catrow["CategoryName"] != DBNull.Value) ? Convert.ToString(catrow["CategoryName"]) : string.Empty
                                    };
                                    service.Categories.Add(category);
                                }
                                if (result.Tables.Count > 0 && result.Tables[3].Rows.Count > 0)
                                {
                                    foreach (System.Data.DataRow itmrow in result.Tables[3].Rows)
                                    {
                                        if ((((catrow["CategoryId"] != DBNull.Value) ? Convert.ToInt32(catrow["CategoryId"]) : 0) == ((itmrow["CategoryId"] != DBNull.Value) ? Convert.ToInt32(itmrow["CategoryId"]) : 0)) && (((catrow["ServiceId"] != DBNull.Value) ? Convert.ToInt32(catrow["ServiceId"]) : 0) == ((itmrow["ServiceId"] != DBNull.Value) ? Convert.ToInt32(itmrow["ServiceId"]) : 0)))
                                        {
                                            category.OrderItemList.Add(new OrderItemDetailResponseViewModel
                                            {
                                                ItemId = (itmrow["ItemId"] != DBNull.Value) ? Convert.ToInt32(itmrow["ItemId"]) : 0,
                                                ItemName = (itmrow["ItemName"] != DBNull.Value) ? Convert.ToString(itmrow["ItemName"]) : string.Empty,
                                                Quantity = (itmrow["Quantity"] != DBNull.Value) ? Convert.ToInt32(itmrow["Quantity"]) : 0,
                                                TotalPrice = (itmrow["TotalPrice"] != DBNull.Value) ? Convert.ToDecimal(itmrow["TotalPrice"]) : 0,
                                                UnitPrice = (itmrow["UnitPrice"] != DBNull.Value) ? Convert.ToDecimal(itmrow["UnitPrice"]) : 0
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.ApplicationTier, ex);
                return new ApiResult<OrderDetailResponseModel>(new ApiResultCode(ApiResultType.Error, 0, "No data in given request"));
            }
            return model == null ? new ApiResult<OrderDetailResponseModel>(new ApiResultCode(ApiResultType.Error, 1, "No data in given request"))
                : new ApiResult<OrderDetailResponseModel>(new ApiResultCode(ApiResultType.Success), model);

        }

    }
}
