using StadhawkCoreApi;
using StadhawkCoreApi.Logger;
using StadhawkLaundry.BAL.Core;
using StadhawkLaundry.BAL.Core.IRepositories;
using StadhawkLaundry.BAL.Persistence.Repositories;
using StadhawkLaundry.DataModel;
using System;
using System.Threading.Tasks;

namespace StadhawkLaundry.BAL.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LaundryContext _context;
        public UnitOfWork(LaundryContext context)
        {
            _context = context;
        }
        private IServicesRepository _service;
        private ICategoryRepository _category;
        private IItemRepository _item;
        private IUsersRepository _iUser;
        private IUsersInRolesRepository _iUsersInRoles;
        private IOrderRepository _iOrder;
        private IMenuMasterServiceRepository _iMenuMasterService;
        private IUserAddressRepository _userAddress;

        public IServicesRepository IService => _service ?? (_service = new ServicesRepository(_context));
        public ICategoryRepository ICategory => _category ?? (_category = new CategoryRepository(_context));
        public IItemRepository IItem => _item ?? (_item = new ItemRepository(_context));
        public IUsersRepository IUser => _iUser ?? (_iUser = new UsersRepository(_context));
        public IUsersInRolesRepository IUsersInRoles => _iUsersInRoles ?? (_iUsersInRoles = new UsersInRolesRepository(_context));
        public IOrderRepository IOrder => _iOrder ?? (_iOrder = new OrderRepository(_context));
        public IMenuMasterServiceRepository IMenuMasterService => _iMenuMasterService ?? (_iMenuMasterService = new MenuMasterServiceRepository(_context));
        public IUserAddressRepository IUserAddress => _userAddress ?? (_userAddress = new UserAddressRepository(_context));
        public async Task<ApiResultCode> Complete()
        {
            try
            {
                await _context.SaveChangesAsync();
                return new ApiResultCode(ApiResultType.Success, 1, "Save successfully");
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.ApplicationTier, ex);
                return new ApiResultCode(ApiResultType.ExceptionDuringOpration, 3, "Exception during saveing");

            }
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
