using System;
using System.Collections.Generic;
using System.Text;

namespace CommonScheme.ConfigCore.Models
{
    /// <summary>
    /// 返回配置给客户端专用模型
    /// </summary>
    public class ConfigEntity
    {
        public int ID { get; set; }
        public int ParentID { get; set; }
        public string Code { get; set; }
        public string Data { get; set; }
        public int DataStatus { get; set; }
    }
}
