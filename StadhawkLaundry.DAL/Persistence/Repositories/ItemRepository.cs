using Microsoft.EntityFrameworkCore;
using StadhawkCoreApi;
using StadhawkCoreApi.Logger;
using StadhawkLaundry.BAL.Core.IRepositories;
using StadhawkLaundry.DataModel;
using StadhawkLaundry.DataModel.Models;
using StadhawkLaundry.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StadhawkLaundry.BAL.Persistence.Repositories
{
    public class ItemRepository : Repository<TblItem>, IItemRepository
    {
        private readonly LaundryContext _context;
        public ItemRepository(LaundryContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ApiResult<ItemViewModel>> GetItemById(string Id)
        {
            try
            {
                var result = await (from itm in _context.TblItem
                                    join subcat in _context.TblSubcategory on itm.ServiceId equals subcat.Id
                                    join cat in _context.TblCategory on subcat.CategoryId equals cat.Id
                                    join ser in _context.TblService on cat.ServiceId equals ser.Id
                                    select new ItemViewModel
                                    {
                                        Name = itm.Name,
                                        ServiceId = ser.Id,
                                        CategoryId = cat.Id,
                                        SubcategoryId = itm.ServiceId,
                                        ItemId = itm.Id.ToString(),
                                        Price =Convert.ToDecimal(ser.Id),
                                    }
                                ).Where(t => t.ItemId == Id).FirstOrDefaultAsync();

                return result == null
                        ? new ApiResult<ItemViewModel>(new ApiResultCode(ApiResultType.Error, 1, "No data in given request"))
                        : new ApiResult<ItemViewModel>(new ApiResultCode(ApiResultType.Success), result);
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.BusinessTier, ex);
                return new ApiResult<ItemViewModel>(new ApiResultCode(ApiResultType.ExceptionDuringOpration, 3, "Please contact system administrator"));
            }
            
        }

        public async Task<ApiResult<IEnumerable<ItemViewModel>>> GetALLltem()
        {
            try
            {
                var result = await (from itm in _context.TblItem
                                    join subcat in _context.TblSubcategory on itm.Id equals subcat.Id
                                    join cat in _context.TblCategory on subcat.CategoryId equals cat.Id
                                    join ser in _context.TblService on cat.ServiceId equals ser.Id
                                    select new ItemViewModel
                                    {
                                        Name = itm.Name,
                                        ServiceId = ser.Id,
                                        CategoryId = cat.Id,
                                        SubcategoryId = itm.ServiceId,
                                        ItemId = itm.Id.ToString(),
                                        Price = Convert.ToDecimal(itm.Id),
                                        ServiceName = ser.Name,
                                        CategoryName = cat.Name,
                                        SubcategoryName = subcat.Name
                                    }
                                ).ToListAsync();

                return result == null
                        ? new ApiResult<IEnumerable<ItemViewModel>>(new ApiResultCode(ApiResultType.Error, 1, "No data in given request"))
                        : new ApiResult<IEnumerable<ItemViewModel>>(new ApiResultCode(ApiResultType.Success), result);
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.BusinessTier, ex);
                return new ApiResult<IEnumerable<ItemViewModel>>(new ApiResultCode(ApiResultType.ExceptionDuringOpration, 3, "Please contact system administrator"));
            }
        }
    }
}
