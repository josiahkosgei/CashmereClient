namespace DeviceManager
{
  public interface IDeviceManager
  {
    void CountNotes();

    void CountCoins();

    void CountBoth();

    void ResetDevice(bool openEscrow = false);

    void SetCurrency(string currency);

    void Connect();

    void Disconnect();

    void CashInStart();
  }
}
