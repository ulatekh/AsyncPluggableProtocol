using System;
using System.Runtime.InteropServices;

namespace AsyncPluggableProtocol
{
    public class ProtocolFactory : IClassFactory
    {
        private Func<IProtocol> _factory;

        internal ProtocolFactory(Func<IProtocol> factory)
        {
            this._factory = factory;
        }

        public static void Register(string name, Func<IProtocol> factory)
        {
            string emptyStr = null;

            IInternetSession session = GetSession(new NativeMethods());
            try
            {
                Guid handlerGuid = typeof(Protocol).GUID;
                session.RegisterNameSpace(
                    new ProtocolFactory(factory),
                    ref handlerGuid,
                    name,
                    0,
                    ref emptyStr,
                    0);
            }
            finally
            {
                Marshal.ReleaseComObject(session);
                session = null;
            }
        }

        internal static IInternetSession GetSession(INativeMethods nativeMethods)
        {
            IInternetSession session;
            int res = nativeMethods.CoInternetGetSession(0, out session, 0);

            if (res != NativeConstants.S_OK || session == null)
                throw new InvalidOperationException("CoInternetGetSession failed.");

            return session;
        }

        public void LockServer(bool Lock)
        {
        }

        public int CreateInstance(IntPtr pUnkOuter, ref Guid riid, out IntPtr ppvObject)
        {
            ppvObject = IntPtr.Zero;

            if (pUnkOuter != IntPtr.Zero)
                return NativeConstants.CLASS_E_NOAGGREGATION;

            if (typeof(IInternetProtocol).GUID.Equals(riid)
                || typeof(IInternetProtocolRoot).GUID.Equals(riid)
                || typeof(IInternetProtocolInfo).GUID.Equals(riid))
            {
                object obj = new Protocol(this._factory());
                IntPtr objPtr = Marshal.GetIUnknownForObject(obj);
                IntPtr resultPtr;
                Guid refIid = riid;
                Marshal.QueryInterface(objPtr, ref refIid, out resultPtr);
                ppvObject = resultPtr;
                return NativeConstants.S_OK;
            }

            return NativeConstants.E_NOINTERFACE;
        }
    }
}