
// Type: CashmereDeposit.GUIScreenList


namespace Cashmere.Library.CashmereDataAccess.Entities
{
  public class GuiScreenList
  {
    public GuiScreenList()
    {
      GuiScreenListScreens = new HashSet<GuiScreenListScreen>();
      Devices = new HashSet<Device>();
      TransactionTypeListItems = new HashSet<TransactionTypeListItem>();
    }

    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public bool Enabled { get; set; }

    public virtual ICollection<GuiScreenListScreen> GuiScreenListScreens { get; set; }

    public virtual ICollection<Device> Devices { get; set; }

    public virtual ICollection<TransactionTypeListItem> TransactionTypeListItems { get; set; }
  }
}
