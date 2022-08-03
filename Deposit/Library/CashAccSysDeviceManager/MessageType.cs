namespace CashAccSysDeviceManager
{
  public enum MessageType
  {
    ACK = 10, // 0x0000000A
    ReqErr = 11, // 0x0000000B
    CancelReset = 12, // 0x0000000C
    AuthoriseReq = 100, // 0x00000064
    AuthoriseResp = 101, // 0x00000065
    StatusReq = 102, // 0x00000066
    StatusReport = 103, // 0x00000067
    PrintReport = 104, // 0x00000068
    ShowController = 105, // 0x00000069
    PrintText = 106, // 0x0000006A
    SelectCurrency = 110, // 0x0000006E
    DropRequest = 120, // 0x00000078
    DropStatus = 121, // 0x00000079
    DropPause = 122, // 0x0000007A
    DropEnd = 123, // 0x0000007B
    DropResult = 124, // 0x0000007C
    BagStatusReq = 130, // 0x00000082
    BagStatusResp = 131, // 0x00000083
    BagRemovalReport = 133, // 0x00000085
    OpenBagRequest = 134, // 0x00000086
    CloseBagRequest = 135, // 0x00000087
    DispenseRequest = 150, // 0x00000096
    DispenseResponse = 151, // 0x00000097
    DispenseStatus = 152, // 0x00000098
    DispenseResult = 153, // 0x00000099
    DispenseRefillCmd = 154, // 0x0000009A
    DispenseRefillSet = 155, // 0x0000009B
    BeginTransaction = 300, // 0x0000012C
    EndTransaction = 301, // 0x0000012D
    TransactionStatusRequest = 302, // 0x0000012E
    TransactionStatusResponse = 303, // 0x0000012F
  }
}
