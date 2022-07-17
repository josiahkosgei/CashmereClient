// DenominationResult


namespace Cashmere.Library.Standard.Statuses
{
  public class DenominationResult : StandardResult
  {
    public Denomination data { get; set; }

    public bool NoteJamDetected { get; set; }

    public bool NotesRejected { get; set; }

    public override string ToString() => string.Format("NoteJamDetected={0}\tNotesRejected={1}\tData={2}", (object) this.NoteJamDetected, (object) this.NotesRejected, (object) this.data.ToString());
  }
}
