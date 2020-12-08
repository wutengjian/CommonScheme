using CommonScheme.ConfigCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonScheme.ConfigCore.ClientServices
{
    public class ConfigSyncMonitor
    {
        public static void Initialization()
        {

        }
        /// <summary>
        /// 增加、修改推送配置项。维护mapConfig、mapClient、mapPush、pushData
        /// </summary>
        /// <param name="entity"></param>
        public static void SetConfig(ConfigEntity entity)
        {

        }
        /// <summary>
        /// 注册客户端。维护mapClient、mapConfig、mapPush
        /// </summary>
        /// <param name="clientID">客户端ID</param>
        /// <param name="configID">配置项ID</param>
        public static void RegisterClient(int clientID, int configID = 0)
        {

        }
        public static void RemoveConfig(int configID) { }
        public static void CancelClient(int clientID) { }
        private static void Monitor()
        {

        }
        private static void PushConfig(object data)
        {

        }
    }
}
