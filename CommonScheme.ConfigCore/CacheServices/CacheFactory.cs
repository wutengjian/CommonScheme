using System;
using System.Collections.Generic;
using System.Text;

namespace CommonScheme.ConfigCore.CacheServices
{
    public class CacheFactory
    {
        private static ICacheService _cache;
        public static void SetCache(ICacheService cache) { _cache = cache; }
        public static ICacheService GetInstace() { return _cache; }
        public static string MadePrefix(string prefix, int parentId = 0)
        {
            return prefix;
        }
    }
}
