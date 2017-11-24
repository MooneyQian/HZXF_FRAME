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
    public class RoleController : BaseController
    {
        #region 定义
        //延迟加载业务处理器
        Lazy<IRoleFacade> _RoleFacade = new Lazy<IRoleFacade>(() => { return new RoleFacade(); }, true);
        //视图地址
        string _path = "/Views/Role/";
        #endregion

        #region 视图

        /// <summary>
        /// 【视图】列表
        /// </summary>
        /// <returns></returns>
        public ViewResult List()
        {
            return View();
        }
        /// <summary>
        /// 【视图】新增
        /// </summary>
        /// <returns></returns>
        public ViewResult Add()
        {
            ViewBag.OperType = "Add";
            ViewBag.ActionUrl = "_Add";

            Role_S model = new Role_S();
            return View(model);
        }
        /// <summary>
        /// 【视图】编辑
        /// </summary>
        /// <returns></returns>
        public ViewResult Edit(string ID)
        {
            Role_S model = _RoleFacade.Value.GetByID<Role_S>(ID);
            ViewBag.OperType = "Edit";
            ViewBag.ActionUrl = "_Edit";

            return View("Add", model);
        }

        /// <summary>
        /// 查看
        /// </summary>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        public ViewResult Show(string RoleID)
        {
            Role_S model = _RoleFacade.Value.GetByID<Role_S>(RoleID);
            return View(model);
        }

        /// <summary>
        /// 【视图】用户角色分配列表
        /// </summary>
        /// <returns></returns>
        public ViewResult UserRoleList(string UserID)
        {
            ViewBag.UserID = UserID;
            return View();
        }

        #endregion

        #region 方法

        /// <summary>
        /// 分页数据获取
        /// </summary>
        /// <param name="role">搜索条件</param>
        /// <param name="pi">分页信息</param>
        /// <returns></returns>
        public JsonResult _RoleList(Role_S role, PageInfo pi)
        {
            var data = _RoleFacade.Value.GetRolePaged(role, pi);
            var result = new
            {
                Rows = data,
                Total = pi.Total
            };
            return Json(result);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult _Add(Role_I role)
        {
            try
            {
                _RoleFacade.Value.Add(role);
                return Json(AjaxResult.Success("新增成功"));
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.Error(ex.Message));
            }
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult _Edit(Role_U role)
        {
            try
            {
                _RoleFacade.Value.Edit(role);
                return Json(AjaxResult.Success("修改成功！"));
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.Error(ex.Message));
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public JsonResult _Delete(string ids)
        {
            try
            {
                _RoleFacade.Value.Del(ids);
                return Json(AjaxResult.Success("删除成功！"));
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.Error(ex.Message));
            }
        }

        /// <summary>
        /// 判断角色名是否被使用
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="RoleName"></param>
        /// <returns></returns>
        public JsonResult _IsExists(string ID, string RoleName)
        {
            return Json(_RoleFacade.Value._IsExists(ID, RoleName));
        }

        /// <summary>
        /// 分页用户数据获取
        /// </summary>
        /// <param name="Role">搜索条件</param>
        /// <param name="pi">分页信息</param>
        /// <returns></returns>
        public JsonResult _UserRoleList(string UserID, string RoleName, PageInfo pi)
        {
            if (!string.IsNullOrEmpty(UserID))
            {
                var data = _RoleFacade.Value.GetAllRoleWithUser(UserID, RoleName);
                var result = new
                {
                    Rows = data,
                    Total = pi.Total
                };
                return Json(result);
            }
            return null;
        }

        /// <summary>
        /// 设置用户角色
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="RoleID"></param>
        /// <param name="IsHas">ture设置 false移除</param>
        /// <returns></returns>
        public JsonResult _SetRole(string UserID, string RoleID)
        {
            try
            {
                _RoleFacade.Value.SetUserRole(UserID, RoleID);
                return Json(AjaxResult.Success("成功！"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.Error("错误！" + ex), JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region 扩展
        #endregion
    }
}
