using System;
using System.Collections.Generic;
using System.Text;
using CommonScheme.NetCore.SysAuthority.Entitys;

namespace CommonScheme.NetCore.SysAuthority.ViewModels
{
    public class SysUserInfo
    {
        public SysUserBase UserInfo { get; set; }
        public int[] RoleIDs { get; set; }
        public bool IsOwn { get; set; }
    }
}
