using StadhawkCoreApi;
using StadhawkLaundry.BAL.Core.IRepositories;
using StadhawkLaundry.ViewModel.ResponseModel;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using StadhawkLaundry.DataModel.Models;
using StadhawkLaundry.DataModel;
using StadhawkCoreApi.Logger;

namespace StadhawkLaundry.BAL.Persistence.Repositories
{
    public class NavigatorRepository : Repository<NavigatorView>, INavigatorRepository
    {

        private readonly LaundryContext _context;
        public NavigatorRepository(LaundryContext context) : base(context)
        {
            _context = context;
        }

        public ApiResult<NavigatorResponseModel> GetNavigation(int RoleID, string URL)
        {
            #region Create Main Menu
            NavigatorResponseModel objNavigator = new NavigatorResponseModel();
            var Result = _context.NavigatorViews.Where(M => M.RoleId == RoleID && M.Parant_Id == null && M.IsDefault == false && M.ishide == false).OrderBy(M => M.Default_Order).ToList();
            foreach (var R in Result)
            {
                Nav objnav = new Nav()
                {
                    URL = R.URL,
                    DisplayText = R.Display_Text,
                    IsSubMenu = true,
                    ClassName = string.IsNullOrEmpty(R.Class) ? "Bsddef" : R.Class,
                    Icon = string.IsNullOrEmpty(R.icon) ? "img-Not-Found.png" : R.icon,
                };
                objNavigator.Menu.Add(objnav);

            }
            #endregion
            #region Create Sub Menu
            var Parant_id = (from parant in _context.NavigatorViews where parant.URL == URL && parant.Parant_Id != null select parant.Parant_Id).Take(1).ToList();  //context.NavigatorViews.Where(M => M.URL == URL).ToList();
            if (Parant_id.Count > 0)
            {
                int Parantid = Parant_id[0].Value;
                var Value = _context.NavigatorViews.Where(M => (M.RoleId > 0 || M.RoleId == RoleID) && (M.Parant_Id.Value == Parantid || M.Navigator_Id == Parantid) && M.ishide == false).OrderBy(M => M.Default_Order).ToList();
                foreach (var v in Value)
                {
                    Nav objnav = new Nav()
                    {
                        URL = v.URL,
                        DisplayText = v.Display_Text,
                        IsSubMenu = v.Parant_Id.HasValue,
                        ClassName = string.IsNullOrEmpty(v.Class) ? "Bsddef" : v.Class,
                        Icon = string.IsNullOrEmpty(v.icon) ? "img-Not-Found.png" : v.icon,
                    };
                    objNavigator.SubMenu.Add(objnav);

                }

            }
            #endregion
            return new ApiResult<NavigatorResponseModel>(new ApiResultCode(ApiResultType.Success), objNavigator);
        }
        //public ApiResultCode AddMenuNavigation(string empCode, int EmpId, string roleId, int userId, bool isEdit)
        //{
        //    try
        //    {
        //            //update menu if role changed
        //            if (isEdit)
        //            {
        //                var Role = context.tblEmployeeMasters.Where(m => m.EmpId == EmpId).Select(m => m.RoleName).FirstOrDefault();
        //                if (Role != roleName)
        //                {
        //                    _context.tblEmployeeRightsDetails.RemoveRange(context.tblEmployeeRightsDetails.Where(m => m.EmployeeId == empCode));
        //                    _context.tblPageRights.RemoveRange(context.tblPageRights.Where(m => m.EmployeeCode == empCode));
        //                    _context.SaveChanges();
        //                }
        //                else
        //                {
        //                    return new ApiResultCode(ApiResultType.Success, 1, "Menu updation not needed.");
        //                }
        //            }
        //            Guid roleId = context.AspNetRoles.Where(m => m.Name.Equals(roleName)).Select(m => m.Id).FirstOrDefault();
        //            var navIdPM = context.NavigatorViews.Where(m => !m.ishide.Value && m.RoleId == roleId && m.PortalType == 2).Select(m => m.Navigator_Id).ToList();
        //            var navIdHR = context.NavigatorViews.Where(m => !m.ishide.Value && m.RoleId == roleId && m.PortalType == 1).Select(m => m.Navigator_Id).ToList();

        //            tblEmployeeRightsDetail obj;
        //            tblPageRights objHR;
        //            foreach (var id in navIdPM)
        //            {
        //                obj = new tblEmployeeRightsDetail();
        //                obj.ID = Guid.NewGuid();
        //                obj.NavigatorId = id;
        //                obj.EmployeeId = empCode;
        //                obj.CreatedBy = userId;
        //                obj.CreatedOn = DateTime.Now;
        //                _context.tblEmployeeRightsDetails.Add(obj);
        //            }
        //            foreach (var id in navIdHR)
        //            {
        //                objHR = new tblPageRights();
        //                objHR.EmployeeCode = empCode;
        //                objHR.NavigatorId = id;
        //                objHR.InsertedBy = new Guid(userId);
        //                objHR.InsertDate = DateTime.Now;
        //                objHR.Status = true;
        //                _context.tblPageRights.Add(objHR);
        //            }
        //            _context.SaveChanges();
                
        //        return new ApiResultCode(ApiResultType.Success, 1, "Menu added successfully.");
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorTrace.Logger(LogArea.BusinessTier, ex);
        //        return new ApiResultCode(ApiResultType.Error, 0, "Error in adding Menu.");
        //    }
        //}

        //public ApiResult<bool> IsAuthorize(string id, string URL)
        //{
        //    bool res = false;
        //    var Result = (from pr in _context.tblPageRights
        //                  join nv in _context.TblNavigator on pr.NavigatorId equals nv.NavigatorId
        //                  where pr.EmployeeCode == strEmpCode && nv.URL == URL
        //                  select nv
        //                  ).ToList();
           
        //    if (Result != null)
        //    {
        //        if (Result.Count() > 0 || res)
        //            return new ApiResult<bool>(new ApiResultCode(ApiResultType.Success), true);
        //        else
        //            return new ApiResult<bool>(new ApiResultCode(ApiResultType.Error), false);
        //    }
        //    else
        //    {
        //        return new ApiResult<bool>(new ApiResultCode(ApiResultType.Error), false);
        //    }
        //}
    }
}
