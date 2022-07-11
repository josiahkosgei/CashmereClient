
// Type: CashmereDeposit.TransactionLimitListItem

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


using System.ComponentModel.DataAnnotations.Schema;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
  public class TransactionLimitListItem
  {
    public Guid Id { get; set; }

    public Guid TransactionLimitListId { get; set; }

    public string CurrencyCode { get; set; }

    public bool ShowFundsSource { get; set; }

    public Guid? ShowFundsForm { get; set; }

    public long FundsSourceAmount { get; set; }

    public bool PreventOverdeposit { get; set; }

    public long OverdepositAmount { get; set; }

    public bool PreventUnderdeposit { get; set; }

    public long UnderdepositAmount { get; set; }

    public bool PreventOvercount { get; set; }

    public int OvercountAmount { get; set; }
    [ForeignKey(nameof(CurrencyCode))]
    public virtual Currency Currency { get; set; }
    [ForeignKey(nameof(TransactionLimitListId))]
    public virtual TransactionLimitList TransactionLimitList { get; set; }
  }
}
