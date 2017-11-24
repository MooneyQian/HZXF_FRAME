using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Manage.Framework;
using Manage.Core.Facades;
using Manage.Core.Models;
using Manage.Core.SSO;
using Manage.Core.Common;

namespace Manage.Core.Controllers
{
    public class AdminController : BaseController
    {
        //延迟加载业务操作器
        Lazy<IAdminFacade> _AdminFacade = new Lazy<IAdminFacade>(() => { return new AdminFacade(); }, true);

        /// <summary>
        /// [操作]重载不锁定登录
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        protected override bool LockLogin(ActionExecutingContext filterContext)
        {
            return false;
        }

        #region 视图

        /// <summary>
        /// 【视图】桌面
        /// </summary>
        /// <returns></returns>
        [SSOAuthorize]
        public ViewResult Index()
        {
            ViewBag.UserName = CurrentUserContext.UserInfo.UserDisplayName;
            return View();
        }

        /// <summary>
        /// 【视图】桌面
        /// </summary>
        /// <returns></returns>
        [SSOAuthorize]
        public ViewResult TIndex()
        {
            ViewBag.UserName = CurrentUserContext.UserInfo.UserDisplayName;
            return View();
        }

        #endregion

        /// <summary>
        /// [视图]登录页面
        /// </summary>
        /// <param name="reurl">登录后自动跳转页面</param>
        /// <returns></returns>
        public virtual ActionResult LogOn(string reurl)
        {
            ViewBag.ReUrl = reurl;
            return View(appConfig.LoginPage);
        }

        /// <summary>
        /// [视图]AJAX登录页面
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxLogin()
        {
            return View("/Views/Account/AjaxLogin.cshtml");
        }

        /// <summary>
        /// [操作]检验登录是否成功
        /// </summary>
        /// <param name="model">登录实体类</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult LogOn(LogOnModel model)
        {
            var result = SSOAuthorization.AdminLogin(model.UserName, model.Password);
            if (result.Status == LoginingStatus.Success)
            {
                //if (string.IsNullOrEmpty(model.ReUrl))
                model.ReUrl = appConfig.IndexAction;

                WriteLog(string.Format("用户名：{0}在{1}成功登录系统！", model.UserName, DateTime.Now.ToString()));
                return Json(AjaxResult.Success(model.ReUrl, "登录成功!"));
            }
            else
            {
                WriteLog(string.Format("用户名：{0}在{1}登录系统失败！原因：{2}", model.UserName, DateTime.Now.ToString(), result.Message));
                return Json(AjaxResult.Error(result.Message));
            }
        }

        /// <summary>
        /// 登录退出
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOff()
        {
            try
            {
                string currentUserId = CurrentUserContext.UserID;
                if (SSOAuthorization.IsLogin)
                {
                    string msg = SSOAuthorization.Logout();

                }
            }
            catch { }
            //单点登录判断
            if (appConfig.SSOEnable && appConfig.SSOType != 1)
            {
                return Redirect(appConfig.SSOServerUrl + "?app_regid=" + appConfig.SSORegisterID + "&action=logoff&app_reurl=" + System.Web.HttpUtility.UrlEncode((HttpContext.Request.Url.ToString() ?? "").ToLower().Replace("/admin/logoff", "")));
            }
            else
            {
                //设置从注销返回登录页面标记
                if (HttpContext.Application.Get("LogoutFlg") == null)
                    HttpContext.Application.Set("LogoutFlg", "true");
                return Redirect(appConfig.LoginAction);
            }
        }
        
        #region 个人中心

        /// <summary>
        /// [视图]修改用户密码
        /// </summary>
        /// <returns></returns>
        [SSOAuthorize]
        public ActionResult ChangePassword()
        {
            var model = new AccountModel()
            {
                UserID = CurrentUserContext.UserID
            };
            return View("/Views/Account/ChangePassword.cshtml", model);
        }

        /// <summary>
        /// [操作]修改用户密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [SSOAuthorize]
        public JsonResult _ChangePassword(AccountModel model)
        {
            if (model.UserID == CurrentUserContext.UserID)
            {
                if (!_AdminFacade.Value.ComparePassword(CurrentUserContext.UserID, model.OldPassword))
                {
                    return Json(AjaxResult.Error("原密码输入错误！"));
                }
                try
                {
                    _AdminFacade.Value.ChangePwd(model.UserID, model.NewPassword);
                }
                catch
                {
                    return Json(AjaxResult.Error("密码修改失败！"));
                }
                return Json(AjaxResult.Success("成功！"));
            }
            return Json(AjaxResult.Error("用户登录信息错误！"));
        }
        #endregion
    }
}
