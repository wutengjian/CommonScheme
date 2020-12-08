using System;
using System.Collections.Generic;
using System.Text;

namespace CommonScheme.ConfigCore.Models
{
    /// <summary>
    /// 客户端实际要的配置
    /// </summary>
    public class ClientConfigModel
    {
        /// <summary>
        /// 客户端ID
        /// </summary>
        public int ClientID { get; set; }
        /// <summary>
        /// 客户端状态，大于0：启用、小于0：关闭
        /// </summary>
        public int ClientState { get; set; }
        public int ConfigID { get; set; }
        public int ConfigParentID { get; set; }
        public string ConfigCode { get; set; }
    }
}
