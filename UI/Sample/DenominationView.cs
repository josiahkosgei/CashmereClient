using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Keyless]
    public partial class DenominationView
    {
        [Column("tx_id")]
        public Guid TxId { get; set; }
        [Column("subtotal")]
        public long? Subtotal { get; set; }
        [Column("50")]
        public long _50 { get; set; }
        [Column("100")]
        public long _100 { get; set; }
        [Column("200")]
        public long _200 { get; set; }
        [Column("500")]
        public long _500 { get; set; }
        [Column("1000")]
        public long _1000 { get; set; }
    }
}
