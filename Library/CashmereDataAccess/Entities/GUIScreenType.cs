
// Type: CashmereDeposit.GUIScreenType


namespace Cashmere.Library.CashmereDataAccess.Entities
{
  public class GuiScreenType
  {
    public GuiScreenType()
    {
        GuiScreens = new HashSet<GuiScreen>();
    }

    public int Id { get; set; }

    public Guid Code { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public bool Enabled { get; set; }

    public virtual ICollection<GuiScreen> GuiScreens { get; set; }
  }
}
