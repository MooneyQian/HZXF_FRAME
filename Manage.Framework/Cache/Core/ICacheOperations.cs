using System;

namespace Manage.Framework.Cache.Core
{
    /// <summary>
    /// 缓存操作类
    /// </summary>
    public interface ICacheOperations
    {
        object Get(string key);

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">KEY值</param>
        /// <param name="value">对象值</param>
        void Set(string key, object value);

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">KEY值</param>
        /// <param name="value">对象值</param>
        /// <param name="expirySeconds">过期时效</param>
        void Set(string key, object value, int expirySeconds);

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">KEY值</param>
        /// <param name="value">对象值</param>
        /// <param name="expiryDate">过期时效/param>
        void Set(string key, object value, DateTime expiryDate);

        /// <summary>
        ///根据KEY值移除缓存
        /// </summary>
        void Remove(string key);

        /// <summary>
        /// 移除所有缓存
        /// </summary>
        /// <returns>Number of objects revoked</returns>
        int RemoveAll();

        //void Scavange(double minutes);
        /// <summary>
        /// 判断是否包含缓存KEY
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool ContainsKey(string key);

        /// <summary>
        /// 缓存的大小
        /// </summary>
        long Size { get; }
    }
}
