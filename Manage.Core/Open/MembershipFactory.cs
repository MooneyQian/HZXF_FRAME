using Manage.Core.Cache;
using Manage.Core.Facades;
using Manage.Core.Models;
using Manage.Core.SSO;
using Manage.Framework;
using Manage.SSO.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manage.Open
{
    /// <summary>
    /// 用户、组织、角色相关对外开放接口
    /// </summary>
    public class MembershipFactory
    {
        /// <summary>
        /// 单例对象
        /// </summary>
        public static readonly MembershipFactory Instance = new MembershipFactory();

        /// <summary>
        /// facade延迟对象
        /// </summary>
        private Lazy<IUserFacade> _userFacade = new Lazy<IUserFacade>(() => { return SSOFacadeAdapter.UserInstance(); }, true);
        private Lazy<IOrganizationFacade> _organFacade = new Lazy<IOrganizationFacade>(() => { return new OrganizationFacade(); }, true);
        private Lazy<IRoleFacade> _roleFacade = new Lazy<IRoleFacade>(() => { return new RoleFacade(); }, true);
        private Lazy<IMenuFacade> _menuFacade = new Lazy<IMenuFacade>(() => { return new MenuFacade(); }, true);

        #region user

        /// <summary>
        /// 根据用户ID获取用户对象
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public UserInfo GetUserByID(string ID)
        {
            return _userFacade.Value.GetUserInfoByID(ID);
        }
        /// <summary>
        /// 根据用户登录帐号获取用户对象
        /// </summary>
        /// <param name="LoginName"></param>
        /// <returns></returns>
        public UserInfo GetUserByLoginName(string LoginName)
        {
            return _userFacade.Value.GetUserByLoginName(LoginName);
        }

        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <param name="LoginNameOrDisplayName"></param>
        /// <returns></returns>
        public List<UserInfo> GetAllUsers(string LoginName, string DisplayName)
        {
            return _userFacade.Value.GetAllUsers(LoginName, DisplayName);
        }

        /// <summary>
        /// 获取所有用户分页数据
        /// </summary>
        /// <param name="LoginNameOrDisplayName"></param>
        /// <returns></returns>
        public List<UserInfo> GetUserPaged(string LoginName, string DisplayName, PageInfo pi)
        {
            return _userFacade.Value.GetUserPaged(LoginName, DisplayName, pi);
        }

        #endregion

        #region organization

        /// <summary>
        /// 获取所有分组，从数据库取
        /// </summary>
        /// <returns></returns>
        internal List<OrganizationInfo> GetAllOrgans()
        {
            return _organFacade.Value.GetAllOrgans();
        }
        /// <summary>
        /// 【缓存优先】获取所有分组
        /// </summary>
        /// <returns></returns>
        public List<OrganizationInfo> GetAllOrgans_Cache()
        {
            var orgs = SSOCacheStorage.Current.GetAllOrgan();
            if (orgs == null)
            {
                orgs = GetAllOrgans();
                SSOCacheStorage.Current.SetAllOrgan(orgs);
            }
            return orgs;
        }
        /// <summary>
        /// 通过分组ID获取分组，从数据库中取
        /// </summary>
        /// <returns></returns>
        public OrganizationInfo GetOrganByID(string ID)
        {
            return _organFacade.Value.GetOrganByID(ID);
        }
        /// <summary>
        /// 【缓存优先】通过分组ID获取分组
        /// </summary>
        /// <returns></returns>
        public OrganizationInfo GetOrganByID_Cache(string ID)
        {
            var orgs = GetAllOrgans_Cache();
            return orgs.Where(w => w.ID == ID).FirstOrDefault() ?? new OrganizationInfo();
        }
        /// <summary>
        /// 获取用户的默认分组
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public OrganizationInfo GetDefaultOrganByUser(string UserID)
        {
            var org = _organFacade.Value.GetDefaultOrganByUser(UserID);
            if (org != null)
                return GetOrganByID_Cache(org.OrganizationID);
            else
                return new OrganizationInfo();
        }
        /// <summary>
        /// 获取用户的所有分组
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<OrganizationInfo> GetOrgansByUser(string UserID)
        {
            var orgs = _organFacade.Value.GetOrgansByUser(UserID);
            if (orgs != null)
                return GetAllOrgans_Cache().Where(w => orgs.Select(s => s.OrganizationID).Contains(w.ID)).ToList() ?? new List<OrganizationInfo>();
            else
                return new List<OrganizationInfo>();
        }

        #endregion

        #region role

        /// <summary>
        /// 获取所有角色，从数据库取
        /// </summary>
        /// <returns></returns>
        internal List<RoleInfo> GetAllRoles()
        {
            return _roleFacade.Value.GetAllRoles();
        }
        /// <summary>
        /// 【缓存优先】获取所有角色
        /// </summary>
        /// <returns></returns>
        public List<RoleInfo> GetAllRoles_Cache()
        {
            var roles = SSOCacheStorage.Current.GetAllRole();
            if (roles == null)
            {
                roles = GetAllRoles();
                SSOCacheStorage.Current.SetAllRole(roles);
            }
            return roles;
        }
        /// <summary>
        /// 通过角色ID获取角色，从数据库中取
        /// </summary>
        /// <returns></returns>
        public RoleInfo GetRoleByID(string ID)
        {
            return _roleFacade.Value.GetRoleByID(ID);
        }
        /// <summary>
        /// 【缓存优先】通过角色ID获取角色
        /// </summary>
        /// <returns></returns>
        public RoleInfo GetRoleByID_Cache(string ID)
        {
            var roles = GetAllRoles_Cache();
            return roles.Where(w => w.ID == ID).FirstOrDefault() ?? new RoleInfo();
        }
        /// <summary>
        /// 获取用户的角色
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<RoleInfo> GetRolesByUser(string UserID)
        {
            var roles = _roleFacade.Value.GetRolesByUser(UserID);
            if (roles != null)
                return GetAllRoles_Cache().Where(w => roles.Select(s => s.RoleID).Contains(w.ID)).ToList() ?? new List<RoleInfo>();
            else
                return new List<RoleInfo>();
        }

        #endregion

        #region menu

        /// <summary>
        /// 获取用户有权限的菜单
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<MenuInfo> GetMenusByUserID(string UserID)
        {
            return _menuFacade.Value.GetMenusByUserID(UserID);
        }
        /// <summary>
        /// 获取用户有权限的菜单(根据菜单类型)
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<MenuInfo> GetMenusByUserID(string UserID, MenuType menuType)
        {
            return _menuFacade.Value.GetMenusByUserID(UserID, menuType);
        }
        /// <summary>
        /// 获取所有或页面内的所有需要验证的功能按钮
        /// </summary>
        /// <param name="MenuID"></param>
        /// <returns></returns>
        public List<MenuInfo> GetFunMenuByMenuID(string MenuID = "")
        {
            return _menuFacade.Value.GetFunMenuByMenuID(MenuID);
        }
        /// <summary>
        /// 获取所有或页面内的所有需要验证的功能按钮，优先缓存中获取
        /// </summary>
        /// <param name="MenuID"></param>
        /// <returns></returns>
        public List<MenuInfo> GetFunMenuByMenuID_Cache(string MenuID = "")
        {
            var menus = SSOCacheStorage.Current.GetAllFunMenu();
            if (menus == null)
            {
                menus = GetFunMenuByMenuID();
                SSOCacheStorage.Current.SetAllFunMenu(menus);
            }
            if (!string.IsNullOrEmpty(MenuID))
                return menus.Where(c => c.PerMenuID == MenuID).ToList();
            else
                return menus;
        }
        /// <summary>
        /// 获取所有菜单（MenuType为Menu的菜单）
        /// </summary>
        /// <returns></returns>
        public List<MenuInfo> GetAllMenus()
        {
            return _menuFacade.Value.GetAllMenuInfos();
        }
        /// <summary>
        /// 【未开放】获取超级管理员菜单
        /// </summary>
        /// <returns></returns>
        internal List<MenuInfo> GetAdminMenu()
        {
            return _menuFacade.Value.GetAdminMenu();
        }

        #endregion
    }
}
