// PCInformation


using System;
using System.Runtime.InteropServices;

namespace Cashmere.Library.Standard.Statuses
{
    public static class PCInformation
    {
        [DllImport("Netapi32.dll", CharSet = CharSet.Unicode)]
        private static extern int NetStatisticsGet(
          [MarshalAs(UnmanagedType.LPWStr)] string serverName,
          [MarshalAs(UnmanagedType.LPWStr)] string service,
          int level,
          int options,
          out IntPtr BufPtr);

        public static DateTime? LastBootTime()
        {
            try
            {
                IntPtr BufPtr = IntPtr.Zero;
                int num = PCInformation.NetStatisticsGet((string)null, "LanmanWorkstation", 0, 0, out BufPtr);
                PCInformation.STAT_WORKSTATION_0 statWorkstation0 = new PCInformation.STAT_WORKSTATION_0();
                if (num == 0)
                    statWorkstation0 = (PCInformation.STAT_WORKSTATION_0)Marshal.PtrToStructure(BufPtr, typeof(PCInformation.STAT_WORKSTATION_0));
                return new DateTime?(DateTime.FromFileTime(statWorkstation0.StatisticsStartTime));
            }
            catch (Exception ex)
            {
                return new DateTime?();
            }
        }

        private struct STAT_WORKSTATION_0
        {
            [MarshalAs(UnmanagedType.I8)]
            public long StatisticsStartTime;
            public long BytesReceived;
            public long SmbsReceived;
            public long PagingReadBytesRequested;
            public long NonPagingReadBytesRequested;
            public long CacheReadBytesRequested;
            public long NetworkReadBytesRequested;
            public long BytesTransmitted;
            public long SmbsTransmitted;
            public long PagingWriteBytesRequested;
            public long NonPagingWriteBytesRequested;
            public long CacheWriteBytesRequested;
            public long NetworkWriteBytesRequested;
            public int InitiallyFailedOperations;
            public int FailedCompletionOperations;
            public int ReadOperations;
            public int RandomReadOperations;
            public int ReadSmbs;
            public int LargeReadSmbs;
            public int SmallReadSmbs;
            public int WriteOperations;
            public int RandomWriteOperations;
            public int WriteSmbs;
            public int LargeWriteSmbs;
            public int SmallWriteSmbs;
            public int RawReadsDenied;
            public int RawWritesDenied;
            public int NetworkErrors;
            public int Sessions;
            public int FailedSessions;
            public int Reconnects;
            public int CoreConnects;
            public int Lanman20Connects;
            public int Lanman21Connects;
            public int LanmanNtConnects;
            public int ServerDisconnects;
            public int HungSessions;
            public int UseCount;
            public int FailedUseCount;
            public int CurrentCommands;
        }
    }
}
