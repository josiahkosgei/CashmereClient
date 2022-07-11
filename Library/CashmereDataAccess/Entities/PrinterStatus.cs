
// Type: CashmereDeposit.PrinterStatu

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    [Table("PrinterStatus")]
    public class PrinterStatus
    {
        [Key]
        public Guid Id { get; set; }

        public Guid PrinterId { get; set; }

        public bool IsError { get; set; }

        public bool HasPaper { get; set; }

        public bool CoverOpen { get; set; }

        public int ErrorCode { get; set; }

        public string ErrorName { get; set; }

        public string ErrorMessage { get; set; }

        public DateTime Modified { get; set; }

        public string MachineName { get; set; }
    }
}
