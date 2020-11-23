using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommonScheme.NetCore.SysAuthority.Entitys
{
    [Table("sysUserInfo")]
    public class SysUserBase : BaseEntity
    {
        [Key]
        public long UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
