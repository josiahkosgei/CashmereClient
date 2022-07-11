
//Reporting.CITReporting.CIT


using CashmereUtil.Reporting.MSExcel;
using System;

namespace CashmereUtil.Reporting.CITReporting
{
  public class CIT
  {
    public string device { get; set; }

    public DateTime cit_date { get; set; }

    public DateTime? cit_complete_date { get; set; }

    public string InitiatingUser { get; set; }

    public string AuthorisingUser { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public string OldBagNumber { get; set; }

    public string NewBagNumber { get; set; }

    public string SealNumber { get; set; }

    public bool Complete { get; set; }

    public string Error { get; set; }

    public string ErrorMessage { get; set; }

    [EpplusIgnore]
    public Guid id { get; set; }

    [EpplusIgnore]
    public Guid device_id { get; set; }
  }
}
