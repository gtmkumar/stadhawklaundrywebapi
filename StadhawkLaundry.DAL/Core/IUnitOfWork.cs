using StadhawkCoreApi;
using StadhawkLaundry.BAL.Core.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StadhawkLaundry.BAL.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IServicesRepository IService { get; }
        ICategoryRepository ICategory { get; }
        IItemRepository IItem { get; }
        IUsersRepository IUser { get; }
        IUsersInRolesRepository IUsersInRoles { get; }
        IOrderRepository IOrder { get; }
        IMenuMasterServiceRepository IMenuMasterService { get; }
        Task<ApiResultCode> Complete();
    }
}
