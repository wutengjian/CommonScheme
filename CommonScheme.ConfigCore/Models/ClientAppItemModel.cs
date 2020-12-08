using System;
using System.Collections.Generic;
using System.Text;

namespace CommonScheme.ConfigCore.Models
{
    /// <summary>
    /// 客户端拥有的模块
    /// </summary>
    public class ClientAppItemModel : BaseModel
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int ClientID { get; set; }
        /// <summary>
        /// 模块ID
        /// </summary>
        public int AppItemID { get; set; }
        /// <summary>
        /// 顺序，后面的覆盖前面的数据
        /// </summary>
        public int OrderIndex { get; set; }
    }
}
