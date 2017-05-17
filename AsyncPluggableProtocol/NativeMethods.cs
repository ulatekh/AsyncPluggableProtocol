using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPluggableProtocol
{
    public class NativeMethods : INativeMethods
    {
        public int CoInternetGetSession(int dwSessionMode, out IInternetSession ppIInternetSession, int dwReserved)
        {
            return CoInternetGetSessionNative(dwSessionMode, out ppIInternetSession, dwReserved);
        }

        [DllImport("urlmon.dll", EntryPoint = "CoInternetGetSession")]
        internal static extern int CoInternetGetSessionNative(int dwSessionMode, out IInternetSession ppIInternetSession, int dwReserved);
    }
}
