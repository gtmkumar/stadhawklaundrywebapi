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


        public async Task<ApiResult<OrderResponseViewModel>> CreateOrder(int userId)
        {
            {
                OrderResponseViewModel model = null;
                try
                {
                    SqlParameter UserId = new SqlParameter("@userId", System.Data.SqlDbType.Int) { Value = userId };
                    var result = _context.ExecuteStoreProcedure("[CreateOrder]", UserId);
                    if (result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
                    {
                        foreach (System.Data.DataRow row in result.Tables[0].Rows)
                        {
                            model = new OrderResponseViewModel()
                            {
                                OrderId = (row["OrderId"] != DBNull.Value) ? Convert.ToInt32(row["ServiceId"]) : 0
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


    }
}
