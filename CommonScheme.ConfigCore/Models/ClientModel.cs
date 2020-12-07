using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CommonScheme.ConfigCore.Models
{
    /// <summary>
    /// 客户端信息
    /// </summary>
    public class ClientModel : BaseModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string IPAddress { get; set; }
        /// <summary>
        /// 证书编码
        /// </summary>
        public string CertCode { get; set; }
        /// <summary>
        /// 客户端状态，大于0：启用、小于0：关闭
        /// </summary>
        public override int DataStatus { get; set; }
    }
}
