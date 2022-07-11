
// Type: Cashmere.Library.Standard.Statuses.Denomination


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cashmere.Library.Standard.Statuses
{
  public class Denomination
  {
    public List<DenominationItem> denominationItems { get; set; }

    public long TotalValue => denominationItems.Sum(X => X.denominationValue * X.count);

    public long TotalCount => denominationItems.Sum(X => X.count);

    public Denomination() => denominationItems = new List<DenominationItem>();

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (DenominationItem denominationItem in denominationItems)
        stringBuilder.Append(string.Format("{0}x {1} {2} = {1} {3}>>", denominationItem.count, denominationItem.Currency, denominationItem.DisplayValue, denominationItem.DisplayTotal));
      return stringBuilder.ToString();
    }

    public static Denomination operator +(Denomination left, Denomination right)
    {
      if (left == null && right != null)
        return right;
      if (right == null && left != null)
        return left;
      int? nullable1 = left?.denominationItems?.Count;
      int? nullable2 = right?.denominationItems?.Count;
      if (!(nullable1.GetValueOrDefault() == nullable2.GetValueOrDefault() & nullable1.HasValue == nullable2.HasValue))
        return null;
      Denomination denomination = new Denomination();
      for (int index = 0; index < right.denominationItems.Count; ++index)
      {
        List<DenominationItem> denominationItems = denomination.denominationItems;
        DenominationItem denominationItem1 = new DenominationItem();
        DenominationItemType? nullable3;
        DenominationItemType? nullable4;
        if (left == null)
        {
          nullable3 = new DenominationItemType?();
          nullable4 = nullable3;
        }
        else
        {
          DenominationItem denominationItem2 = left.denominationItems[index];
          if (denominationItem2 == null)
          {
            nullable3 = new DenominationItemType?();
            nullable4 = nullable3;
          }
          else
            nullable4 = new DenominationItemType?(denominationItem2.type);
        }
        DenominationItemType? nullable5 = nullable4;
        int num1;
        if (!nullable5.HasValue)
        {
          nullable3 = right?.denominationItems[index]?.type;
          num1 = nullable3.HasValue ? (int) nullable3.GetValueOrDefault() : 0;
        }
        else
          num1 = (int) nullable5.GetValueOrDefault();
        denominationItem1.type = (DenominationItemType) num1;
        denominationItem1.Currency = left?.denominationItems[index]?.Currency ?? right?.denominationItems[index]?.Currency ?? "XER";
        long? count = left?.denominationItems[index]?.count;
        long num2 = count ?? 0L;
        count = right?.denominationItems[index]?.count;
        long num3 = count ?? 0L;
        denominationItem1.count = num2 + num3;
        int? nullable6;
        if (left == null)
        {
          nullable1 = new int?();
          nullable6 = nullable1;
        }
        else
        {
          DenominationItem denominationItem2 = left.denominationItems[index];
          if (denominationItem2 == null)
          {
            nullable1 = new int?();
            nullable6 = nullable1;
          }
          else
            nullable6 = new int?(denominationItem2.denominationValue);
        }
        nullable2 = nullable6;
        int num4;
        if (!nullable2.HasValue)
        {
          nullable1 = right?.denominationItems[index]?.denominationValue;
          num4 = nullable1 ?? 0;
        }
        else
          num4 = nullable2.GetValueOrDefault();
        denominationItem1.denominationValue = num4;
        denominationItems.Add(denominationItem1);
      }
      return denomination;
    }
  }
}
