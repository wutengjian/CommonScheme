using System;
using System.Collections.Generic;
using System.Text;

namespace CommonScheme.ConfigCore.OAServices
{
    public class OAFactory
    {
        private static IOAService _oa;
        public static void SetOA(IOAService oa) { _oa = oa; }
        public static IOAService GetInstace() { return _oa; }       
    }
}
