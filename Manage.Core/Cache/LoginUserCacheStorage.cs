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
    /// 登录用户缓存服务
    /// </summary>
    public class LoginUserCacheStorage : BaseCacheStorage
    {
        public LoginUserCacheStorage()
            : base()
        {
        }

        #region Properties
        private static LoginUserCacheStorage _storage = null;
        /// <summary>
        /// 当前登录用户缓存服务
        /// </summary>
        public static LoginUserCacheStorage Current
        {
            get
            {
                if (_storage == null)
                {
                    _storage = new LoginUserCacheStorage();
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
                return "CACHE_LOGIN_USER";
            }
        }
        /// <summary>
        /// 缓存类型键名
        /// </summary>
        protected override string KEYID
        {
            get
            {
                return typeof(LoginUserContext).FullName;
            }
        }
        #endregion

        #region overload

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void Set(string loginID, LoginUserContext context)
        {
            _Set(loginID, context);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public LoginUserContext Get(string loginID)
        {
            var obj = _Get(loginID);
            if (obj != null)
                return obj as LoginUserContext;
            else
                return null;
        }

        #endregion

        #region 重置

        /// <summary>
        /// 重置登录用户缓存
        /// </summary>
        public void ClearSSPUser(string loginID)
        {
            base.Remove(loginID);
        }

        #endregion
    }
}
