using System;
using System.Collections.Generic;
using System.Text;

namespace CommonScheme.ConfigCore.Models
{
    /// <summary>
    /// 应用、模块
    /// </summary>
    public class AppItemModel : BaseModel
    {
        public int ID { get; set; }
        /// <summary>
        /// 0:应用,1：模块
        /// </summary>
        public int ParentID { get; set; }
        public string Code { get; set; }
        /// <summary>
        /// 解释
        /// </summary>
        public string Explain { get; set; }
    }
}
