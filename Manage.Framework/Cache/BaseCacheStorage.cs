using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Manage.Framework.Cache.Core;

namespace Manage.Framework
{
    public abstract class BaseCacheStorage
    {
        /// <summary>
        /// 设置缓存空间名
        /// </summary>
        protected abstract string CacheArea { get; }
        /// <summary>
        /// 用于区分每缓存类型的键名
        /// </summary>
        protected abstract string KEYID { get; }
        private MemoryCache _cache;
        protected MemoryCache Cache
        {
            get { return _cache; }
        }

        /// <summary>
        /// 默认Default缓存空间
        /// </summary>
        public BaseCacheStorage()
        {
            _cache = MemoryCacheClient.GetInstance(CacheArea);
        }

        /// <summary>
        /// 自定义缓存空间
        /// </summary>
        /// <param name="name"></param>
        public BaseCacheStorage(string name)
        {
            _cache = MemoryCacheClient.GetInstance(name);
        }

        public virtual string GenerateCacheKey(string key)
        {
            return KEYID + "_" + key;
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        protected void _Set(string key, object obj)
        {
            var newKey = GenerateCacheKey(key);
            _cache.Set(newKey, obj);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected object _Get(string key)
        {
            var newKey = GenerateCacheKey(key);
            return _cache.Get(newKey);
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key"></param>
        public virtual void Remove(string key)
        {
            var newKey = GenerateCacheKey(key);
            _cache.Remove(newKey);
        }
        
        /// <summary>
        /// 重置缓存域
        /// </summary>
        /// <returns>大于0表示移除的缓存记录数，等于0没有缓存可以移除。</returns>
        public virtual int RemoveAll()
        {
            return _cache.RemoveAll();
        }

        /// <summary>
        /// 是否包含键
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual bool ContainsKey(string key)
        {
            var newKey = GenerateCacheKey(key);
            return (_cache.Get(newKey) != null);
        }
    }
}
