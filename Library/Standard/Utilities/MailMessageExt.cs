
//MailMessageExt


using System;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Reflection;

namespace Cashmere.Library.Standard.Utilities
{
  public static class MailMessageExt
  {
    public static void Save(this MailMessage Message, string FileName)
    {
        using FileStream fileStream = new FileStream(FileName, FileMode.Create);
        object obj = typeof (SmtpClient).Assembly.GetType("System.Net.Mail.MailWriter").GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[1]
        {
            typeof (Stream)
        }, null).Invoke(new object[1]
        {
            fileStream
        });
        typeof (MailMessage).GetMethod("Send", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(Message, BindingFlags.Instance | BindingFlags.NonPublic, null, new object[3]
        {
            obj,
            true,
            true
        }, null);
        obj.GetType().GetMethod("Close", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(obj, BindingFlags.Instance | BindingFlags.NonPublic, null, new object[0], null);
    }
  }
}
