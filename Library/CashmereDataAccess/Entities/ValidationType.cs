
// Type: CashmereDeposit.ValidationType

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


namespace Cashmere.Library.CashmereDataAccess.Entities
{
  public class ValidationType
  {
    public ValidationType()
    {
        ValidationItems = new HashSet<ValidationItem>();
    }

    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public bool Enabled { get; set; }

    public virtual ICollection<ValidationItem> ValidationItems { get; set; }
  }
}
