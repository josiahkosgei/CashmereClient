
// Type: CashmereDeposit.TransactionType

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


namespace Cashmere.Library.CashmereDataAccess.Entities
{
  public class TransactionType
  {
    public TransactionType()
    {
        TransactionTypeListItems = new HashSet<TransactionTypeListItem>();
    }

    public int Id { get; set; }

    public Guid Code { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public bool Enabled { get; set; }

    public virtual ICollection<TransactionTypeListItem> TransactionTypeListItems { get; set; }
  }
}
