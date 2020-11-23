using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CommonScheme.NetCore.SysAuthority.Entitys
{
    [Table("sysRoleResource")]
    public class RoleResource : BaseEntity
    {
        [Key]
        public long ID { get; set; }
        public int RoleID { get; set; }
        public int ResourceID { get; set; }
    }
}
