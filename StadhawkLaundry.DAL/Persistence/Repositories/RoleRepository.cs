using StadhawkLaundry.BAL.Core.IRepositories;
using StadhawkLaundry.DataModel;
using StadhawkLaundry.DataModel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StadhawkLaundry.BAL.Persistence.Repositories
{
    public class RoleRepository : Repository<Roles>, IRoleRepository
    {
        private readonly LaundryContext _context;
        public RoleRepository(LaundryContext context) : base(context)
        {
            _context = context;
        }
    }
}
