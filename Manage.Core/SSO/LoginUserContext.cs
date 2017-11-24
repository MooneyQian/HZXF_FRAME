using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Manage.Core.SSO;
using Manage.Framework;

namespace Manage.Core.SSO
{
    /// <summary>
    /// 登录用户缓存服务
    /// </summary>
    [Serializable]
    public class LoginUserContext : IDisposable
    {
        #region Properties        
        /// <summary>
        /// 登录用户编号
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// 公钥
        /// </summary>
        public string PublicCode { get; set; }

        /// <summary>
        /// 登录用户信息
        /// </summary>
        public UserInfo UserInfo { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime LoginTime { get; set; }

        /// <summary>
        /// 默认组织机构
        /// </summary>
        public OrganizationInfo DefaultOrganization { get; set; }

        /// <summary>
        /// 用户所有组织机构
        /// </summary>
        public List<OrganizationInfo> Organizations { get; set; }
        
        /// <summary>
        /// 用户角色
        /// </summary>
        public List<RoleInfo> UserRoles { get; set; }

        /// <summary>
        /// 用户菜单(MenuType为Menu的菜单)
        /// </summary>
        public List<MenuInfo> UserMenus { get; set; }

        /// <summary>
        /// 用户功能(MenuType为Function的菜单)
        /// </summary>
        public List<MenuInfo> UserFuns { get; set; }

        /// <summary>
        /// 用户数据权限(MenuType为Data的菜单)
        /// </summary>
        public List<MenuInfo> UserDatas { get; set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        public UserType UserType { get; set; }

        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            GC.Collect();
        }

        #endregion
    }
}
