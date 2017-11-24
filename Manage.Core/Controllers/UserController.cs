using System;
using System.Web.Mvc;
using Manage.Core.Facades;
using Manage.Core.Models;
using Manage.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Manage.Framework;
using Manage.Core.Facades;
using Manage.Core.Models;

namespace Manage.Core.Controllers
{
    public class UserController : BaseController
    {
        #region 定义
        //延迟加载业务处理器
        Lazy<IUserFacade> _UserFacade = new Lazy<IUserFacade>(() => { return SSOFacadeAdapter.UserInstance(); }, true);
        //视图地址
        string _path = "/Views/User/";
        #endregion

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
        /// 【视图】用户列表
        /// </summary>
        /// <returns></returns>
        public ViewResult List()
        {
            //ViewBag.SSOEnable = appConfig.SSOEnable && appConfig.SSOType != 1;
            return View();
        }
        /// <summary>
        /// 【视图】新增用户
        /// </summary>
        /// <returns></returns>
        public ViewResult Add()
        {
            ViewBag.OperType = "Add";
            ViewBag.ActionUrl = "_Add";
            ViewBag.UserTypes = typeof(UserType_ZH).EnumToSelectList(new string[] { UserType_ZH.超级管理员.ToString() });
            User_S model = new User_S();
            return View(model);
        }
        /// <summary>
        /// 【视图】编辑用户
        /// </summary>
        /// <returns></returns>
        public ViewResult Edit(string ID)
        {
            User_S model = _UserFacade.Value.GetUserByID(ID);
            ViewBag.UserTypes = typeof(UserType_ZH).EnumToSelectList(new string[] { UserType_ZH.超级管理员.ToString() });
            ViewBag.OperType = "Edit";
            ViewBag.ActionUrl = "_Edit";

            return View("Add", model);
        }

        /// <summary>
        /// 查看用户
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public ViewResult Show(string UserID)
        {
            User_S model = _UserFacade.Value.GetUserByID(UserID);
            return View(_path + "Show.cshtml", model);
        }
        #endregion

        #region 方法
        /// <summary>
        /// 分页用户数据获取
        /// </summary>
        /// <param name="user">搜索条件</param>
        /// <param name="pi">分页信息</param>
        /// <returns></returns>
        public JsonResult _UserList(User_S user, PageInfo pi)
        {
            var data = _UserFacade.Value.GetUserPaged(user, pi);
            var result = new
            {
                Rows = data,
                Total = pi.Total
            };
            return Json(result);
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult _Add(User_I user)
        {
            try
            {
                if (_UserFacade.Value.AddUser(user))
                    return Json(AjaxResult.Success("用户新增成功，默认密码为：" + appConfig.DefaultPassword));
                else
                    return Json(AjaxResult.Error("用户新增失败！"));
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.Error(ex.Message));
            }
        }

        /// <summary>
        /// 编辑用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult _Edit(User_U user)
        {
            try
            {
                if (_UserFacade.Value.EditUser(user))
                    return Json(AjaxResult.Success("用户修改成功！"));
                else
                    return Json(AjaxResult.Error("用户修改失败！"));
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.Error(ex.Message));
            }
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public JsonResult _Delete(string ids)
        {
            try
            {
                if (_UserFacade.Value.DelUsers(ids))
                    return Json(AjaxResult.Success("用户删除成功！"));
                else
                    return Json(AjaxResult.Error("用户删除失败！"));
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.Error(ex.Message));
            }
        }

        /// <summary>
        /// 判断登录名是否被占用
        /// </summary>
        /// <param name="LoginName"></param>
        /// <returns></returns>
        public JsonResult _IsExists(string LoginName)
        {
            return Json(_UserFacade.Value.IsExists(LoginName));
        }

        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public JsonResult _ResetPwd(string UserID)
        {
            try
            {
                _UserFacade.Value.ChangePwd(UserID, appConfig.DefaultPassword);
                return Json(AjaxResult.Success("密码重置成功，默认密码为：" + appConfig.DefaultPassword));
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.Error("重置密码失败，原因：" + ex.Message));
            }
        }
        #endregion

        #region 扩展
        #endregion
    }
}
