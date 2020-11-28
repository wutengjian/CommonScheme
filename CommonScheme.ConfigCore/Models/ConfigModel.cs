using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CommonScheme.ConfigCore.Models
{
    public class ConfigModel : BaseModel
    {
        public int ID { get; set; }
        public int ParentID { get; set; }
        public string Code { get; set; }
        /// <summary>
        /// 解释
        /// </summary>
        public string Explain { get; set; }
        public string Data { get; set; }
    }
}
