using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Manage.Framework;
using Manage.Core.Facades;

namespace Manage.Core.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            //跳转管理员桌面菜单
            if (CurrentUserContext.UserType == UserType.Administrators && appConfig.HomeStyle == (int)HomeStyle.桌面)
                return Redirect("/Admin/Index");
            //跳转管理员桌面菜单
            if (CurrentUserContext.UserType == UserType.Administrators && appConfig.HomeStyle == (int)HomeStyle.传统)
                return Redirect("/Admin/TIndex");
            //跳转到用户自定义首页
            if (appConfig.IndexAction != "/Home/Index")
                return Redirect(appConfig.IndexAction);
            ViewBag.UserName = CurrentUserContext.UserInfo.UserDisplayName;//用户名称
            ViewBag.IsAdmin = CurrentUserContext.UserType == UserType.Administrators;//是否管理员
            ViewBag.LogoUrl = appConfig.LogoUrl;//logo地址
            ViewBag.CompanyName = appConfig.CompanyName;//logo地址
            ViewBag.Copyright = appConfig.Copyright; //底部版权文字
            ViewBag.AppName = appConfig.AppName;//应用名称
            ViewBag.Version = appConfig.Version;//应用版本

            ViewBag.AppConfig = appConfig;
            //跳转系统下拉框
            if (appConfig.SSOEnable)
                ViewBag.AppRegs = SSOFacadeAdapter.AppRegisterInstance().GetAppRegisterByUserID(CurrentUserContext.UserID)
                    .Where(c => c.AppRegisterID != appConfig.SSORegisterID)
                    .Select(s => new SelectListItem() { Text = s.AppName, Value = s.HomePageUrl }).ToList();

            return View(appConfig.IndexPage);
        }

        /// <summary>
        /// 获取菜单Json
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetMenuTree()
        {
            var tree = CurrentUserContext.UserMenus.Select(s => new
            {
                id = s.ID,
                pId = s.PerMenuID,
                name = s.MenuName,
                title = s.MenuDesc,
                menucode = s.MenuCode,
                menupath = s.MenuPath,
                order = s.MenuOrder,
                level = s.MenuLevel,
                menuicon = s.MenuIcon  
            });
            return Json(AjaxResult.Success(tree));
        }
    }
}
