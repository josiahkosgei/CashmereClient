
// Type: CashmereDeposit.DeviceStatu


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    [Table("DeviceStatus")]
    public class DeviceStatus
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey("Device")]
        public Guid DeviceId { get; set; }

        public string ControllerState { get; set; }

        public string BaType { get; set; }

        public string BaStatus { get; set; }

        public string BaCurrency { get; set; }

        public string BagNumber { get; set; }

        public string BagStatus { get; set; }

        public int BagNoteLevel { get; set; }

        public string BagNoteCapacity { get; set; }

        public long? BagValueLevel { get; set; }

        public long? BagValueCapacity { get; set; }

        public int BagPercentFull { get; set; }

        public string SensorsType { get; set; }

        public string SensorsStatus { get; set; }

        public int SensorsValue { get; set; }

        public string SensorsDoor { get; set; }

        public string SensorsBag { get; set; }

        public string EscrowType { get; set; }

        public string EscrowStatus { get; set; }

        public string EscrowPosition { get; set; }

        public string TransactionStatus { get; set; }

        public string TransactionType { get; set; }

        public DateTime? MachineDatetime { get; set; }

        public int CurrentStatus { get; set; }

        public DateTime? Modified { get; set; }

        public string MachineName { get; set; }
    }
}
