using System;

namespace CashmereDeposit.Models
{
  public class CoreBankingPostResult
  {
    public string RequestGUID;
    public string ResponseGUID;
    public string ServiceRequestID;
    public string ServiceRequestVersion;
    public string ChannelID;
    public string BankId;
    public string TimeZone;
    public string RequestFullContent;
    public DateTime TransactionDateTime;
    public DateTime MessageDateTime;
    public string TransactionID;
    public bool isSuccess;
    public string StatusCode;
    public string StatusMessage;
    public int ErrorCode;
    public string ErrorMessage;
    public string ErrorType;
  }
}
