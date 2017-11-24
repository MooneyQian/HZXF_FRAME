using Manage.Core.SSO;
using Manage.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Manage.Core
{
    public class SSOAuthorize : AuthorizeAttribute
    {
        /// <summary>
        /// 应用系统配置信息
        /// </summary>
        protected AppConfig appConfig = ConfigManager.Instance.Single<AppConfig>();

        /// <summary>
        /// 判断用户登录
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            var flag = SSOAuthorization.IsLogin;
            if(!flag)
            {
                 httpContext.Response.StatusCode = 403; 
            }
            return flag;
        }

        
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            if (filterContext.HttpContext.Response.StatusCode == 403 )
            {
                //如果没有登陆，则转向到登陆页面
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.HttpContext.Response.StatusCode = 200;
                    filterContext.Result = new JsonResult() { Data = "timeOut", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else
                {
                    //判断是否为单点登录
                    if (appConfig.SSOEnable && appConfig.SSOType != 1)
                    {
                        filterContext.Result = new RedirectResult(appConfig.SSOServerUrl + "?app_regid=" + appConfig.SSORegisterID + "&app_reurl=" + System.Web.HttpUtility.UrlEncode(filterContext.RequestContext.HttpContext.Request.Url.ToString() ?? ""));
                    }
                    else
                    {
                        filterContext.Result = new RedirectResult(appConfig.LoginAction + "?reurl=" + System.Web.HttpUtility.UrlEncode(filterContext.RequestContext.HttpContext.Request.Url.ToString() ?? ""));
                    }
               }
            }
            else
            {
                string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                string actionName = filterContext.ActionDescriptor.ActionName;
                //判断当前用户是否有权限访问该功能
                
            }
        }
    }
}
