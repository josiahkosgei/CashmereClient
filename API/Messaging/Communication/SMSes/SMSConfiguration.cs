﻿
// Type: Cashmere.API.Messaging.Communication.SMSes.SMSConfiguration
// Assembly: Cashmere.API.Messaging.Communication, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F45642AD-C4B4-4961-9A77-6FFE525EEEC0
// Assembly location: C:\DEV\maniwa\Coop\Coop\CashmereDeposit\App\UI\6.0\Cashmere.API.Messaging.Communication.dll

namespace Cashmere.API.Messaging.Communication.SMSes
{
  public class SMSConfiguration : ISMSConfiguration
  {
    public string SMSHost { get; set; }

    public bool UseSSL { get; set; }
  }
}
