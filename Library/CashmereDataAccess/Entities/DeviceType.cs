
// Type: CashmereDeposit.DeviceType


namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class DeviceType
    {
        public DeviceType()
        {
            Devices = new HashSet<Device>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool NoteIn { get; set; }

        public bool NoteOut { get; set; }

        public bool NoteEscrow { get; set; }

        public bool CoinIn { get; set; }

        public bool CoinOut { get; set; }

        public bool CoinEscrow { get; set; }

        public virtual ICollection<Device> Devices { get; set; }
    }
}
