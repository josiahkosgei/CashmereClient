
// Type: CashmereDeposit.TransactionTypeList

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


namespace Cashmere.Library.CashmereDataAccess.Entities
{
  public class TransactionTypeList
  {
    public TransactionTypeList()
    {
      TransactionTypeListTransactionTypeListItems = new HashSet<TransactionTypeListTransactionTypeListItem>();
      Devices = new HashSet<Device>();
    }

    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public bool Enabled { get; set; }

    public virtual ICollection<TransactionTypeListTransactionTypeListItem> TransactionTypeListTransactionTypeListItems { get; set; }

    public virtual ICollection<Device> Devices { get; set; }
  }
}
