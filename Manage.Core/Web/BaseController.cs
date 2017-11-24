using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Manage.Core.SSO;
using Manage.Framework;
using Manage.Framework.Cache.Core;

namespace Manage.Core
{
    public class BaseController : Controller
    {
        /// <summary>
        /// 应用系统配置信息
        /// </summary>
        protected AppConfig appConfig;

        /// <summary>
        /// 异常日志记录器
        /// </summary>
        protected ILogger exceptionLogger = null;

        /// <summary>
        /// 操作日志记录器
        /// </summary>
        protected ILogger operationLogger = null;

        /// <summary>
        /// 登录用户上下文
        /// </summary>
        protected LoginUserContext CurrentUserContext
        {
            get
            {
                return SSOAuthorization.GetCurrentUserContext;
            }
        }

        public BaseController()
        {
            appConfig = ConfigManager.Instance.Single<AppConfig>();

            //日志容器
            switch (appConfig.LogType)
            {
                case 0:
                    exceptionLogger = LoggerFactory.CreateLog(LoggingType.Text, "AppExceptionLogger");
                    operationLogger = LoggerFactory.CreateLog(LoggingType.Text, "AppOperationLogger");
                    break;
                case 1:
                    exceptionLogger = LoggerFactory.CreateLog(LoggingType.TraceSource, "HS_Exception");
                    operationLogger = LoggerFactory.CreateLog(LoggingType.TraceSource, "HS_Operation");
                    break;
                case 3:
                    exceptionLogger = LoggerFactory.CreateLog(LoggingType.AppDatabase);
                    operationLogger = exceptionLogger;
                    break;
            }
            ViewBag.AppConfig = appConfig;
            ViewBag.PageTitle = appConfig.AppName;
        }

        /// <summary>
        /// 重载执行中
        /// </summary>
        /// <param name="filterContext">过滤器上下文</param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!SSOAuthorization.IsLogin)
            {
                var flag = LockLogin(filterContext);
                if (flag)
                {
                    return;
                }
            }

            string areaName = filterContext.RouteData.DataTokens["area"] != null ? filterContext.RouteData.DataTokens["area"].ToString() : "";
            string controllerName = filterContext.RouteData.Values["controller"] != null ? filterContext.RouteData.Values["controller"].ToString() : "";
            string actionName = filterContext.RouteData.Values["action"] != null ? filterContext.RouteData.Values["action"].ToString() : "";

            //访问权限判断

            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// 锁定登录。作用：重载直接return treu可以实现非登录调用Action
        /// </summary>
        protected virtual bool LockLogin(ActionExecutingContext filterContext)
        {
            //根据请求类型区分异常处理
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
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
            else
            {
                filterContext.Result = new JsonResult()
                {
                    Data = "timeOut",
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return true;
        }

        /// <summary>
        /// 重载执行后
        /// </summary>
        /// <param name="filterContext">过滤器上下文</param>
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            //string areaName = filterContext.RouteData.DataTokens["area"] != null ? filterContext.RouteData.DataTokens["area"].ToString() : "";
            //string controllerName = filterContext.RouteData.Values["controller"] != null ? filterContext.RouteData.Values["controller"].ToString() : "";
            //string actionName = filterContext.RouteData.Values["action"] != null ? filterContext.RouteData.Values["action"].ToString() : "";
        }

        /// <summary>
        /// 重载例外
        /// </summary>
        /// <param name="filterContext">过滤器上下文</param>
        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            //日志
            if (appConfig != null && appConfig.LogEnable && exceptionLogger != null)
            {
                var area = filterContext.RouteData.DataTokens["area"] != null ? filterContext.RouteData.DataTokens["area"].ToString() : "";
                var actionName = filterContext.RouteData.Values["action"] != null ? (string)filterContext.RouteData.Values["action"] : "";
                var controller = filterContext.RouteData.Values["controller"] != null ? (string)filterContext.RouteData.Values["controller"] : "";
                string logId = Guid.NewGuid().ToString();
                //if (!string.IsNullOrEmpty(appConfig.LogSort) && appConfig.LogSort.IndexOf("Error") != -1)
                {
                    exceptionLogger.Error(new LogItemModel()
                    {
                        ID = logId,
                        LogTime = DateTime.Now,
                        Module = area,
                        ClassName = controller,
                        MethodName = actionName,
                        Exception = filterContext.Exception,
                        Message = string.Format("系统运行出错，出错信息请查看{0}日志！", filterContext.Exception.Message)
                    });
                }
            }

            //根据请求类型区分异常处理
            //if (!filterContext.HttpContext.Request.IsAjaxRequest())
            //{
            //    //错误消息提示模式：0 发布模式，1 开发模式
            //    if (appConfig.TipMessageMode == 0)
            //    {
            //        filterContext.ExceptionHandled = true;
            //        //跳转进入异常统一处理模块
            //        filterContext.Result = RedirectToAction("Index", "ExceptionProceed",
            //                        new RouteValueDictionary(new Dictionary<string, object>()
            //                    { 
            //                        {"type",filterContext.Exception.GetType()},
            //                        {"message",filterContext.Exception.Message},
            //                    }));
            //    }
            //}
            //else
            //{
            //    filterContext.Result = AjaxError(filterContext.Exception.Message, filterContext);

            //    //设置异常已经处理过。
            //    filterContext.ExceptionHandled = true;
            //}
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="message">日志信息</param>
        protected void WriteLog(string message)
        {
            if (appConfig != null && appConfig.LogEnable && operationLogger != null)
            {
                string strOperatorID = string.Empty;
                string strOperatorName = string.Empty;
                if (CurrentUserContext != null && CurrentUserContext.UserInfo != null)
                {
                    strOperatorID = CurrentUserContext.UserID;
                    strOperatorName = CurrentUserContext.UserInfo.UserDisplayName;
                }
                var log = new LogItemModel()
                {
                    ClassName = ControllerContext.RouteData.Values["controller"].ToString(),
                    MethodName = ControllerContext.RouteData.Values["action"].ToString(),
                    LogTime = ControllerContext.HttpContext.Timestamp,
                    IPAddress = ControllerContext.HttpContext.Request.UserHostAddress,
                    OperaterId = strOperatorID,
                    OperaterName = strOperatorName,
                    Message = message
                };

                operationLogger.Track(log);
            }
        }
    }
}
