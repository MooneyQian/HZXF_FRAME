using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Manage.Framework.Cache.Core
{
    /// <summary>
    /// 内存缓存实现类
    /// </summary>
    public class MemoryCache : ICacheOperations
    {
        public MemoryCache()
        {
        }
        public MemoryCache(string prefix)
        {
            _keyPrefix = prefix;
        }
        private string _keyPrefix="";
        private bool _isCache = true;
        /// <summary>
        /// Key前缀
        /// </summary>
        public string KeyPrefix
        {
            get
            {
                return _keyPrefix;
            }
            set
            {
                _keyPrefix = value;
            }
        }
        /// <summary>
        /// 是否缓存
        /// </summary>
        public bool IsCache
        {
            set
            {
                _isCache = value;
            }
            get
            {
                return _isCache;
            }
        }
        /// <summary>
        /// 缓存的大小
        /// </summary>
        public long Size
        {
            get { return CacheItemDictionary.Size; }
        }
        /// <summary>
        /// 缓存条目数
        /// </summary>
        public int Count
        {
            get { return CacheItemDictionary.ItemDictionary.Count; }
        }

        /// <summary>
        /// 每隔多少分钟扫描一次
        /// </summary>
        public int IntervalMinutes
        {
            get
            {
                return CacheItemDictionary.IntervalMinutes;
            }
            set
            {
                CacheItemDictionary.IntervalMinutes = value;
            }
        }
        /// <summary>
        /// 处理多少分钟之前的元素
        /// </summary>
        public int ScavangeMinutes
        {
            get
            {
                return CacheItemDictionary.ScavangeMinutes;
            }
            set
            {
                CacheItemDictionary.ScavangeMinutes = value;
            }
        }
        /// <summary>
        /// 最大可缓存元素数
        /// </summary>
        public long MaxCount
        {
            get
            {
                return CacheItemDictionary.MaxCount;
            }
            set
            {
                CacheItemDictionary.MaxCount = value;
            }
        }
        /// <summary>
        /// 最大可使用缓存大小
        /// </summary>
        public long MaxSize
        {
            get
            {
                return CacheItemDictionary.MaxSize;
            }
            set
            {
                CacheItemDictionary.MaxSize = value;
            }
        }
        /// <summary>
        /// 根据KEY获得缓存值
        /// </summary>
        /// <param name="key">KEY值</param>
        /// <returns>查找到缓存值</returns>
        public object Get(string key)
        {
            if (!IsCache)
            {
                return null;
            }
            key = KeyPrefix + key;
            if (CacheItemDictionary.ContainsKey(key))
            {
                CacheItem cacheItem =CacheItemDictionary.Get(key);
                return SerializationUtility.ToObject(cacheItem.value);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 设置缓存值
        /// </summary>
        /// <param name="key">KEY值</param>
        /// <param name="value">对象值</param>
        public void Set(string key, object value)
        {
            Set(key, value, DateTime.Now.AddYears(1));
        }

        /// <summary>
        /// 设置缓存值
        /// </summary>
        /// <param name="key">KEY值</param>
        /// <param name="value">对象值</param>
        /// <param name="expirySeconds">过期时效，分组是秒</param>
        public void Set(string key, object value, int expirySeconds)
        {
            Set(key, value, DateTime.Now.AddSeconds(expirySeconds));
        }

        /// <summary>
        /// 设置缓存值
        /// </summary>
        /// <param name="key">KEY值</param>
        /// <param name="value">对象值</param>
        /// <param name="expiryTimeSpan">过期间隔</param>
        public void Set(string key, object value, TimeSpan expiryTimeSpan)
        {
            Set(key, value, DateTime.Now.Add(expiryTimeSpan));
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">KEY值</param>
        /// <param name="value">对象值</param>
        /// <param name="expiryDate">过期时间</param>
        public void Set(string key, object value, DateTime expiryDate)
        {
            if (!IsCache)
            {
                return;
            }

            key = KeyPrefix + key;

            if (!value.GetType().IsSerializable)
                throw new SerializationException("Object is not serializable");

            CacheItemDictionary.Add(key,
                new CacheItem(DateTime.Now, SerializationUtility.ToBytes(value), expiryDate));
        }
        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">KEY值</param>
        public void Remove(string key)
        {
            if (!IsCache)
            {
                return;
            }
            key = KeyPrefix + key;
            CacheItemDictionary.Remove(key);
        }
        /// <summary>
        /// 移除所有缓存
        /// </summary>
        /// <returns>大于0表示移除的缓存记录数，等于0没有缓存可以移除。</returns>
        public int RemoveAll()
        {
            if (!IsCache)
            {
                return 0;
            }

            int interfaceCount = 0;

            interfaceCount = CacheItemDictionary.ItemDictionary.Count;
            CacheItemDictionary.Clear();

            return interfaceCount;
        }

        /// <summary>
        /// 检查是否包括KEY值
        /// </summary>
        /// <param name="key">KEY值</param>
        /// <returns></returns>
        public bool ContainsKey(string key)
        {
            if (!IsCache)
            {
                return false;
            }
            key = KeyPrefix + key;
            return CacheItemDictionary.ContainsKey(key);
        }
    }
}
