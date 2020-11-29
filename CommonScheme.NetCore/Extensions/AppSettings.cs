using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommonScheme.NetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CommonScheme.NetCore
{
    public class AppSettings
    {
        private static Dictionary<string, IConfigurationSection> appSections;
        public static void UseAppSetting(IConfigurationBuilder builder)
        {
            appSections = new Dictionary<string, IConfigurationSection>();
            IConfiguration configuration = builder.Build();
            AppSettings.SetAppSetting("ConnectionStrings", configuration.GetSection("ConnectionStrings"));
            AppSettings.SetAppSetting("AppConfigs", configuration.GetSection("AppConfigs"));

        }
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
