using System;
using System.Collections.Generic;
using System.Text;
using CommonScheme.NetCore.SysAuthority.Entitys;

namespace CommonScheme.NetCore.SysAuthority.ViewModels
{
    public class SysRoleInfo
    {
        public SysRoleBase RoleInfo { get; set; }
        public int[] ResourceIDs { get; set; }
        public bool IsOwn { get; set; }
    }
}
