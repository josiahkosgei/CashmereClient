
//DataTypes.Auth.APIUser


using System;

namespace CashmereUtil.DataTypes.Auth
{
    public class APIUser
    {
        public Guid id { get; set; }

        public string Name { get; set; }

        public bool Enabled { get; set; }

        public Guid AppId { get; set; }

        public string AppKey { get; set; }
    }
}
