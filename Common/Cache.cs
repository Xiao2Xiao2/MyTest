using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;

namespace Common
{
    /// <summary> 
    /// 缓存控制类 
    /// </summary> 
    public static class Cache
    {
        public static List<string> AllUseCacheKey = new List<string>();
        private static ObjectCache oCache = MemoryCache.Default;
        /// <summary> 
        /// 添加缓存 
        /// </summary> 
        /// <param name="key"></param> 
        /// <param name="value"></param> 
        /// <param name="absoluteExpiration"></param> 
        public static void AddCache(string key, object value, DateTimeOffset absoluteExpiration)
        {
            if (AllUseCacheKey.Contains(key))
            {
                RemoveCache(key);
            }
            AllUseCacheKey.Add(key);
            oCache.Add(key, value, absoluteExpiration);
        }
        /// <summary> 
        /// 移除缓存 
        /// </summary> 
        /// <param name="key"></param> 
        public static void RemoveCache(string key)
        {
            if (AllUseCacheKey.Contains(key))
            {
                AllUseCacheKey.Remove(key);
            }
            oCache.Remove(key);
            //HttpContext.Current.Cache.Remove(key);
        }

        /// <summary> 
        /// 移除缓存 
        /// </summary> 
        /// <param name="key"></param> 
        public static object ReadCache(string key)
        {
            if (AllUseCacheKey.Contains(key))
            {
               return oCache.Get(key);
            }
            return null;
            //HttpContext.Current.Cache.Remove(key);
        }

        /// <summary> 
        /// 清空使用的缓存 
        /// </summary> 
        public static void ClearCache()
        {
            foreach (string value in AllUseCacheKey)
            {
                oCache.Remove(value);
            }
            AllUseCacheKey.Clear();
        }
    }
}
