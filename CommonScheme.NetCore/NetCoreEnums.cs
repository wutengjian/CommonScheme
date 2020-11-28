using System;
using System.Collections.Generic;
using System.Text;

namespace CommonScheme.NetCore
{
    /// <summary>
    /// 日志存储形式
    /// </summary>
    public enum StorageType { Txt, Sqlite, MongoDB, SqlServer, Mysql }
    public enum LogLevel { Error, Warn, Info, Debug, Track }
}
