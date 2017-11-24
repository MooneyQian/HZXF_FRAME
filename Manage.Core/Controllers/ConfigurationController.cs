using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Manage.Framework;
using Manage.Core;
using Manage.Core.Models;
using Manage.Open;
using Business.Controller.Common;
using Manage.Core.Facades;

namespace Manage.Core.Controllers
{
    public class ConfigurationController : BaseController
    {
        #region 定义
        //延迟加载业务处理器
        Lazy<IConfigurationFacade> _ConfigurationFacade = new Lazy<IConfigurationFacade>(() => { return new ConfigurationFacade(); }, true);

        #endregion

        #region 视图
        /// <summary>
        /// 【视图】设置页面
        /// </summary>
        /// <returns></returns>
        public ViewResult Add()
        {
            ViewBag.ActionUrl = "_SaveAll";
            var userid = CurrentUserContext.UserID.ToString();
            var data = _ConfigurationFacade.Value.GetConfigurationByUserId(userid);
            ViewBag.UserInfo = CurrentUserContext.UserInfo;
            return View("/Views/UserCenter/UserCenter.cshtml", data);
        }


        #endregion

        #region 方法

        //<summary>
        // 根据当前登陆用户获取用户配置信息
        /// 如果没有配置信息则 创建一天空的配置信息
        //</summary>
        //<param name="OID"></param>
        //<returns></returns>
        public JsonResult _Add()
        {
            //获取当前登陆的用户id
            var userid = CurrentUserContext.UserID.ToString();
            var data = _ConfigurationFacade.Value.GetConfigurationByUserId(userid);
            var result = new
            {
                Rows = data,

            };
            return Json(result);
        }

        //保存全部
        public JsonResult _SaveAll(string mystr)
        {           
            Configuration_U model = new Configuration_U();
            model.SETTEXT = mystr;
            model.USERID = CurrentUserContext.UserID.ToString();

            if (_ConfigurationFacade.Value.UpdateConfiguration(model))
            {
                return Json(AjaxResult.Success(mystr, "设置成功"));
            }
            return Json(AjaxResult.Success("设置失败"));
            
        }


        #endregion

        #region 扩展
        #endregion
    }
}
