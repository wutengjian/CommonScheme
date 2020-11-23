using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommonScheme.NetCore
{
    public class HostDataInfo
    {
        private static string urls;
        public static string Urls
        {
            get { return urls; }
            set { urls = value; }
        }
    }
    public class AppSettings
    {
        private static Dictionary<string, IConfigurationSection> appSections = new Dictionary<string, IConfigurationSection>();

        public static string GetAppSeting(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;
            string[] items = name.Split(':');
            int nIndex = items.Length;
            do
            {
                nIndex--;
                if (appSections.ContainsKey(items[nIndex]))
                {
                    return getAppSeting(appSections[items[nIndex]], items[items.Length - 1]);
                }
            } while (nIndex >= 0);
            return null;
        }
        private static string getAppSeting(IConfigurationSection appSection, string name)
        { 
            return appSection.GetSection(name) == null ? "" : appSection.GetSection(name).Value;
        }
        public static void SetAppSetting(string name, IConfigurationSection section)
        {
            if (appSections.ContainsKey(name))
                appSections[name] = section;
            else
                appSections.Add(name, section);
        }
    }
}
