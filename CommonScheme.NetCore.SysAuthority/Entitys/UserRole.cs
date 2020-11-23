using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CommonScheme.NetCore.SysAuthority.Entitys
{
    [Table("sysUserRole")]
    public class UserRole : BaseEntity
    {
        [Key]
        public long ID { get; set; }
        public long UserID { get; set; }
        public int RoleID { get; set; }
    }
}
