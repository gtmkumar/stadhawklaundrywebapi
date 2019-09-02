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
            var services = await _context.TblService.ToListAsync();
            var categories = await _context.TblCategory.ToListAsync();
            var subcategories = await _context.TblSubcategory.ToListAsync();
            var items = await _context.TblItem.ToListAsync();

            foreach (var item in services)
            {
                orderView.Services.Add(new ServicesViewModel()
                {
                    ServiceId = item.Id.ToString(),
                    Name = item.Name
                });
            }

            foreach (var item in categories)
            {
                orderView.Categories.Add(new CategoryViewModel()
                {
                    CategoryId = item.Id.ToString(),
                    Name = item.Name
                });
            }

            foreach (var item in subcategories)
            {
                orderView.Subcategories.Add(new SubcategoryViewModel()
                {
                    SubcategoryId = item.Id.ToString(),
                    Name = item.Name
                });
            }

            foreach (var item in items)
            {
                orderView.Items.Add(new ItemViewModel()
                {
                    ItemId = item.Id.ToString(),
                    Name = item.Name
                });
            }
            return orderView == null
                    ? new ApiResult<OrderViewModel>(new ApiResultCode(ApiResultType.Error, 1, "No data in given request"))
                    : new ApiResult<OrderViewModel>(new ApiResultCode(ApiResultType.Success), orderView);
        }
        
        //public async Task<ApiResult<OrderViewModel>> GetItemDetails()
        //{
        //    var result = await (from s in _context.TblService
        //                        join cat in _context.TblCategory on s.Id equals cat.ServiceId
        //                        join subcat in _context.TblSubcategory on cat.Id equals subcat.CategoryId
        //                        join itm in _context.TblItem on subcat.Id equals itm.SubcategoryId
        //                        select new OrderViewModel()
        //                        {
        //                            Services = new List<ServicesViewModel>() { new ServicesViewModel {
        //                         ServiceId = s.Id.ToString(),
        //                         Name = s.Name
        //                     } },
        //                            Categories = new List<CategoryViewModel>() { new CategoryViewModel() {
        //                         CategoryId = cat.Id.ToString(),
        //                         Name = cat.Name
        //                     } },
        //                            Subcategories = new List<SubcategoryViewModel>() { new SubcategoryViewModel() {
        //                         SubcategoryId = subcat.Id.ToString(),
        //                         Name = subcat.Name
        //                     } },
        //                            Items = new List<ItemViewModel>() { new ItemViewModel() {
        //                         ItemId = itm.Id.ToString(),
        //                         Name=itm.Name,
        //                         Price=itm.Price
        //                     } }
        //                        }).FirstOrDefaultAsync();

        //    return result == null
        //            ? new ApiResult<OrderViewModel>(new ApiResultCode(ApiResultType.Error, 1, "No data in given request"))
        //            : new ApiResult<OrderViewModel>(new ApiResultCode(ApiResultType.Success), result);
        //}

    }
}
