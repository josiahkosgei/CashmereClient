
// Type: CashmereDeposit.Models.CoreBankingAccountVerifyResult

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


using System;

namespace CashmereDeposit.Models
{
  public class CoreBankingAccountVerifyResult
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
    public string AccountNumber;
    public string AccountName;
    public bool isSuccess;
    public string StatusCode;
    public string StatusMessage;
    public int ErrorCode;
    public string ErrorMessage;
    public string ErrorType;
  }
}
