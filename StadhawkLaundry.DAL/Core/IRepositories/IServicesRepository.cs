using StadhawkCoreApi;
using StadhawkLaundry.DataModel.Models;
using StadhawkLaundry.ViewModel.ResponseModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StadhawkLaundry.BAL.Core.IRepositories
{
    public interface IServicesRepository: IRepository<TblServiceMaster>
    {
        Task<ApiResult<IEnumerable<ServiceLabelMasterResponseViewModel>>> GetServiceMaster(int customerId);
        Task<ApiResult<IEnumerable<ServiceMasterResponseViewModel>>> GetServiceByStore(int storeId);
        Task<ApiResult<IEnumerable<ServiceByKgResponseViewModel>>> GetServiceByKg(int storeId);
    }
}
