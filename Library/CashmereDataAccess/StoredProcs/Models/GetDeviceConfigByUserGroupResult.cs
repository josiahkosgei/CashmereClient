namespace Cashmere.Library.CashmereDataAccess.StoredProcs.Models
{
    public partial class GetDeviceConfigByUserGroupResult
    {
        public Guid Id { get; set; }
        public int GroupId { get; set; }
        public string ConfigId { get; set; }
        public string ConfigValue { get; set; }
    }
}
