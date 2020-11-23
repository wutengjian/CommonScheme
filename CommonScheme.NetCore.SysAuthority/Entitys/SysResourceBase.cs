using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommonScheme.NetCore.SysAuthority.Entitys
{

    [Table("sysResourceInfo")]
    public class SysResourceBase: BaseEntity
    {
        [Key]
        public int ResourceID { get; set; }
        /// <summary>
        /// 资源类型，1：页面地址，2：api地址，
        /// </summary>
        public int ResourceType { get; set; }
        public string ResourceName { get; set; }
    }
}
