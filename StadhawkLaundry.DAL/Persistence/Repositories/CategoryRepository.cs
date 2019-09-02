using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StadhawkCoreApi;
using StadhawkCoreApi.Logger;
using StadhawkLaundry.BAL.Core.IRepositories;
using StadhawkLaundry.DataModel;
using StadhawkLaundry.DataModel.Models;
using StadhawkLaundry.ViewModel;

namespace StadhawkLaundry.BAL.Persistence.Repositories
{
    public class CategoryRepository : Repository<TblCategory>, ICategoryRepository
    {
        private readonly LaundryContext _context;
        public CategoryRepository(LaundryContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ApiResult<IEnumerable<CategoryViewModel>>> GetCategoryWithServiceData()
        {
            try
            {
                var result = await (_context.TblCategory.Join(_context.TblService, cat => cat.ServiceId, ser => ser.Id, (cat, ser) => new { cat, ser})
                .Select(c => new CategoryViewModel
                {
                    Service = c.ser.Name,
                    Name = c.cat.Name,
                    CategoryId = c.cat.Id.ToString(),
                    ServiceId  = c.ser.Id

                }).ToListAsync());
                return result == null
                    ? new ApiResult<IEnumerable<CategoryViewModel>>(new ApiResultCode(ApiResultType.Error, 1, "No data in given request"))
                    : new ApiResult<IEnumerable<CategoryViewModel>>(new ApiResultCode(ApiResultType.Success), result);
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.BusinessTier, ex);
                return new ApiResult<IEnumerable<CategoryViewModel>>(new ApiResultCode(ApiResultType.ExceptionDuringOpration, 3, "Please contact system administrator"));
            }
        }
    }
}
