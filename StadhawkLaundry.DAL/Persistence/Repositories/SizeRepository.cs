using StadhawkLaundry.BAL.Core.IRepositories;
using StadhawkLaundry.DataModel;
using StadhawkLaundry.DataModel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StadhawkLaundry.BAL.Persistence.Repositories
{
    public class SizeRepository : Repository<TblSize> , ISizeRepository
    {
        private readonly LaundryContext _context;
        public SizeRepository(LaundryContext context) :base(context: context)
        {
            _context = context;
        }
    }
}
