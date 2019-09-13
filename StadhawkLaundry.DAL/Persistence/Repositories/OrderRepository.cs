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
    }
}
