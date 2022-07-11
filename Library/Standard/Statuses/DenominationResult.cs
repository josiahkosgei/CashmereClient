
// Type: Cashmere.Library.Standard.Statuses.DenominationResult


namespace Cashmere.Library.Standard.Statuses
{
  public class DenominationResult : StandardResult
  {
    public Denomination data { get; set; }

    public bool NoteJamDetected { get; set; }

    public bool NotesRejected { get; set; }

    public override string ToString() => string.Format("NoteJamDetected={0}\tNotesRejected={1}\tData={2}", NoteJamDetected, NotesRejected, data.ToString());
  }
}
