using StadhawkCoreApi;
using StadhawkLaundry.BAL.Core.IRepositories;
using StadhawkLaundry.DataModel;
using StadhawkLaundry.DataModel.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using StadhawkLaundry.ViewModel;
using StadhawkCoreApi.Logger;

namespace StadhawkLaundry.BAL.Persistence.Repositories
{
    public class SubCategoryRepository : Repository<TblSubcategory>, ISubcategoryRepository
    {
        private readonly LaundryContext _context;
        public SubCategoryRepository(LaundryContext context) : base(context)
        {
            _context = context;
        }
        public async Task<ApiResult<SubcategoryViewModel>> GetSubcategoryById(Guid Id)
        {
            try
            {
                var result = await (_context.TblSubcategory.Join(_context.TblCategory, subcat => subcat.CategoryId, cat => cat.Id, (subcat, cat) => new { subcat, cat }).Where(t => t.subcat.Id== Id)
                .Select(c => new SubcategoryViewModel
                {
                    Name = c.subcat.Name,
                    CategoryId = c.subcat.CategoryId,
                    ServiceId = c.cat.ServiceId,
                    Category = c.cat.Name,
                    Price = c.subcat.Price,
                    SubcategoryId = c.subcat.Id.ToString()
                }).FirstOrDefaultAsync());
                return result == null
                    ? new ApiResult<SubcategoryViewModel>(new ApiResultCode(ApiResultType.Error, 1, "No data in given request"))
                    : new ApiResult<SubcategoryViewModel>(new ApiResultCode(ApiResultType.Success), result);
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.BusinessTier, ex);
                return new ApiResult<SubcategoryViewModel>(new ApiResultCode(ApiResultType.ExceptionDuringOpration, 3, "Please contact system administrator"));
            }
        }

        public async Task<ApiResult<IEnumerable<SubcategoryViewModel>>> GetJoinedData()
        {
            try
            {
                var result = await (_context.TblSubcategory.Join(_context.TblCategory, subcat => subcat.CategoryId, cat => cat.Id, (subcat, cat) => new { subcat, cat })
                .Select(c => new SubcategoryViewModel
                {
                    Name = c.subcat.Name,
                    CategoryId = c.subcat.CategoryId,
                    Category = c.cat.Name,
                    Price = c.subcat.Price,
                    SubcategoryId = c.subcat.Id.ToString()
                }).ToListAsync());
                return result == null
                    ? new ApiResult<IEnumerable<SubcategoryViewModel>>(new ApiResultCode(ApiResultType.Error, 1, "No data in given request"))
                    : new ApiResult<IEnumerable<SubcategoryViewModel>>(new ApiResultCode(ApiResultType.Success), result);
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.BusinessTier, ex);
                return new ApiResult<IEnumerable<SubcategoryViewModel>>(new ApiResultCode(ApiResultType.ExceptionDuringOpration, 3, "Please contact system administrator"));
            }
        }
    }
}
