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
        ISubcategoryRepository ISubcategory { get; }
        IItemRepository IItem { get; }
        ISizeRepository ISize { get; }
        IUsersRepository IUser { get; }
        IUsersInRolesRepository IUsersInRoles { get; }
        IRoleRepository IRole { get; }
        IOrderRepository IOrder { get; }
        IMenuMasterServiceRepository IMenuMasterService { get; }
        IUserAddressRepository IUserAddress { get; }

        //IServiceMasterRepository IServiceMaster { get; }
        Task<ApiResultCode> Complete();
    }
}
