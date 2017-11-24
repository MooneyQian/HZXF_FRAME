using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Manage.Core.SSO;
using Manage.Framework;

namespace Manage.Core.Cache
{
    /// <summary>
    /// 组织机构、角色缓存服务
    /// </summary>
    public class SSOCacheStorage : BaseCacheStorage
    {
        public SSOCacheStorage()
            : base()
        {
        }

        #region Properties
        private static SSOCacheStorage _storage = null;
        /// <summary>
        /// 当前登录用户缓存服务
        /// </summary>
        public static SSOCacheStorage Current
        {
            get
            {
                if (_storage == null)
                {
                    _storage = new SSOCacheStorage();
                }
                return _storage;
            }
        }

        #endregion

        #region impl
        /// <summary>
        /// 缓存空间名
        /// </summary>
        protected override string CacheArea
        {
            get
            {
                return "CACHE_ORGAN_ROLE_MENU";
            }
        }
        /// <summary>
        /// 缓存类型键名
        /// </summary>
        protected override string KEYID
        {
            get
            {
                return typeof(SSOCacheStorage).FullName;
            }
        }
        #endregion

        #region overload

        /// <summary>
        /// 设置组织机构缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void SetAllOrgan(List<OrganizationInfo> context)
        {
            _Set("ALL_ORGAN", context);
        }

        /// <summary>
        /// 设置角色缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void SetAllRole(List<RoleInfo> context)
        {
            _Set("ALL_ROLE", context);
        }

        /// <summary>
        /// 设置功能菜单
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void SetAllFunMenu(List<MenuInfo> context)
        {
            _Set("ALL_FunMenu", context);
        }

        /// <summary>
        /// 获取组织机构缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<OrganizationInfo> GetAllOrgan()
        {
            var obj = _Get("ALL_ORGAN");
            if (obj != null)
                return obj as List<OrganizationInfo>;
            else
                return null;
        }

        /// <summary>
        /// 获取角色缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<RoleInfo> GetAllRole()
        {
            var obj = _Get("ALL_ROLE");
            if (obj != null)
                return obj as List<RoleInfo>;
            else
                return null;
        }

        /// <summary>
        /// 获取功能菜单缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<MenuInfo> GetAllFunMenu()
        {
            var obj = _Get("ALL_FunMenu");
            if (obj != null)
                return obj as List<MenuInfo>;
            else
                return null;
        }

        #endregion

        #region 重置

        /// <summary>
        /// 重置组织机构缓存
        /// </summary>
        public void ClearAllOrgan()
        {
            base.Remove("ALL_ORGAN");
        }

        /// <summary>
        /// 清理角色缓存
        /// </summary>
        public void ClearAllRole()
        {
            base.Remove("ALL_ROLE");
        }

        /// <summary>
        /// 清理功能菜单缓存
        /// </summary>
        public void ClearAllFunMenu()
        {
            base.Remove("ALL_FunMenu");
        }

        #endregion
    }
}
