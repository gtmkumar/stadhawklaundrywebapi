using StadhawkCoreApi;
using StadhawkLaundry.BAL.Core.IRepositories;
using StadhawkLaundry.ViewModel.ResponseModel;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace StadhawkLaundry.BAL.Persistence.Repositories
{
    public class NavigatorRepository : INavigatorRepository
    {
        public ApiResult<string> GetDefaultUrl(Guid RoleID, int pathSelection)
        {
            string DefultURL = "";
            using (var context = new PersonalisationContext())
            {

                // var projectType = System.Configuration.ConfigurationManager.AppSettings["ProjectType"];
                //  byte validProjectType = 0;
                // byte.TryParse(projectType, out validProjectType);

                var Result = context.NavigatorViews.FirstOrDefault(M => M.RoleId == RoleID && M.IsDefault == true && M.PortalType == pathSelection);
                if (Result != null)
                {
                    DefultURL = Result.URL;
                }
            }
            return new ApiResult<string>(new ApiResultCode(ApiResultType.Success), DefultURL);
        }

        public ApiResult<NavigatorResponseModel> GetMenuNavigation(Guid RoleID, string employeeID, string empCode)
        {
            using (var context = new PersonalisationContext())
            {
                byte validProjectType = 0;
                byte.TryParse(projectType, out validProjectType);
                var result = context.NavigatorViews.Where(t => t.RoleId == RoleID && t.ishide == false && t.PortalType == validProjectType && t.Display_Text.Contains("dashboard")).OrderBy(m => m.Default_Order).ToList();
                if (result == null)
                {
                    return new ApiResult<NavigatorResponseModel>(new ApiResultCode(ApiResultType.Error, 0, "No menu found for your role id."));
                }
                NavigatorResponseModel objNavigator = new NavigatorResponseModel();
                var submenu = result.Where(t => t.Parant_Id.HasValue).GroupBy(t => t.Parant_Id.Value);

                foreach (var item in result)
                {
                    if (!item.Parant_Id.HasValue)
                    {
                        Nav nav = new Nav();

                        nav.Id = item.Navigator_Id;
                        nav.URL = item.URL;
                        nav.ClassName = item.Class;
                        nav.DisplayText = item.Display_Text;
                        nav.IsSubMenu = false;
                        if (submenu != null)
                        {
                            var _object = submenu.FirstOrDefault(t => t.Key == item.Navigator_Id);
                            if (_object != null)
                                foreach (var sub in _object)
                                {
                                    nav.SubMenu.Add(new Nav { Id = sub.Navigator_Id, URL = sub.URL, ClassName = sub.Class, DisplayText = sub.Display_Text, ParentId = sub.Parant_Id.Value });
                                }
                        }
                        objNavigator.Menu.Add(nav);
                    }
                }
                using (var M_context = new MIS_DbContext())
                {

                    var CurrentUserName = M_context.AspNetUsers.Where(x => x.Id.ToString().ToLower() == employeeID).Select(x => x.UserName).FirstOrDefault();

                    var ManagerId = (from t in M_context.tblEmployeeMasters
                                     join t2 in M_context.tblTeamTypes on t.Team_Id equals t2.Team_Id
                                     where t.EmpId == new Guid(employeeID)
                                     select new
                                     {
                                         ManagerId = t2.ReportinEmpId
                                     }.ManagerId).FirstOrDefault();

                    var navigatoreIds = (from t in M_context.tblPageRights
                                         where t.EmployeeCode == CurrentUserName
                                         select new
                                         {
                                             NavigatorId = t.NavigatorId,
                                         }.NavigatorId).ToList();

                    var mynewresult = context.tblNavigators.Where(t => navigatoreIds.Contains(t.Navigator_Id) && t.ishide == false && t.PortalType == validProjectType && navigatoreIds.Contains(t.Navigator_Id)).OrderBy(m => m.Default_Order).ToList();


                    if (mynewresult.Count > 0)
                    {
                        var mynewsubmenu = mynewresult.Where(t => t.Parant_Id.HasValue && navigatoreIds.Contains(t.Navigator_Id)).GroupBy(t => t.Parant_Id.Value);

                        foreach (var item in mynewresult)
                        {
                            if (!item.Parant_Id.HasValue)
                            {
                                var counter = objNavigator.Menu.Where(x => x.Id == item.Navigator_Id).Count();
                                if (counter == 0)
                                {

                                    Nav nav = new Nav();

                                    nav.Id = item.Navigator_Id;
                                    nav.URL = item.URL;
                                    nav.ClassName = item.Class;
                                    nav.DisplayText = item.Display_Text;
                                    nav.IsSubMenu = false;
                                    if (mynewsubmenu != null)
                                    {
                                        var _object = mynewsubmenu.FirstOrDefault(t => t.Key == item.Navigator_Id);
                                        if (_object != null)
                                            foreach (var sub in _object)
                                            {
                                                nav.SubMenu.Add(new Nav { Id = sub.Navigator_Id, URL = sub.URL, ClassName = sub.Class, DisplayText = sub.Display_Text, ParentId = sub.Parant_Id.Value });
                                            }
                                    }

                                    objNavigator.Menu.Add(nav);
                                }
                                else
                                {
                                    var selectsubmenu = objNavigator.Menu.Where(x => x.Id == item.Navigator_Id);
                                    var idx = objNavigator.Menu.Where(x => x.Id == item.Navigator_Id);

                                    var _object = mynewsubmenu.FirstOrDefault(t => t.Key == item.Navigator_Id);
                                    if (_object != null)
                                    {
                                        for (int k = 0; k < objNavigator.Menu.Count; k++)
                                        {
                                            if (objNavigator.Menu[k].Id.ToString().ToLower() == item.Navigator_Id.ToString().ToLower())
                                            {
                                                foreach (var sub in _object)
                                                {
                                                    var Submenucounter = objNavigator.Menu[k].SubMenu.Where(x => x.Id.ToString().ToLower() == sub.Navigator_Id.ToString().ToLower()).Count();
                                                    if (Submenucounter == 0)
                                                    {
                                                        objNavigator.Menu[k].SubMenu.Add(new Nav { Id = sub.Navigator_Id, URL = sub.URL, ClassName = sub.Class, DisplayText = sub.Display_Text, ParentId = sub.Parant_Id.Value });
                                                    }
                                                }
                                            }

                                        }

                                    }
                                }

                            }
                        }
                    }
                }


                return new ApiResult<NavigatorModel>(new ApiResultCode(ApiResultType.Success), objNavigator);
            }
        }

        public ApiResult<bool> IsAuthorize(string strEmpCode, string URL)
        {
            using (var context = new MIS_DbContext())
            {
                bool res = false;
                var Result = (from pr in context.tblPageRights
                              join nv in context.tblNavigators on pr.NavigatorId equals nv.Navigator_Id
                              where pr.EmployeeCode == strEmpCode && nv.URL == URL
                              select nv
                              ).ToList();
                try
                {
                    res = context.tblDefaulUrl.Any(t => t.DefaultUrl == URL);
                }
                catch (Exception ex)
                {
                    throw;
                }

                if (Result != null)
                {
                    if (Result.Count() > 0 || res)
                        return new ApiResult<bool>(new ApiResultCode(ApiResultType.Success), true);
                    else
                        return new ApiResult<bool>(new ApiResultCode(ApiResultType.Error), false);
                }
                else
                {
                    return new ApiResult<bool>(new ApiResultCode(ApiResultType.Error), false);
                }
            }
        }
    }
}
