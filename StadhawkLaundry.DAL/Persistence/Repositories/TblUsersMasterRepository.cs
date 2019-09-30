using StadhawkLaundry.BAL.Core.IRepositories;
using StadhawkLaundry.DataModel;
using StadhawkLaundry.DataModel.Models;

namespace StadhawkLaundry.BAL.Persistence.Repositories
{
    public class TblUsersMasterRepository : Repository<TblUsersMaster>, ITblUsersMasterRepository
    {
        private readonly LaundryContext _context;
        public TblUsersMasterRepository(LaundryContext context) : base(context)
        {
            _context = context;
        }
    }
}
