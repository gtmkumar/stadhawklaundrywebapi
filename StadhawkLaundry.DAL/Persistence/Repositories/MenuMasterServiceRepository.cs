using StadhawkLaundry.BAL.Core.IRepositories;
using StadhawkLaundry.DataModel;
using StadhawkLaundry.DataModel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StadhawkLaundry.BAL.Persistence.Repositories
{
    public class MenuMasterServiceRepository : Repository<MenuMaster>, IMenuMasterServiceRepository
    {
        private readonly LaundryContext _context;
        public MenuMasterServiceRepository(LaundryContext context) : base(context)
        {
            _context = context;
        }
    }
}
