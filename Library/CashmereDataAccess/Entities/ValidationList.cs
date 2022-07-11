
// Type: CashmereDeposit.ValidationList

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


namespace Cashmere.Library.CashmereDataAccess.Entities
{
  public class ValidationList
  {
    public ValidationList()
    {
      ValidationListValidationItems = new HashSet<ValidationListValidationItem>();
      GuiScreenListScreens = new HashSet<GuiScreenListScreen>();
    }

    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string Category { get; set; }

    public bool Enabled { get; set; }

    public virtual ICollection<ValidationListValidationItem> ValidationListValidationItems { get; set; }

    public virtual ICollection<GuiScreenListScreen> GuiScreenListScreens { get; set; }
  }
}
