using System;
using System.Collections.Generic;
using System.Text;

namespace CommonScheme.ConfigCore.DBStorages
{
    public class DBFactory
    {
        private static Dictionary<string, object> map { get; set; }
        public static void Factory() { map = new Dictionary<string, object>(); }
        public static void MapModel<T>(string key, T t)
        {
            if (map.ContainsKey(key) == false)
                map.Add(key, null);
            map[key] = t;
        }
        public static T GetModel<T>(string key)
        {
            if (map.ContainsKey(key))
                return (T)map[key];
            return default(T);
        }
    }
}
