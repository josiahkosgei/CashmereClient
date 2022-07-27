// ControllerStatusResult


namespace Cashmere.Library.Standard.Statuses
{
    public class ControllerStatusResult : StandardResult
    {
        public ControllerStatus data { get; set; }

        public string RawData { get; set; }
    }
}
