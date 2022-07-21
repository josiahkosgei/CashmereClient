using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Views
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
