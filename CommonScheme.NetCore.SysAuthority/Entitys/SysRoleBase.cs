using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommonScheme.NetCore.SysAuthority.Entitys
{
    [Table("sysRoleInfo")]
    public class SysRoleBase : BaseEntity
    {
        [Key]
        public int RoleID { get; set; }
        public string RoleName { get; set; }
    }
}
