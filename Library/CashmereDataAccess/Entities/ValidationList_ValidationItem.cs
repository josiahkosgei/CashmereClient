
// Type: CashmereDeposit.ValidationList_ValidationItem

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


namespace Cashmere.Library.CashmereDataAccess.Entities
{
  public class ValidationListValidationItem
  {
    public Guid Id { get; set; }

    public Guid ValidationListId { get; set; }

    public Guid ValidationItemId { get; set; }

    public int Order { get; set; }

    public bool Enabled { get; set; }

    public virtual ValidationItem ValidationItem { get; set; }

    public virtual ValidationList ValidationList { get; set; }
  }
}
