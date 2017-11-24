using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Manage.Framework;
using Manage.Core.Facades;
using Manage.Core.Models;
using Manage.Core.Common;

namespace Manage.Core.Controllers
{
    public class OrganizationController : BaseController
    {
        #region 定义
        //延迟加载业务处理器
        Lazy<IOrganizationFacade> _OrganizationFacade = new Lazy<IOrganizationFacade>(() => { return new OrganizationFacade(); }, true);
        //视图地址
        string _path = "/Views/Organization/";
        #endregion

        #region 视图
        /// <summary>
        /// 【视图】分组列表
        /// </summary>
        /// <returns></returns>
        public ViewResult List()
        {
            return View();
        }
        /// <summary>
        /// 【视图】添加子分组
        /// </summary>
        /// <param name="pid">父节点ID，0表示顶层</param>
        /// <returns></returns>
        public ViewResult Add(string pid)
        {
            Organization_S model = new Organization_S();
            if (pid == "root")
            {
                model = new Organization_S() { OrganParentID = "0", ParOrganName = "", LevelNO = 1 };
            }
            else
            {
                var perOrganization = _OrganizationFacade.Value.GetByID<Organization_S>(pid);
                model = new Organization_S() { OrganParentID = pid, ParOrganName = perOrganization.OrganName, LevelNO = perOrganization.LevelNO + 1 };
            }
            ViewBag.ActionUrl = "_Add";
            ViewBag.OperType = "Add";
            return View(model);
        }

        /// <summary>
        /// 【视图】编辑分组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ViewResult Edit(string id)
        {
            var model = _OrganizationFacade.Value.GetByID<Organization_S>(id);
            if (model != null)
            {
                var perOrganization = _OrganizationFacade.Value.GetByID<Organization_S>(model.OrganParentID);
                if (perOrganization != null)
                    model.ParOrganName = perOrganization.OrganName;
            }
            ViewBag.ActionUrl = "_Edit";
            ViewBag.OperType = "Edit";
            return View("Add", model);
        }

        /// <summary>
        /// 【视图】查看分组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ViewResult Show(string id)
        {
            var model = _OrganizationFacade.Value.GetByID<Organization_S>(id);
            if (model != null)
            {
                var perOrganization = _OrganizationFacade.Value.GetByID<Organization_S>(model.OrganParentID);
                if (perOrganization != null)
                    model.ParOrganName = perOrganization.OrganName;
            }
            return View(model);
        }

        /// <summary>
        /// 【视图】分配角色分组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ViewResult RoleOrganizationList(string RoleID)
        {
            ViewBag.RoleID = RoleID;
            return View();
        }

        /// <summary>
        /// 分组用户设置列表
        /// </summary>
        /// <returns></returns>
        public ViewResult OrganizationUserList()
        {
            return View();
        }
        /// <summary>
        /// 分组设备列表
        /// </summary>
        /// <returns></returns>
        public ViewResult OrganizationEquipmentList()
        {
            return View();
        }

        #endregion

        #region 方法

        /// <summary>
        /// 获取分组Json
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetOrganizationTree()
        {
            var tree = _OrganizationFacade.Value.GetAll<Organization_S>().Select(s => new
            {
                id = s.ID,
                pId = s.OrganParentID == Define._TOPPARENTID ? "root" : s.OrganParentID,//增加一个ID为root的顶层虚拟分组
                name = s.OrganName,
                title = s.OrganDesc,
                order = s.OrganOrder,
                level = s.LevelNO,
                open = s.LevelNO <= 1
            }).ToList();
            tree.Add(new
            {
                id = "root",
                pId = "0",
                name = "顶层虚拟部门",
                title = "虚拟部门",
                order = 0,
                level = -1,
                open = true
            });
            return Json(AjaxResult.Success(tree));
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public JsonResult _Add(Organization_I menu)
        {
            try
            {
                _OrganizationFacade.Value.Add(menu);
                var model = (new
                {
                    id = menu.ID,
                    pId = menu.OrganParentID == Define._TOPPARENTID ? "root" : menu.OrganParentID,
                    name = menu.OrganName,
                    title = menu.OrganDesc,
                    order = menu.OrganOrder,
                    level = menu.LevelNO
                });
                return Json(AjaxResult.Success(model, "分组新增成功!"));
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.Error("分组新增失败!错误原因：" + ex.Message));
            }
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public JsonResult _Edit(Organization_U menu)
        {
            try
            {
                _OrganizationFacade.Value.Edit(menu);
                var model = (new
                {
                    id = menu.ID,
                    name = menu.OrganName,
                    title = menu.OrganDesc,
                    order = menu.OrganOrder
                });
                return Json(AjaxResult.Success(model, "分组更新成功!"));
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.Error("分组更新失败!错误原因：" + ex.Message));
            }
        }

        /// <summary>
        /// 删除分组
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public JsonResult _Delete(string ids)
        {
            try
            {
                _OrganizationFacade.Value.Del(ids);
                return Json(AjaxResult.Success("用户删除成功！"));
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.Error(ex.Message));
            }
        }

        /// <summary>
        /// 判断分组编号是否有重复
        /// </summary>
        /// <param name="OrganNO"></param>
        /// <returns></returns>
        public JsonResult _OrganNOIsSuc(string ID, string OrganNO)
        {
            try
            {
                var res = _OrganizationFacade.Value.IsExists(ID, OrganNO);
                return Json(res);
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.Error("错误！"));
            }
        }


        /// <summary>
        /// 分页用户数据获取
        /// </summary>
        /// <param name="user">搜索条件</param>
        /// <param name="pi">分页信息</param>
        /// <returns></returns>
        public JsonResult _OrganizationUserList(string OrgID, string LoginName, string DisplayName, PageInfo pi)
        {
            List<OrganUser_S> data = new List<OrganUser_S>();
            if (!string.IsNullOrEmpty(OrgID))
                data = _OrganizationFacade.Value.GetUserWithOrgan(OrgID, LoginName, DisplayName);
            var result = new
            {
                Rows = data,
                Total = pi.Total
            };
            return Json(result);
        }

        /// <summary>
        /// 设置分组用户
        /// </summary>
        /// <param name="OrganID"></param>
        /// <param name="UserID"></param>
        /// <param name="isHas"></param>
        /// <returns></returns>
        public JsonResult _SetUserOrgan(string OrganID, string UserID, bool IsHas)
        {
            if (!string.IsNullOrWhiteSpace(OrganID) && !string.IsNullOrWhiteSpace(UserID))
            {
                _OrganizationFacade.Value.SetUserOrgan(OrganID, UserID, IsHas);
                return Json(AjaxResult.Success("成功！"), JsonRequestBehavior.AllowGet);
            }
            return Json(AjaxResult.Error("错误！"), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 设置为主分组
        /// </summary>
        /// <param name="OrganID"></param>
        /// <param name="UserID"></param>
        /// <param name="isHas"></param>
        /// <returns></returns>
        public JsonResult _SetDefaultOrgan(string OrganID, string UserID)
        {
            if (!string.IsNullOrWhiteSpace(OrganID) && !string.IsNullOrWhiteSpace(UserID))
            {
                _OrganizationFacade.Value.SetDefaultOrgan(OrganID, UserID);
                return Json(AjaxResult.Success("成功！"), JsonRequestBehavior.AllowGet);
            }
            return Json(AjaxResult.Error("错误！"), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 扩展

        #endregion
    }
}
