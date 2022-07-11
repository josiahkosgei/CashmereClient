
// Type: CashmereDeposit.UptimeComponentState

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


namespace Cashmere.Library.CashmereDataAccess.Entities
{
  public class UptimeComponentState
  {
    public Guid Id { get; set; }

    public Guid Device { get; set; }

    public DateTime Created { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int ComponentState { get; set; }
  }
}
