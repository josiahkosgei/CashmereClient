
//DE50StatusChangedResult


using System;

namespace CashAccSysDeviceManager
{
    public class DE50StatusChangedResult : EventArgs
    {
        public string Status { get; set; }

        public DE50StatusChangedResult(string status) => Status = status;
    }
}
