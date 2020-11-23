using System;
using System.Collections.Generic;
using System.Text;

namespace CommonScheme.NetCore.SysAuthority
{
    public class BaseEntity
    {
        public string Remake { get; set; }
        public long CreateUserID { get; set; }
        public long UpdateUserID { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public int DataState { get; set; }
        public BaseEntity():base()
        {
            UpdateUserID = CreateUserID = 0;
            UpdateTime = CreateTime = DateTime.Now;
            DataState = 1;            
        }
    }
}
