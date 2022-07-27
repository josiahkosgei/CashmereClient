using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DeviceManager
{
    public static class Test
    {
        [DebuggerStepThrough]
        public static void AsyncSafeInvoke(this EventHandler<string> evnt, object sender, string args)
        {
            EventHandler<string> eventHandler1 = evnt;
            if (eventHandler1 == null)
                return;
            Delegate[] invocationList = eventHandler1.GetInvocationList();
            Task.Factory.StartNew(() =>
            {
                foreach (EventHandler<string> eventHandler2 in invocationList)
                    eventHandler2(sender, args);
            });
        }
    }
}
