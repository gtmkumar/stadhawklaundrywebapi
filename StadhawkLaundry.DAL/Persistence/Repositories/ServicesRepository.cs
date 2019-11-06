using StadhawkCoreApi;
using StadhawkCoreApi.Logger;
using StadhawkLaundry.BAL.Core.IRepositories;
using StadhawkLaundry.DataModel;
using StadhawkLaundry.DataModel.Models;
using StadhawkLaundry.ViewModel.ResponseModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
namespace StadhawkLaundry.BAL.Persistence.Repositories
{
    public class ServicesRepository : Repository<TblServiceMaster>, IServicesRepository
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

        public async Task<ApiResult<IEnumerable<ServiceMasterResponseViewModel>>> GetServiceByStore(int storeId)
        {
            List<ServiceMasterResponseViewModel> listitems = new List<ServiceMasterResponseViewModel>();
            try
            {
                SqlParameter StoreId = new SqlParameter("@StoreId", System.Data.SqlDbType.Int) { Value = storeId };
                var ietms = _context.ExecuteStoreProcedure("[GetServiceByStore]", StoreId);

                if (ietms.Tables[0].Rows.Count > 0)
                {
                    listitems = (from DataRow dr in ietms.Tables[0].Rows
                                 select new ServiceMasterResponseViewModel()
                                 {
                                     ServiceId = (dr["ServiceId"] != DBNull.Value) ? Convert.ToInt32(dr["ServiceId"]) : 0,
                                     ServiceName = (dr["ServiceName"] != DBNull.Value) ? Convert.ToString(dr["ServiceName"]) : string.Empty,
                                     ServiceTypeId = (dr["ServiceTypeId"] != DBNull.Value) ? Convert.ToInt32(dr["ServiceTypeId"]) : 0,
                                     ServiceUrl = (dr["ServiceUrl"] != DBNull.Value) ? Convert.ToString(dr["ServiceUrl"]) : string.Empty
                                 }).ToList();
                }
                return listitems.Count < 0
                            ? new ApiResult<IEnumerable<ServiceMasterResponseViewModel>>(new ApiResultCode(ApiResultType.Error, 1, "No data in given request"))
                            : new ApiResult<IEnumerable<ServiceMasterResponseViewModel>>(new ApiResultCode(ApiResultType.Success), listitems);
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.BusinessTier, ex);
                return new ApiResult<IEnumerable<ServiceMasterResponseViewModel>>(new ApiResultCode(ApiResultType.ExceptionDuringOpration, 3, "Please contact system administrator"));
            }
        }

        public async Task<ApiResult<ServiceByKgResponseViewModel>> GetServiceByKg(int storeId,int serviceId)
        {
            ServiceByKgResponseViewModel item = null;
            try
            {
                SqlParameter StoreId = new SqlParameter("@StoreId", System.Data.SqlDbType.Int) { Value = storeId };
                SqlParameter ServiceId = new SqlParameter("@ServiceId", System.Data.SqlDbType.Int) { Value = serviceId };
                var ietms = _context.ExecuteStoreProcedure("[usp_getservicebykg]", StoreId, ServiceId);

                if (ietms.Tables[0].Rows.Count > 0)
                {
                    item = (from DataRow dr in ietms.Tables[0].Rows
                                 select new ServiceByKgResponseViewModel()
                                 {
                                     StoreId = (dr["StoreId"] != DBNull.Value) ? Convert.ToInt32(dr["StoreId"]) : 0,
                                     ServiceId = (dr["ServiceId"] != DBNull.Value) ? Convert.ToInt32(dr["ServiceId"]) : 0,
                                     ServiceName = (dr["ServiceName"] != DBNull.Value) ? Convert.ToString(dr["ServiceName"]) : string.Empty,
                                     pricePerKG = (dr["pricePerKG"] != DBNull.Value) ? Convert.ToDecimal(dr["pricePerKG"]) : 0,
                                     QuantityInKG = (dr["QuantityInKG"] != DBNull.Value) ? Convert.ToDecimal(dr["QuantityInKG"]) : 0,
                                     ServiceImageUrl = (dr["ServiceImageUrl"] != DBNull.Value) ? Convert.ToString(dr["ServiceImageUrl"]) : string.Empty,
                                     CartId = (dr["CartId"] != DBNull.Value) ? Convert.ToInt32(dr["CartId"]) : 0,
                                 }).FirstOrDefault();
                }
                return item == null
                            ? new ApiResult<ServiceByKgResponseViewModel>(new ApiResultCode(ApiResultType.Error, 1, "No data in given request"))
                            : new ApiResult<ServiceByKgResponseViewModel>(new ApiResultCode(ApiResultType.Success), item);
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.BusinessTier, ex);
                return new ApiResult<ServiceByKgResponseViewModel>(new ApiResultCode(ApiResultType.ExceptionDuringOpration, 3, "Please contact system administrator"));
            }
        }
    }
}
