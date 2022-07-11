
//Reporting.CITReporting.EscrowJam


using CashmereUtil.Reporting.MSExcel;
using System;

namespace CashmereUtil.Reporting.CITReporting
{
  public class EscrowJam
  {
    [EpplusIgnore]
    public Guid id { get; set; }

    [EpplusIgnore]
    public Guid transaction_id { get; set; }

    public DateTime DateDetected { get; set; }

    public DateTime? RecoveryDate { get; set; }

    public Decimal DroppedAmount { get; set; }

    public Decimal EscrowAmount { get; set; }

    public Decimal PostedAmount { get; set; }

    public Decimal RetreivedAmount { get; set; }

    public string InitialisingUser { get; set; }

    public string AuthorisingUser { get; set; }

    public string AdditionalInfo { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public string DeviceReferenceNumber { get; set; }

    public string CB_Reference { get; set; }

    public string AccountNumber { get; set; }

    public override string ToString() => string.Format("id={0}\transaction_id={1}", id, transaction_id) + string.Format("\tStartTime={0:yyyy-MM-dd HH:mm:ss.fff}", EndTime) + string.Format("\tEndTime={0:yyyy-MM-dd HH:mm:ss.fff}", EndTime) + "\tInitialisingUser=" + InitialisingUser + "\tAuthorisingUser=" + AuthorisingUser + "\tDeviceReferenceNumber=" + DeviceReferenceNumber + "\tCB_Reference=" + CB_Reference + "\tAccountNumber=" + AccountNumber + string.Format("\tDateDetected={0:yyyy-MM-dd HH:mm:ss.fff}", DateDetected) + string.Format("\tRecoveryDate={0:yyyy-MM-dd HH:mm:ss.fff}", RecoveryDate) + string.Format("\tAmount={0:#,#0.##}", DroppedAmount) + string.Format("\tAmount={0:#,#0.##}", EscrowAmount) + string.Format("\tAmount={0:#,#0.##}", PostedAmount) + string.Format("\tAmount={0:#,#0.##}", RetreivedAmount) + "\tAdditionalInfo=" + AdditionalInfo;
  }
}
