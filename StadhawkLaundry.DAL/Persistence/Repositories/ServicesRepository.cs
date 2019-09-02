using StadhawkLaundry.BAL.Core.IRepositories;
using StadhawkLaundry.DataModel;
using StadhawkLaundry.DataModel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StadhawkLaundry.BAL.Persistence.Repositories
{
    public class ServicesRepository : Repository<TblService>, IServicesRepository
    {
        private LaundryContext _context;
        public ServicesRepository(LaundryContext context) : base(context)
        {
            _context = context;
        }
    }
}
