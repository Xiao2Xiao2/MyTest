using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Caching;

namespace DHelper.Common
{
    /// <summary>
    /// 缓存操作
    /// </summary>
    public static class Cache
    {
        public static List<string> AllUseCacheKey = new List<string>();
        private static ObjectCache oCache = MemoryCache.Default;
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">名称</param>
        /// <param name="value">值</param>
        /// <param name="absoluteExpiration">过期时间</param>
        public static void AddCache(string key, object value, DateTimeOffset absoluteExpiration)
        {
            if (Cache.AllUseCacheKey.Contains(key))
            {
                Cache.RemoveCache(key);
            }
            Cache.AllUseCacheKey.Add(key);
            Cache.oCache.Add(key, value, absoluteExpiration, null);
        }
        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">名称</param>

        public static void RemoveCache(string key)
        {
            if (Cache.AllUseCacheKey.Contains(key))
            {
                Cache.AllUseCacheKey.Remove(key);
            }
            Cache.oCache.Remove(key, null);
        }
        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <param name="key">名称</param>
        /// <returns></returns>

        public static object ReadCache(string key)
        {
            object result;
            if (Cache.AllUseCacheKey.Contains(key))
            {
                result = Cache.oCache.Get(key, null);
            }
            else
            {
                result = null;
            }
            return result;
        }
        /// <summary>
        /// 清空缓存
        /// </summary>

        public static void ClearCache()
        {
            foreach (string current in Cache.AllUseCacheKey)
            {
                Cache.oCache.Remove(current, null);
            }
            Cache.AllUseCacheKey.Clear();
        }
    }
}
