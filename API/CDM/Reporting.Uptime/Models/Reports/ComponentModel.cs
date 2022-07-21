using System.ComponentModel;
using Cashmere.Library.Standard.Statuses;

namespace Cashmere.API.CDM.Reporting.Uptime.Models.Reports
{
    public class ComponentModel
    {
        [Description("Component")]
        public CashmereDeviceState component_state { get; set; }

        [Description("Axis")]
        public int axis => (int)component_state;

        [Description("Start Date")]
        public DateTime start_date { get; set; }

        [Description("Duration")]
        public TimeSpan duration => end_date - start_date;

        [Description("End Date")]
        public DateTime end_date { get; set; }
    }
}
