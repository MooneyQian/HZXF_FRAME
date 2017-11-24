using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manage.Framework.Cache.Core
{
    public class MemoryCacheClient
    {
        private static Dictionary<string, MemoryCache> instances = new Dictionary<string, MemoryCache>();
        private static MemoryCache defaultInstance = null;
        
        /// <summary>
        /// 创建缓存
        /// </summary>
        /// <param name="name">名称</param>
        public static void Setup(string name)
        {
            if (instances.ContainsKey(name))
            {
                throw new Exception("Trying to configure MemoryCachedClient instance \"" + name + "\" twice.");
            }
            instances[name] = new MemoryCache(name);
        }

        /// <summary>
        /// 获得名称为（Default)的缓存操作实例
        /// </summary>
        /// <returns>缓存操作实例</returns>
        public static MemoryCache GetInstance()
        {
            return defaultInstance ?? (defaultInstance = GetInstance("Default"));
        }
        
        /// <summary>
        /// 根据名称获得缓存操作实例
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>缓存操作实例</returns>
        public static MemoryCache GetInstance(string name)
        {
            MemoryCache c;
            if (instances.TryGetValue(name, out c))
            {
                return c;
            }
            else
            {
                    Setup(name);
                    c = GetInstance(name);
                    c.IsCache = true;
                    c.IntervalMinutes = 1;
                    c.ScavangeMinutes = 60;
                    c.MaxCount = 1000000;
                    c.MaxSize = 100 * 1024 * 1024;
                    return c;
                throw new Exception("Unable to find MemoryCachedClient instance \"" + name + "\".");
            }
        }

        /// <summary>
        /// 获取所有实例名
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAllInstanceName()
        {
            return instances.Keys.ToList();
        }
    }
}
