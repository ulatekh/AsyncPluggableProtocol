using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPluggableProtocol
{
    public interface INativeMethods
    {
        int CoInternetGetSession(int dwSessionMode, out IInternetSession ppIInternetSession, int dwReserved);
    }
}
