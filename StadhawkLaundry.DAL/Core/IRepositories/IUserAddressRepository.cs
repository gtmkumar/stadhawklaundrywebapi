﻿using StadhawkCoreApi;
using StadhawkLaundry.DataModel.Models;
using StadhawkLaundry.ViewModel.RequestModel;
using StadhawkLaundry.ViewModel.ResponseModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StadhawkLaundry.BAL.Core.IRepositories
{

    public interface IUserAddressRepository : IRepository<TblUserAddress>
    {
        Task<ApiResult<IEnumerable<UserAddressResponseViewModel>>> UserAddress(string UserId, int? addressId = null);
        Task<ApiResult<bool>> SaveUserAddress(UserAddressRequestViewModel userAddress, string userId);
        Task<ApiResult<bool>> UpdateUserAddress(UserAddressRequestViewModel userAddress, string userId);
    }
}
