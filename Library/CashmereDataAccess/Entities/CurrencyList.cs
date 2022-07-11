
// Type: CashmereDeposit.CurrencyList


using System.ComponentModel.DataAnnotations;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
  public class CurrencyList
  {
    public CurrencyList()
    {
      CurrencyListCurrencies = new HashSet<CurrencyListCurrency>();
      Devices = new HashSet<Device>();
    }
        
    [Key]
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string DefaultCurrencyId { get; set; }

    public virtual Currency DefaultCurrency { get; set; }

    public virtual ICollection<CurrencyListCurrency> CurrencyListCurrencies { get; set; }

    public virtual ICollection<Device> Devices { get; set; }
  }
}
