using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Manage.Framework;
using Manage.Core.Facades;
using Manage.Core.Models;

namespace Manage.Core.Controllers
{
    public class AppSettingController : BaseController
    {
        #region 定义
        //延迟加载业务处理器
        //Lazy<IRoleFacade> _RoleFacade = new Lazy<IRoleFacade>(() => { return new RoleFacade(); }, true);
        //视图地址
        string _path = "/Views/AppSetting/";
        #endregion

        #region 视图

        /// <summary>
        /// 【视图】设置页面
        /// </summary>
        /// <returns></returns>
        public ViewResult Setting()
        {
            ViewBag.ActionUrl = "_Save";
            return View(appConfig);
        }

        #endregion

        #region 方法
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult _Save(AppConfig config)
        {
            try
            {
                ConfigManager.Instance.SaveConfig<AppConfig>(config);
                return Json(AjaxResult.Success("设置成功，重新登录后生效！部分功能需要重新启动网站后生效。"));
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.Error(ex.Message));
            }
        }

        #endregion

        #region 扩展
        #endregion
    }
}
