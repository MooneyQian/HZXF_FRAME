using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Manage.Core.SSO;
using Manage.Framework;
using Manage.Core.Models;

namespace Manage.Core.Cache
{
    /// <summary>
    /// 数据字典缓存服务
    /// </summary>
    public class DictionaryCacheStorage : BaseCacheStorage
    {
        public DictionaryCacheStorage()
            : base()
        {
        }

        #region Properties
        private static DictionaryCacheStorage _storage = null;
        /// <summary>
        /// 数据字典缓存服务
        /// </summary>
        public static DictionaryCacheStorage Current
        {
            get
            {
                if (_storage == null)
                {
                    _storage = new DictionaryCacheStorage();
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
                return "CACHE_SYS_DICTIONARY";
            }
        }
        /// <summary>
        /// 缓存类型键名
        /// </summary>
        protected override string KEYID
        {
            get
            {
                return typeof(SysDictionary).FullName;
            }
        }
        #endregion

        #region overload

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void Set(string DictType, List<SysDictionary> context)
        {
            _Set(DictType, context);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<SysDictionary> Get(string DictType)
        {
            var obj = _Get(DictType);
            if (obj != null)
                return obj as List<SysDictionary>;
            else
                return null;
        }

        #endregion

        #region 重置

        /// <summary>
        /// 重置登录用户缓存
        /// </summary>
        public void ClearDictionary(string DictType)
        {
            base.Remove(DictType);
        }

        #endregion
    }
}
