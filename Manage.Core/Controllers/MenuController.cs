using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Manage.Framework;
using Manage.Core.Facades;
using Manage.Core.Models;
using Manage.Core.Common;
using Manage.Core.SSO;
using Manage.Open;
using Business.Controller.Common;

namespace Manage.Core.Controllers
{
    public class MenuController : BaseController
    {
        #region 定义
        //延迟加载业务处理器
        Lazy<IMenuFacade> _MenuFacade = new Lazy<IMenuFacade>(() => { return new MenuFacade(); }, true);
        //视图地址
        string _path = "/Views/Menu/";
        #endregion

        #region 视图
        /// <summary>
        /// 【视图】菜单列表
        /// </summary>
        /// <returns></returns>
        public ViewResult List()
        {
            return View();
        }
        /// <summary>
        /// 【视图】添加子菜单
        /// </summary>
        /// <param name="pid">父节点ID，0表示顶层</param>
        /// <returns></returns>
        public ViewResult Add(string pid)
        {
            Menu_S model = new Menu_S();
            if (pid == "root")
            {
                model = new Menu_S() { PerMenuID = "0", PerMenuName = "", MenuLevel = 1 };
            }
            else
            {
                var perMenu = _MenuFacade.Value.GetByID<Menu_S>(pid);
                model = new Menu_S() { PerMenuID = pid, PerMenuName = perMenu.MenuName, MenuLevel = perMenu.MenuLevel + 1 };
            }
            ViewBag.ActionUrl = "_Add";
            ViewBag.OperType = "Add";
            //model.Extend3 = "SView";
            ViewBag.PrivilegeType = DictionaryshipFactory.Instance.GetDictSelectList(DictParam.SysPrivilegeType, true);

            return View(model);
        }

        /// <summary>
        /// 【视图】编辑菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ViewResult Edit(string id)
        {
            var model = _MenuFacade.Value.GetByID<Menu_S>(id);
            if (model != null)
            {
                var perMenu = _MenuFacade.Value.GetByID<Menu_S>(model.PerMenuID);
                if (perMenu != null)
                    model.PerMenuName = perMenu.MenuName;
            }

            ViewBag.ActionUrl = "_Edit";
            ViewBag.OperType = "Edit";
            ViewBag.PrivilegeType = DictionaryshipFactory.Instance.GetDictSelectList(DictParam.SysPrivilegeType, true);
            return View("Add", model);
        }

        /// <summary>
        /// 【视图】查看菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ViewResult Show(string id)
        {
            var model = _MenuFacade.Value.GetByID<Menu_S>(id);
            if (model != null)
            {
                var perMenu = _MenuFacade.Value.GetByID<Menu_S>(model.PerMenuID);
                if (perMenu != null)
                    model.PerMenuName = perMenu.MenuName;
            }
            return View(model);
        }

        /// <summary>
        /// 【视图】分配角色菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ViewResult RoleMenuList(string RoleID)
        {
            ViewBag.RoleID = RoleID;
            return View();
        }

        #endregion

        #region 方法

        /// <summary>
        /// 获取菜单Json
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetMenuTree()
        {
            var tree = _MenuFacade.Value.GetAllMenus().Select(s => new
            {
                id = s.ID,
                pId = s.PerMenuID == Define._TOPPARENTID ? "root" : s.PerMenuID,//增加一个ID为root的顶层虚拟菜单
                name = s.MenuName,
                title = s.MenuDesc,
                menucode = s.MenuCode,
                menupath = s.MenuPath,
                order = s.MenuOrder,
                level = s.MenuLevel,
                open = s.MenuLevel <= 1,
                menuType = s.MenuType
            }).ToList();
            tree.Add(new
            {
                id = "root",
                pId = "0",
                name = "顶层虚拟菜单",
                title = "虚拟菜单",
                menucode = "",
                menupath = "",
                order = 0,
                level = -1,
                open = true,
                menuType = (int)MenuType.Menu
            });
            return Json(AjaxResult.Success(tree));
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public JsonResult _Add(Menu_I menu)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(menu.MenuIcon))
                {
                    byte[] outputb = Convert.FromBase64String(menu.MenuIcon);
                    menu.MenuIcon = Encoding.Default.GetString(outputb);
                }
                _MenuFacade.Value.Add(menu);
                var model = (new
                {
                    id = menu.ID,
                    pId = menu.PerMenuID == Define._TOPPARENTID ? "root" : menu.PerMenuID,
                    name = menu.MenuName,
                    title = menu.MenuDesc,
                    menucode = menu.MenuCode,
                    menupath = menu.MenuPath,
                    order = menu.MenuOrder,
                    level = menu.MenuLevel,
                    menuType = menu.MenuType
                });
                return Json(AjaxResult.Success(model, "菜单新增成功!"));
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.Error("菜单新增失败!错误原因：" + ex.Message));
            }
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public JsonResult _Edit(Menu_U menu)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(menu.MenuIcon))
                {
                    byte[] outputb = Convert.FromBase64String(menu.MenuIcon);
                    menu.MenuIcon = Encoding.Default.GetString(outputb);
                }

                _MenuFacade.Value.Edit(menu);
                var model = (new
                {
                    id = menu.ID,
                    name = menu.MenuName,
                    title = menu.MenuDesc,
                    menucode = menu.MenuCode,
                    menupath = menu.MenuPath,
                    order = menu.MenuOrder
                });
                return Json(AjaxResult.Success(model, "菜单更新成功!"));
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.Error("菜单更新失败!错误原因：" + ex.Message));
            }
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public JsonResult _Delete(string ids)
        {
            try
            {
                _MenuFacade.Value.Del(ids);
                return Json(AjaxResult.Success("用户删除成功！"));
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.Error(ex.Message));
            }
        }

        /// <summary>
        /// 判断同级中菜单编号是否有重复
        /// </summary>
        /// <param name="MenuCode"></param>
        /// <param name="PerMenuID"></param>
        /// <returns></returns>
        public JsonResult _MenuFuncCodeIsSuc(string ID, string MenuCode, string PerMenuID)
        {
            try
            {
                var res = _MenuFacade.Value.IsFuncExists(ID, MenuCode, PerMenuID);
                return Json(res);
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.Error("错误！"));
            }
        }

        /// <summary>
        /// 判断数据权限编号是否有重复
        /// </summary>
        /// <param name="MenuCode"></param>
        /// <returns></returns>
        public JsonResult _MenuDataCodeIsSuc(string ID, string MenuCode)
        {
            try
            {
                var res = _MenuFacade.Value.IsDataExists(ID, MenuCode);
                return Json(res);
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.Error("错误！"));
            }
        }


        /// <summary>
        /// 分配角色菜单数据
        /// </summary>
        /// <param name="RoleID"></param>
        /// <param name="MenuName"></param>
        /// <returns></returns>
        public JsonResult _RoleMenuList(string RoleID, string MenuName)
        {
            if (!string.IsNullOrEmpty(RoleID))
            {
                var data = _MenuFacade.Value.GetAllMenuWithRole(RoleID, MenuName);
                var result = new
                {
                    Rows = data
                };
                return Json(result);
            }
            return null;
        }

        /// <summary>
        /// 设置角色菜单
        /// </summary>
        /// <param name="RoleID"></param>
        /// <param name="MenuIDs"></param>
        /// <returns></returns>
        public JsonResult _SetRoleMenu(string RoleID, List<string> MenuIDs)
        {
            try
            {
                if (!string.IsNullOrEmpty(RoleID))
                {
                    _MenuFacade.Value.SetRoleMenus(RoleID, MenuIDs);
                    return Json(AjaxResult.Success("角色菜单设置成功！"));
                }
                else
                {
                    return Json(AjaxResult.Error("角色菜单设置失败，原因：角色ID不存在！"));
                }
            }
            catch (Exception ex)
            {
                return Json(AjaxResult.Error("角色菜单设置失败，原因：" + ex.Message));
            }
        }

        #endregion

        #region 扩展

        /// <summary>
        /// 验证按钮菜单权限
        /// </summary>
        /// <param name="menuid">页面菜单ID</param>
        /// <param name="menus">需要验证的菜单编号，使用","逗号分隔</param>
        /// <returns>返回拥有权限的菜单名称</returns>
        public JsonResult VaildMenuRole(string menuid, string menus)
        {
            if (string.IsNullOrWhiteSpace(menuid))
                return Json(AjaxResult.Error("页面菜单ID未设置。"));
            if (string.IsNullOrWhiteSpace(menus))
                return Json(AjaxResult.Error("验证按钮名称未传递。"));

            List<string> success = new List<string> { };
            //获取该页面下需要权限验证的所有按钮菜单权限
            var allFunMenu = Manage.Open.MembershipFactory.Instance.GetFunMenuByMenuID_Cache(menuid);
            //获取当前用户拥有的所有按钮菜单权限
            var userFunMenu = CurrentUserContext.UserFuns;
            foreach (var m in menus.Split(','))
            {
                if (allFunMenu.Count(c => c.MenuCode == m) == 0     //该按钮不需要权限验证
                    || userFunMenu.Count(c => c.PerMenuID == menuid && c.MenuCode == m) > 0)     //该按钮需要权限验证，且用户拥有该权限
                    success.Add(m);
            }
            return Json(AjaxResult.Success(string.Join(",", success.ToArray()), "成功"));
        }


        /// <summary>
        /// 根据菜单id获取菜单按钮权限
        /// </summary>
        /// <param name="menuid"></param>
        /// <returns></returns>
        public JsonResult _GetMenuRole(string menuid)
        {
            if (string.IsNullOrWhiteSpace(menuid))
                return Json(AjaxResult.Error("页面菜单ID未设置。"));
            //获取该页面下需要权限验证的所有按钮菜单权限
            var allFunMenu = Manage.Open.MembershipFactory.Instance.GetFunMenuByMenuID_Cache(menuid);
            //获取当前用户拥有的所有按钮菜单权限
            var userFunMenu = CurrentUserContext.UserFuns;
            var res = new List<MenuInfo>();

            if (CurrentUserContext.UserType == UserType.Administrators)
            {
                res = allFunMenu;
            }
            else
            {
                foreach (var item in allFunMenu)
                {
                    if (userFunMenu.FindIndex(c => c.ID == item.ID) >= 0)
                    {
                        res.Add(item);
                    }

                }
            }

            var result = new
                 {
                     Rows = res
                 };
            return Json(result);

        }

        #endregion
    }
}
