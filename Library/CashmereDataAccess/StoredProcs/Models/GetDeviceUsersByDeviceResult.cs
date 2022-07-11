namespace Cashmere.Library.CashmereDataAccess.StoredProcs.Models
{
    public partial class GetDeviceUsersByDeviceResult
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public Guid RoleId { get; set; }
        public string Email { get; set; }
        public bool EmailEnabled { get; set; }
        public string Phone { get; set; }
        public bool PhoneEnabled { get; set; }
        public bool PasswordResetRequired { get; set; }
        public int LoginAttempts { get; set; }
        public int UserGroup { get; set; }
        public bool? DepositorEnabled { get; set; }
        public bool UserDeleted { get; set; }
        public bool? IsActive { get; set; }
        public Guid? ApplicationUserLoginDetail { get; set; }
    }
}
