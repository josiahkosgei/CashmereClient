using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    /// <summary>
    /// Current State of the device
    /// </summary>
    [Table("DeviceStatus")]
    [Index(nameof(DeviceId), Name = "idevice_id_DeviceStatus")]
    public partial class DeviceStatus
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("device_id")]
        public Guid DeviceId { get; set; }
        [Required]
        [Column("controller_state")]
        [StringLength(20)]
        public string ControllerState { get; set; }
        [Required]
        [Column("ba_type")]
        [StringLength(20)]
        public string BaType { get; set; }
        [Required]
        [Column("ba_status")]
        [StringLength(20)]
        public string BaStatus { get; set; }
        [Required]
        [Column("ba_currency")]
        [StringLength(3)]
        [Unicode(false)]
        public string BaCurrency { get; set; }
        [Required]
        [Column("bag_number")]
        [StringLength(50)]
        public string BagNumber { get; set; }
        [Required]
        [Column("bag_status")]
        [StringLength(20)]
        public string BagStatus { get; set; }
        [Column("bag_note_level")]
        public int BagNoteLevel { get; set; }
        [Required]
        [Column("bag_note_capacity")]
        [StringLength(10)]
        public string BagNoteCapacity { get; set; }
        [Column("bag_value_level")]
        public long? BagValueLevel { get; set; }
        [Column("bag_value_capacity")]
        public long? BagValueCapacity { get; set; }
        [Column("bag_percent_full")]
        public int BagPercentFull { get; set; }
        [Required]
        [Column("sensors_type")]
        [StringLength(20)]
        public string SensorsType { get; set; }
        [Required]
        [Column("sensors_status")]
        [StringLength(20)]
        public string SensorsStatus { get; set; }
        [Column("sensors_value")]
        public int SensorsValue { get; set; }
        [Required]
        [Column("sensors_door")]
        [StringLength(20)]
        public string SensorsDoor { get; set; }
        [Required]
        [Column("sensors_bag")]
        [StringLength(20)]
        public string SensorsBag { get; set; }
        [Required]
        [Column("escrow_type")]
        [StringLength(20)]
        public string EscrowType { get; set; }
        [Required]
        [Column("escrow_status")]
        [StringLength(20)]
        public string EscrowStatus { get; set; }
        [Required]
        [Column("escrow_position")]
        [StringLength(20)]
        public string EscrowPosition { get; set; }
        [Column("transaction_status")]
        [StringLength(20)]
        public string TransactionStatus { get; set; }
        [Column("transaction_type")]
        [StringLength(20)]
        public string TransactionType { get; set; }
        [Column("machine_datetime")]
        public DateTime? MachineDatetime { get; set; }
        [Column("current_status")]
        public int CurrentStatus { get; set; }
        [Column("modified")]
        public DateTime? Modified { get; set; }
        [Required]
        [Column("machine_name")]
        [StringLength(50)]
        public string MachineName { get; set; }
    }
}
