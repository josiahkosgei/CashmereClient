
//CashmereDepositApplicationState


using Caliburn.Micro;

namespace CashmereUtil
{
  public class CashmereDepositApplicationState : PropertyChangedBase
  {
    public bool HasPrinterError { get; set; }

    public bool HasDatabaseError { get; set; }

    public bool HasFileSystemError { get; set; }

    public bool HasDeviceError { get; set; }

    public bool HasServerError { get; set; }
  }
}
