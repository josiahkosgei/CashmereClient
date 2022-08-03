//DenominationItem

namespace Cashmere.Library.Standard.Statuses
{
  public class DenominationItem
  {
    private string currency;

    public long count { get; set; }

    public int denominationValue { get; set; }

    public DenominationItemType type { get; set; }

    public string Currency
    {
      get => this.currency;
      set
      {
        if ((value != null ? value.Length : 0) == 3 && !int.TryParse(value, out int _))
          this.currency = value;
        else
          this.currency = "KES";
      }
    }

    public long Total => (long) this.denominationValue * this.count;

    public string DisplayValue => ((double) this.denominationValue / 100.0).ToString("N2");

    public string DisplayCount => this.count.ToString("N0");

    public string DisplayTotal => ((double) this.Total / 100.0).ToString("N2");

    public string DisplayDenominationString => string.Format("{0,7} X {1,-3} {2,-13} = {3,20}", (object) this.DisplayCount, (object) this.Currency.Substring(0, 3).ToUpper(), (object) this.DisplayValue, (object) this.DisplayTotal);
  }
}
