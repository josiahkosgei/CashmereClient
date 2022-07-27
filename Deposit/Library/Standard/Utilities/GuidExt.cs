
//GuidExt


using System;
using System.Runtime.InteropServices;

namespace Cashmere.Library.Standard.Utilities
{
    public static class GuidExt
    {
        [DllImport("rpcrt4.dll", SetLastError = true)]
        private static extern int UuidCreateSequential(out Guid guid);

        public static Guid UuidCreateSequential()
        {
            Guid guid;
            int sequential = UuidCreateSequential(out guid);
            if (sequential != 0)
                throw new ApplicationException("UuidCreateSequential failed: " + sequential);
            return guid;
        }
    }
}
