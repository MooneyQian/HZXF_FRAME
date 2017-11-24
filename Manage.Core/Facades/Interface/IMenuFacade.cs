using System;
using Manage.Core.Models;
using System.Collections.Generic;
using Manage.Framework;
using Manage.Core.SSO;
namespace Manage.Core.Facades
{
    public interface IMenuFacade : IBaseFacade
    {
        
        /// <summary>
        /// 获取所有菜单（除管理员菜单）
        /// </summary>
        /// <returns></returns>
        List<Menu_S> GetAllMenus();

        /// <summary>
        /// 判断菜单编号是否存在
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="MenuCode"></param>
        /// <param name="PerMenuID"></param>
        /// <returns></returns>
        bool IsFuncExists(string ID, string MenuCode, string PerMenuID);

        /// <summary>
        /// 判断数据权限编号是否存在
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="MenuCode"></param>
        /// <returns></returns>
        bool IsDataExists(string ID, string MenuCode);
        
        /// <summary>
        /// 获取角色菜单分配页面数据
        /// </summary>
        /// <param name="RoleID"></param>
        /// <param name="MenuName"></param>
        /// <returns></returns>
        List<Menu_S> GetAllMenuWithRole(string RoleID, string MenuName);
        
        /// <summary>
        /// 设置角色菜单
        /// </summary>
        /// <param name="RoleID"></param>
        /// <param name="MenuIDs"></param>
        void SetRoleMenus(string RoleID, List<string> MenuIDs);

        
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        void Add(Menu_I model);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        void Edit(Menu_U model);

        /// <summary>
        /// 删除多个对象
        /// </summary>
        /// <param name="IDs">需要删除数据的ID，使用“,”分隔</param>
        void Del(string IDs);

        #region MembershipFactory

        /// <summary>
        /// 获取用户有权限的菜单
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        List<MenuInfo> GetMenusByUserID(string UserID);

        /// <summary>
        /// 获取用户有权限的菜单(根据菜单类型)
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        List<MenuInfo> GetMenusByUserID(string UserID, MenuType menuType);

        /// <summary>
        /// 获取所有或页面内的所有需要验证的功能按钮
        /// </summary>
        /// <param name="MenuID"></param>
        /// <returns></returns>
        List<MenuInfo> GetFunMenuByMenuID(string MenuID = "");

        /// <summary>
        /// 获取所有菜单（MenuType为Menu的菜单）
        /// </summary>
        /// <returns></returns>
        List<MenuInfo> GetAllMenuInfos();

        /// <summary>
        /// 获取超级管理员菜单
        /// </summary>
        /// <returns></returns>
        List<MenuInfo> GetAdminMenu();

        #endregion
    }
}
