
// Type: Cashmere.Library.Standard.Statuses.DenominationItem


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
      get => currency;
      set
      {
        if ((value != null ? value.Length : 0) == 3 && !int.TryParse(value, out int _))
          currency = value;
        else
          currency = "KES";
      }
    }

    public long Total => denominationValue * count;

    public string DisplayValue => (denominationValue / 100.0).ToString("N2");

    public string DisplayCount => count.ToString("N0");

    public string DisplayTotal => (Total / 100.0).ToString("N2");

    public string DisplayDenominationString => string.Format("{0,7} X {1,-3} {2,-13} = {3,20}", DisplayCount, Currency.Substring(0, 3).ToUpper(), DisplayValue, DisplayTotal);
  }
}
