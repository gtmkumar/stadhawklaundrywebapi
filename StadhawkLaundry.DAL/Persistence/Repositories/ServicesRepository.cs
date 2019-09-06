using StadhawkCoreApi;
using StadhawkCoreApi.Logger;
using StadhawkLaundry.BAL.Core.IRepositories;
using StadhawkLaundry.DataModel;
using StadhawkLaundry.DataModel.Models;
using StadhawkLaundry.ViewModel.ResponseModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace StadhawkLaundry.BAL.Persistence.Repositories
{
    public class ServicesRepository : Repository<TblService>, IServicesRepository
    {
        private LaundryContext _context;
        public ServicesRepository(LaundryContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ApiResult<IEnumerable<ServiceLabelMasterResponseViewModel>>> GetServiceMaster(int customerId)
        {
            ServiceLabelMasterResponseViewModel service = null;
            List<ServiceLabelMasterResponseViewModel> serviceList = new List<ServiceLabelMasterResponseViewModel>();
            try
            {
                SqlParameter Customerid = new SqlParameter("@CustomerId", System.Data.SqlDbType.Int) { Value = customerId };
                var result = _context.ExecuteStoreProcedure("GetServiceMaster", Customerid);
                if (result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
                {
                    foreach (System.Data.DataRow row in result.Tables[0].Rows)
                    {
                        service = new ServiceLabelMasterResponseViewModel
                        {
                            Id = (row["Id"] != DBNull.Value) ? Convert.ToInt32(row["Id"]) : 0,
                            LabelName = (row["LabelName"] != DBNull.Value) ? Convert.ToString(row["LabelName"]) : string.Empty
                        };

                        if (result.Tables.Count > 0 && result.Tables[1].Rows.Count > 0)
                        {
                            foreach (System.Data.DataRow row1 in result.Tables[1].Rows)
                            {
                                if (Convert.ToString(row["Id"]) == Convert.ToString(row1["ServiceTypeId"]))
                                {
                                    service.ServiceMaster.Add(new ServiceMasterResponseViewModel
                                    {
                                        ServiceId = (row1["ServiceId"] != DBNull.Value) ? Convert.ToInt32(row1["ServiceId"]) : 0,
                                        ServiceName = (row1["ServiceName"] != DBNull.Value) ? Convert.ToString(row1["ServiceName"]) : string.Empty,
                                        ServiceTypeId = (row1["ServiceTypeId"] != DBNull.Value) ? Convert.ToInt32(row1["ServiceTypeId"]) : 0,
                                        ServiceUrl = (row1["ServiceUrl"] != DBNull.Value) ? Convert.ToString(row1["ServiceUrl"]) : string.Empty
                                    });
                                }
                            }
                        }
                        serviceList.Add(service);
                    }
                }

                if (service != null)
                    return new ApiResult<IEnumerable<ServiceLabelMasterResponseViewModel >> (new ApiResultCode(ApiResultType.Success), serviceList);
            }
            catch (Exception ex)
            {
                StadhawkCoreApi.Logger.ErrorTrace.Logger(LogArea.ApplicationTier, ex);
            }
            return new ApiResult<IEnumerable<ServiceLabelMasterResponseViewModel>>(new ApiResultCode(ApiResultType.Error), serviceList);
        }
    }
}
