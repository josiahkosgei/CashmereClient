using Cashmere.Library.Standard.Logging;
using Cashmere.Library.Standard.Utilities;
using Microsoft.Extensions.Configuration;
using System;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace CashAccSysDeviceManager
{
  public class CashAccSysSerialFix : IDisposable, INotifyPropertyChanged
  {
    private static CashAccSysSerialFix instance;
    private BackgroundWorker SerialSendingWorker = new BackgroundWorker();
    private bool quitSendingSerial;
    private bool dE50CassetteFull;
    private bool escrowBillPresent;
    private bool hopperBillPresent;
    private DE50Mode dE50Mode;
    private DE50OperatingState dE50Operation;
    private const string RemoteCancelCommand = "\u00020017\u0003\u0005";
    private const string StoringErrorRecoveryModeCommand = "\u000200213\u00033";
    private SerialPort DE50Port;
    private SerialPort CashAccSysPort;
    private bool IsInitialised;
    private bool AllowClearEscrowJam;
    private string fromDE50;
    private CashmereLogger Log = new CashmereLogger(Assembly.GetExecutingAssembly().GetName().Version.ToString(), "SerialFix", null);
    private CashmereLogger DropLog = new CashmereLogger(Assembly.GetExecutingAssembly().GetName().Version.ToString(), nameof (DropLog), null);
    private bool _canDrop;
    private bool _isStorageJam;
    private string lastPartialFrame;
    private SerialFixState state;

    internal bool DE50CassetteFull
    {
      get => dE50CassetteFull;
      set
      {
        if (dE50CassetteFull == value)
          return;
        DropLog.Info(nameof (CashAccSysSerialFix), "PropertyChanged", "SettingProperty", "Setting {0} from {1} to {2}", new object[3]
        {
           nameof (DE50CassetteFull),
           dE50CassetteFull,
           value
        });
        dE50CassetteFull = value;
        OnPropertyChanged(nameof (DE50CassetteFull));
      }
    }

    public bool EscrowBillPresent
    {
      get => escrowBillPresent;
      set
      {
        if (escrowBillPresent == value)
          return;
        DropLog.Info(nameof (CashAccSysSerialFix), "PropertyChanged", "SettingProperty", "Setting {0} from {1} to {2}", new object[3]
        {
           nameof (EscrowBillPresent),
           escrowBillPresent,
           value
        });
        escrowBillPresent = value;
        OnPropertyChanged(nameof (EscrowBillPresent));
      }
    }

    public bool HopperBillPresent
    {
      get => hopperBillPresent;
      set
      {
        if (hopperBillPresent == value)
          return;
        DropLog.Info(nameof (CashAccSysSerialFix), "PropertyChanged", "SettingProperty", "Setting {0} from {1} to {2}", new object[3]
        {
           nameof (HopperBillPresent),
           hopperBillPresent,
           value
        });
        hopperBillPresent = value;
        OnPropertyChanged(nameof (HopperBillPresent));
      }
    }

    public DE50Mode DE50Mode
    {
      get => dE50Mode;
      set
      {
        if (dE50Mode == value)
          return;
        DropLog.Info(nameof (CashAccSysSerialFix), "PropertyChanged", "SettingProperty", "Setting {0} from {1} to {2}", new object[3]
        {
           nameof (DE50Mode),
           DE50Mode,
           value
        });
        dE50Mode = value;
        OnPropertyChanged(nameof (DE50Mode));
      }
    }

    public DE50OperatingState DE50Operation
    {
      get => dE50Operation;
      set
      {
        if (dE50Operation == value)
          return;
        DropLog.Info(nameof (CashAccSysSerialFix), "PropertyChanged", "SettingProperty", "Setting {0} from {1} to {2}", new object[3]
        {
           nameof (DE50Operation),
           DE50Operation,
           value
        });
        dE50Operation = value;
        OnPropertyChanged(nameof (DE50Operation));
      }
    }

    private bool CanDrop
    {
      get => _canDrop;
      set
      {
        if (_canDrop == value)
          return;
        _canDrop = value;
        Log.Info(GetType().Name, "process message", "processing", "CanDrop = {0};", new object[1]
        {
           _canDrop
        });
      }
    }

    public bool IsStorageJam
    {
      get => _isStorageJam;
      set
      {
        if (_isStorageJam == value)
          return;
        _isStorageJam = value;
        Log.Info(GetType().Name, "process message", "processing", "IsStorageJam = {0};", new object[1]
        {
           _isStorageJam
        });
      }
    }

    public string FromDE50
    {
      get => fromDE50;
      set
      {
        if (string.IsNullOrEmpty(value))
          return;
        fromDE50 = value;
        if (value.Length <= 1)
          return;
        OnDE50StatusChangedEvent(this, new DE50StatusChangedResult(FromDE50));
      }
    }

    internal SerialFixState State
    {
      get => state;
      set
      {
        if (state == value)
          return;
        state = value;
        OnPropertyChanged(nameof (State));
      }
    }

    private CashAccSysSerialFix(string DEVICE_PORT, string CONTROLLER_PORT)
    {
      try
      {
        if (string.IsNullOrWhiteSpace(DEVICE_PORT))
          throw new ArgumentException("'DEVICE_PORT' cannot be null or whitespace", nameof (DEVICE_PORT));
        if (string.IsNullOrWhiteSpace(CONTROLLER_PORT))
          throw new ArgumentException("'CONTROLLER_PORT' cannot be null or whitespace", nameof (CONTROLLER_PORT));
        DE50Port = new SerialPort(DEVICE_PORT, 9600, Parity.Even, 7, StopBits.One);
        CashAccSysPort = new SerialPort(CONTROLLER_PORT, 9600, Parity.Even, 7, StopBits.One);
        DE50Port.Open();
        CashAccSysPort.Open();
        SerialSendingWorker.DoWork += new DoWorkEventHandler(SerialSendingWorker_DoWork);
        SerialSendingWorker.RunWorkerAsync();
      }
      catch (Exception ex)
      {
        Log.Error(GetType().Name, "General Error", nameof (CashAccSysSerialFix), "{0}", new object[1]
        {
           ex.MessageString()
        });
        State = SerialFixState.SERIAL_PORT_ERROR;
      }
    }

    public static CashAccSysSerialFix GetInstance(
      string DEVICE_PORT,
      string CONTROLLER_PORT)
    {
      if (instance == null)
                instance = new CashAccSysSerialFix(DEVICE_PORT, CONTROLLER_PORT);
      return instance;
    }

    public static CashAccSysSerialFix NewCashAccSysSerialFix(
      string DEVICE_PORT,
      string CONTROLLER_PORT)
    {
      if (instance != null)
                instance.Dispose();
      return GetInstance(DEVICE_PORT, CONTROLLER_PORT);
    }

    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
      PropertyChangedEventHandler propertyChanged = PropertyChanged;
      if (propertyChanged == null)
        return;
      propertyChanged(this, new PropertyChangedEventArgs(name));
    }

    private void SerialSendingWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      Log.Trace(GetType().Name, "SerialSendingWorker", "Loop", "SerialSendingWorker_DoWork started", Array.Empty<object>());
      while (!quitSendingSerial)
      {
        try
        {
          Log.Trace(GetType().Name, "timer1_Tick", nameof (SerialSendingWorker_DoWork), "Tick Start", Array.Empty<object>());
          string str1 = CashAccSysPort.ReadExisting();
          Log.Debug(GetType().Name, "process message", "processing", "ATX: {0}", new object[1]
          {
             str1
          });
          fromDE50 = null;
          string str2 = DE50Port.ReadExisting();
          Log.Debug(GetType().Name, "process message", "processing", "ARX: {0}", new object[1]
          {
             str2
          });
          lastPartialFrame += str2;
          Log.Debug(GetType().Name, "latest frame", "processing", "LastPartialFrame {0}", new object[1]
          {
             lastPartialFrame
          });
          if (lastPartialFrame.Length == 1)
          {
            FromDE50 = lastPartialFrame;
            lastPartialFrame = "";
          }
          else
          {
            string lastPartialFrame1 = lastPartialFrame;
            if ((lastPartialFrame1 != null ? (lastPartialFrame1.Contains("\u0002") ? 1 : 0) : 0) != 0)
              lastPartialFrame = new string(lastPartialFrame.Skip<char>(lastPartialFrame.IndexOf("\u0002")).ToArray<char>());
            string lastPartialFrame2 = lastPartialFrame;
            if ((lastPartialFrame2 != null ? (lastPartialFrame2.Contains("\u0003") ? 1 : 0) : 0) != 0)
            {
              int num = lastPartialFrame.LastIndexOf("\u0003") + 1;
              int count;
              if (lastPartialFrame.Length + 1 >= (count = num + 1))
              {
                FromDE50 = new string(lastPartialFrame.Take<char>(count).ToArray<char>());
                lastPartialFrame = new string(lastPartialFrame.Skip<char>(count).ToArray<char>());
              }
            }
          }
          Log.Trace(GetType().Name, "process message", "processing", "if (FromDE50 != null && FromDE50.Length > 1)", Array.Empty<object>());
          if (FromDE50 != null && FromDE50.Length > 1)
          {
            Log.Trace(GetType().Name, "process message", "processing", "else if (FromDE50[4] == 'E')", Array.Empty<object>());
            byte[] bytes = Encoding.ASCII.GetBytes(FromDE50.ToCharArray());
            byte b = bytes[6];
            HopperBillPresent = b.IsBitSet(0);
            EscrowBillPresent = b.IsBitSet(2);
            char ch1 = FromDE50[4];
            DE50Operation = (DE50OperatingState) (bytes[4] - 64);
            char ch2 = FromDE50[FromDE50.Length - 14];
            DE50Mode = (DE50Mode) (bytes[bytes.Length - 14] - 64);
            DE50CassetteFull = bytes[bytes.Length - 13].IsBitSet(2);
            if (FromDE50.IndexOf("\u0003") > 22)
              DropLog.Info("DE50", "Store End", "Result", FromDE50, Array.Empty<object>());
            FromDE50 = FromDE50.Substring(0, FromDE50.IndexOf("\u0003") + 2);
            switch (ch1)
            {
              case 'E':
                CanDrop = false;
                break;
              case 'R':
                IsStorageJam = true;
                if (AllowClearEscrowJam || IsInitialised)
                {
                  switch (ch2)
                  {
                    case '@':
                      DE50Port.Write("\u000200213\u00033");
                      break;
                    case 'B':
                      DE50Port.Write("\u00020017\u0003\u0005");
                      break;
                  }
                }
                else
                  break;
                break;
              default:
                if (!IsInitialised && ch1 == 'H')
                {
                  str1 = "\u00020044001\u00032";
                  Log.Info(GetType().Name, "process message", "processing", "ToDE50 = u00020044001u0003u0032;", Array.Empty<object>());
                  FromDE50 = "\u0015";
                  Log.Info(GetType().Name, "process message", "processing", "FromDE50 = u0015;", Array.Empty<object>());
                  break;
                }
                break;
            }
          }
          Log.Trace(GetType().Name, "process message", "processing", "if (ToDE50 != null)", Array.Empty<object>());
          if (str1 != null && str1.Length > 4)
          {
            Log.Trace(GetType().Name, "process message", "processing", "if (ToDE50.Length > 4 && ToDE50[4] == '4')", Array.Empty<object>());
            if (str1[4] == '4')
            {
              Log.Trace(GetType().Name, "process message", "processing", "if ((!CanDrop) && !IsStorageJam)", Array.Empty<object>());
              if (!CanDrop && !IsStorageJam && IsInitialised)
              {
                str1 = "\u00020015\u0003\a";
                Log.Info(GetType().Name, "process message", "processing", "ToDE50 = u00020015u0003u0007;", Array.Empty<object>());
                FromDE50 = "\u0015";
                Log.Info(GetType().Name, "process message", "processing", "FromDE50 = u0015;", Array.Empty<object>());
              }
            }
            else if (str1[4] == '2')
            {
              Log.Info(GetType().Name, "process message", "processing", "batch request, reset CanDrop: {0}", new object[1]
              {
                 str1
              });
              IsInitialised = true;
              CanDrop = true;
              IsStorageJam = false;
              AllowClearEscrowJam = false;
            }
            else if (str1[4] == '5' && !CanDrop && !AllowClearEscrowJam)
              str1 = null;
          }
          if (!string.IsNullOrEmpty(FromDE50))
          {
            Log.Info(GetType().Name, "process message", "processing", "RX: {0}", new object[1]
            {
               FromDE50
            });
            CashAccSysPort.Write(FromDE50 ?? "");
          }
          if (!string.IsNullOrEmpty(str1))
          {
            Log.Info(GetType().Name, "process message", "processing", "TX: {0}", new object[1]
            {
               str1
            });
            DE50Port.Write(str1 ?? "");
          }
        }
        catch (Exception ex)
        {
          Log.Warning(GetType().Name, "Exception", "Handler", "Exception encountered: {0}>>{1}>>{2}", new object[3]
          {
             ex?.Message,
             ex?.InnerException?.Message,
             ex?.InnerException?.InnerException?.Message
          });
        }
        Log.Debug(GetType().Name, "SerialSendingWorker", "Loop", "while loop completed, sleeping...", Array.Empty<object>());
        Thread.Sleep(100);
      }
    }

    public void Dispose()
    {
      SerialSendingWorker.Dispose();
      SerialSendingWorker = null;
    }

    public void ClearEscrowJam() => AllowClearEscrowJam = true;

    public event EventHandler<DE50StatusChangedResult> DE50StatusChangedEvent;

    private void OnDE50StatusChangedEvent(object sender, DE50StatusChangedResult e)
    {
      if (DE50StatusChangedEvent == null)
        return;
      DE50StatusChangedEvent(this, e);
    }

    public event EventHandler DE50CassetteFullEvent;

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnDE50CassetteFullEvent(object sender)
    {
      if (DE50CassetteFullEvent == null)
        return;
      DE50CassetteFullEvent(this, EventArgs.Empty);
    }
  }
}
