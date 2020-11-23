using System;
using System.Collections.Generic;
using System.Text;

namespace CommonScheme.ConfigCore.Models
{
    public class ConfigEntity
    {
        public int ID { get; set; }
        public int ParentID { get; set; }
        public string Code { get; set; }
        public string Data { get; set; }
        public int DataStatus { get; set; }
    }
}
