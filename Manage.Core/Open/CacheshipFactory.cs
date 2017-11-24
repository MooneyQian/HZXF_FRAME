using Manage.Core.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manage.Open
{
    /// <summary>
    /// 缓存相关对外开放接口
    /// </summary>
    public class CacheshipFactory
    {
        /// <summary>
        /// 单例对象
        /// </summary>
        public static readonly CacheshipFactory Instance = new CacheshipFactory();

        /// <summary>
        /// 清理登录用户缓存
        /// </summary>
        /// <param name="UserID">用户ID</param>
        public void ClearSSOUserCache(string UserID)
        {
            LoginUserCacheStorage.Current.ClearSSPUser(UserID);
        }

        /// <summary>
        /// 清理所有登录用户缓存
        /// </summary>
        /// <returns>大于0表示移除的缓存记录数，等于0没有缓存可以移除。</returns>
        public int ClearAllSSOUser()
        {
            return LoginUserCacheStorage.Current.RemoveAll();
        }

        /// <summary>
        /// 清理组织机构缓存
        /// </summary>
        public void ClearOrganCache()
        {
            SSOCacheStorage.Current.ClearAllOrgan();
        }

        /// <summary>
        /// 清理角色缓存
        /// </summary>
        public void ClearRoleCache()
        {
            SSOCacheStorage.Current.ClearAllRole();
        }

        /// <summary>
        /// 清理功能菜单缓存
        /// </summary>
        public void ClearFunMenuCache()
        {
            SSOCacheStorage.Current.ClearAllFunMenu();
        }

        /// <summary>
        /// 清理数据字典缓存
        /// </summary>
        /// <param name="DictType">数据字典类型</param>
        public void ClearDictionaryCache(string DictType)
        {
            DictionaryCacheStorage.Current.ClearDictionary(DictType);
        }

        /// <summary>
        /// 清理所有数据字典缓存
        /// </summary>
        /// <returns>大于0表示移除的缓存记录数，等于0没有缓存可以移除。</returns>
        public int ClearAllDictionary()
        {
            return DictionaryCacheStorage.Current.RemoveAll();
        }
    }
}
