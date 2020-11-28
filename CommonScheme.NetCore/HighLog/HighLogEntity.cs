using System;
using System.Collections.Generic;
using System.Text;

namespace CommonScheme.NetCore.HighLog
{
    public class HighLogEntity
    {
        /// <summary>
        /// 工程模块
        /// </summary>
        public string ModularCode { get; set; }
        /// <summary>
        /// 模块标识(功能key)
        /// </summary>
        public string BlockMark { get; set; }
        public int LogLevel { get; set; }
        public string Context { get; set; }
        public DateTime LogTime { get; set; }
        public DateTime CreateTime { get; set; }
    }
    public class HighLogOption
    {
        public int LogLevel { get; set; }
        public int StorageType { get; set; }
        public string FilePath { get; set; }
        public string Prefix { get; set; }
        public string DBConn { get; set; }
    }
}
