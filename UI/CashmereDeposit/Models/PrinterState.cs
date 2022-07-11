
// Type: CashmereDeposit.Models.PrinterState

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


namespace CashmereDeposit.Models
{
  public class PrinterState
  {
    public bool HasError { get; set; }

    public bool HasPaper { get; set; } = true;

    public bool CoverOpen { get; set; }

    public PrinterErrorType ErrorType { get; set; }

    public int ErrorCode { get; set; }

    public string ErrorMessage { get; set; }
  }
}
