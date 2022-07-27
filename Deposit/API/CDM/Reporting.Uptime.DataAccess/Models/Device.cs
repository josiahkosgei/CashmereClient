
using System;

namespace Cashmere.API.CDM.Reporting.UptimeDataAccess.Models
{
    public class Device
    {
        public Guid id { get; set; }

        public string device_number { get; set; }

        public string device_location { get; set; }

        public string name { get; set; }

        public string machine_name { get; set; }

        public Guid branch_id { get; set; }

        public string description { get; set; }

        public int type_id { get; set; }

        public bool enabled { get; set; }

        public int config_group { get; set; }

        public int? user_group { get; set; }

        public int GUIScreen_list { get; set; }

        public int? language_list { get; set; }

        public int currency_list { get; set; }

        public int transaction_type_list { get; set; }

        public int login_cycles { get; set; }

        public int login_attempts { get; set; }

        public string mac_address { get; set; }

        public Guid app_id { get; set; }

        public byte[] app_key { get; set; }
    }
}
