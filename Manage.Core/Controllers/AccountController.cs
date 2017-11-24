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
using System.Web;

namespace Manage.Core.Controllers
{
    public class AccountController : BaseController
    {
        //延迟加载业务操作器
        Lazy<IUserFacade> _UserFacade = new Lazy<IUserFacade>(() => { return SSOFacadeAdapter.UserInstance(); }, true);

        /// <summary>
        /// [操作]重载不锁定登录
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        protected override bool LockLogin(ActionExecutingContext filterContext)
        {
            return false;
        }

        /// <summary>
        /// [视图]登录页面
        /// </summary>
        /// <param name="reurl">登录后自动跳转页面</param>
        /// <returns></returns>
        public virtual ActionResult LogOn(string reurl)
		{
			bool flag = false;
			if (base.HttpContext.Application.Get("LogoutFlg") != null)
			{
				flag = base.HttpContext.Application.Get("LogoutFlg").Convert(false);
				base.HttpContext.Application.Remove("LogoutFlg");
			}
			ActionResult result;
			if (this.appConfig.SSOEnable && this.appConfig.SSOType != 1)
			{
				result = this.Redirect(string.Concat(new string[]
				{
					this.appConfig.SSOServerUrl,
					"?app_regid=",
					this.appConfig.SSORegisterID,
					"&app_reurl=",
					HttpUtility.UrlEncode((base.HttpContext.Request.Url.ToString() ?? "").ToLower().Replace("/account/logon", "")),
					flag ? "&action=logoff" : ""
				}));
			}
			else
			{
				result = base.View(this.appConfig.LoginPage);
			}
			return result;
		}

        /// <summary>
        /// [视图]AJAX登录页面
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxLogin()
        {

            #region 单点登录方式

            if (appConfig.SSOEnable && appConfig.SSOType != 1)
            {
                if (!SSOAuthorization.IsLogin)
                {
                    var serUrl = "http://" + Request.Url.Authority + ((Request.ApplicationPath != null && Request.ApplicationPath != "/") ? Request.ApplicationPath : "");
                    return Redirect(appConfig.SSOServerUrl + "?app_regid=" + appConfig.SSORegisterID + "&app_reurl=" + System.Web.HttpUtility.UrlEncode(serUrl + "/Account/AjaxLogin"));
                }
                else
                {
                    ViewBag.SSOAjaxLogin = "success";
                }
            }
            #endregion

            return View();
        }

        /// <summary>
        /// [操作]检验登录是否成功
        /// </summary>
        /// <param name="model">登录实体类</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult LogOn(LogOnModel model)
        {
            var result = SSOAuthorization.Login(model.UserName, model.Password);
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
                return Redirect(appConfig.SSOServerUrl + "?app_regid=" + appConfig.SSORegisterID + "&action=logoff");
            }
            else
            {
                //设置从注销返回登录页面标记
                if (HttpContext.Application.Get("LogoutFlg") == null)
                    HttpContext.Application.Set("LogoutFlg", "true");
                return Redirect(appConfig.LoginAction);
            }
        }


        /// <summary>
        /// 单点登录验证中心登录验证入口
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="app_reurl"></param>
        /// <returns></returns>
        public ActionResult SSOLoginVerified(string ticket, string app_reurl)
        {
            try
            {
                var result = SSOAuthorization.SSOLogin(ticket);
                if (result.Status == LoginingStatus.Success)
                {
                    WriteLog(string.Format("用户名：{0}在{1}成功登录系统！", CurrentUserContext.UserInfo.UserDisplayName, DateTime.Now.ToString()));

                    if (string.IsNullOrEmpty(app_reurl))
                        return Redirect(appConfig.IndexAction);
                    else
                        return Redirect(HttpUtility.UrlDecode(app_reurl));
                }
                else
                {
                    WriteLog(string.Format("票据：{0}在{1}登录系统失败！原因：{2}", ticket, DateTime.Now.ToString(), result.Message));
                    return Content("<script>alert('" + result.Message + "'); location.href='" + appConfig.SSOServerUrl + "?app_regid=" + appConfig.SSORegisterID + "&action=logoff&app_reurl=" + HttpUtility.UrlEncode(app_reurl) + "';</script>");
                }
            }
            catch (Exception ex)
            {
                WriteLog(string.Format("票据：{0}在{1}登录系统失败！原因：{2}", ticket, DateTime.Now.ToString(), ex.Message));
                return Content("<script>alert('系统错误，登录跳转失败，错误信息请查看日志文件！'); location.href='" + appConfig.SSOServerUrl + "?app_regid=" + appConfig.SSORegisterID + "&action=logoff&app_reurl=" + HttpUtility.UrlEncode(app_reurl) + "';</script>");
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
            ViewBag.ActionUrl = "_ChangePassword";
            return View(model);
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
                if (!_UserFacade.Value.ComparePassword(CurrentUserContext.UserID, model.OldPassword))
                {
                    return Json(AjaxResult.Error("原密码输入错误！"));
                }
                try
                {
                    _UserFacade.Value.ChangePwd(model.UserID, model.NewPassword);
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
