using System.Collections.Generic;
using Cashmere.Library.CashmereDataAccess.Entities;

namespace CashmereDeposit.Models.Forms
{
  public class Validation
  {
    public ValidationType Type { get; set; }

    public List<ValidationValue> Values { get; set; }

    public string ErrorMessage { get; set; }

    public string SuccessMessage { get; set; }
  }
}
