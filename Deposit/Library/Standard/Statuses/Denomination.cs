//Denomination

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cashmere.Library.Standard.Statuses
{
  public class Denomination
  {
    public List<DenominationItem> DenominationItems { get; set; }

    public long TotalValue => this.DenominationItems.Sum<DenominationItem>((Func<DenominationItem, long>) (X => (long) X.denominationValue * X.count));

    public long TotalCount => this.DenominationItems.Sum<DenominationItem>((Func<DenominationItem, long>) (X => X.count));

    public Denomination() => this.DenominationItems = new List<DenominationItem>();

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (DenominationItem denominationItem in this.DenominationItems)
        stringBuilder.Append(string.Format("{0}x {1} {2} = {1} {3}>>", (object) denominationItem.count, (object) denominationItem.Currency, (object) denominationItem.DisplayValue, (object) denominationItem.DisplayTotal));
      return stringBuilder.ToString();
    }

    public static Denomination operator +(Denomination left, Denomination right)
    {
      if (left == null && right != null)
        return right;
      if (right == null && left != null)
        return left;
      int? nullable1 = left?.DenominationItems?.Count;
      int? nullable2 = right?.DenominationItems?.Count;
      if (!(nullable1.GetValueOrDefault() == nullable2.GetValueOrDefault() & nullable1.HasValue == nullable2.HasValue))
        return (Denomination) null;
      Denomination denomination = new Denomination();
      for (int index = 0; index < right.DenominationItems.Count; ++index)
      {
        List<DenominationItem> denominationItems = denomination.DenominationItems;
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
          DenominationItem denominationItem2 = left.DenominationItems[index];
          if (denominationItem2 == null)
          {
            nullable3 = new DenominationItemType?();
            nullable4 = nullable3;
          }
          else
            nullable4 = new DenominationItemType?(denominationItem2.type);
        }
        DenominationItemType? nullable5 = nullable4;
        int valueOrDefault1;
        if (!nullable5.HasValue)
        {
          DenominationItemType? nullable6;
          if (right == null)
          {
            nullable3 = new DenominationItemType?();
            nullable6 = nullable3;
          }
          else
          {
            DenominationItem denominationItem3 = right.DenominationItems[index];
            if (denominationItem3 == null)
            {
              nullable3 = new DenominationItemType?();
              nullable6 = nullable3;
            }
            else
              nullable6 = new DenominationItemType?(denominationItem3.type);
          }
          nullable3 = nullable6;
          valueOrDefault1 = (int) nullable3.GetValueOrDefault();
        }
        else
          valueOrDefault1 = (int) nullable5.GetValueOrDefault();
        denominationItem1.type = (DenominationItemType) valueOrDefault1;
        denominationItem1.Currency = left?.DenominationItems[index]?.Currency ?? right?.DenominationItems[index]?.Currency ?? "XER";
        long? nullable7;
        long? nullable8;
        if (left == null)
        {
          nullable7 = new long?();
          nullable8 = nullable7;
        }
        else
        {
          DenominationItem denominationItem4 = left.DenominationItems[index];
          if (denominationItem4 == null)
          {
            nullable7 = new long?();
            nullable8 = nullable7;
          }
          else
            nullable8 = new long?(denominationItem4.count);
        }
        nullable7 = nullable8;
        long valueOrDefault2 = nullable7.GetValueOrDefault();
        long? nullable9;
        if (right == null)
        {
          nullable7 = new long?();
          nullable9 = nullable7;
        }
        else
        {
          DenominationItem denominationItem5 = right.DenominationItems[index];
          if (denominationItem5 == null)
          {
            nullable7 = new long?();
            nullable9 = nullable7;
          }
          else
            nullable9 = new long?(denominationItem5.count);
        }
        nullable7 = nullable9;
        long valueOrDefault3 = nullable7.GetValueOrDefault();
        denominationItem1.count = valueOrDefault2 + valueOrDefault3;
        int? nullable10;
        if (left == null)
        {
          nullable1 = new int?();
          nullable10 = nullable1;
        }
        else
        {
          DenominationItem denominationItem6 = left.DenominationItems[index];
          if (denominationItem6 == null)
          {
            nullable1 = new int?();
            nullable10 = nullable1;
          }
          else
            nullable10 = new int?(denominationItem6.denominationValue);
        }
        nullable2 = nullable10;
        int valueOrDefault4;
        if (!nullable2.HasValue)
        {
          int? nullable11;
          if (right == null)
          {
            nullable1 = new int?();
            nullable11 = nullable1;
          }
          else
          {
            DenominationItem denominationItem7 = right.DenominationItems[index];
            if (denominationItem7 == null)
            {
              nullable1 = new int?();
              nullable11 = nullable1;
            }
            else
              nullable11 = new int?(denominationItem7.denominationValue);
          }
          nullable1 = nullable11;
          valueOrDefault4 = nullable1.GetValueOrDefault();
        }
        else
          valueOrDefault4 = nullable2.GetValueOrDefault();
        denominationItem1.denominationValue = valueOrDefault4;
        denominationItems.Add(denominationItem1);
      }
      return denomination;
    }
  }
}
