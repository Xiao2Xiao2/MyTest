using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Caching;

namespace DHelper.Common
{
    public static class Cache
    {
        public static List<string> AllUseCacheKey = new List<string>();
        private static ObjectCache oCache = MemoryCache.Default;

        public static void AddCache(string key, object value, DateTimeOffset absoluteExpiration)
        {
            if (Cache.AllUseCacheKey.Contains(key))
            {
                Cache.RemoveCache(key);
            }
            Cache.AllUseCacheKey.Add(key);
            Cache.oCache.Add(key, value, absoluteExpiration, null);
        }

        public static void RemoveCache(string key)
        {
            if (Cache.AllUseCacheKey.Contains(key))
            {
                Cache.AllUseCacheKey.Remove(key);
            }
            Cache.oCache.Remove(key, null);
        }

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
