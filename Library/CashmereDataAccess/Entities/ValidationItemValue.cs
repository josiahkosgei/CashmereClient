
// Type: CashmereDeposit.ValidationItemValue

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


namespace Cashmere.Library.CashmereDataAccess.Entities
{
  public class ValidationItemValue
  {
    public Guid Id { get; set; }

    public Guid ValidationItemId { get; set; }

    public string Value { get; set; }

    public int Order { get; set; }

    public virtual ValidationItem ValidationItem { get; set; }
  }
}
