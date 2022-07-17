// Denomination


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cashmere.Library.Standard.Statuses
{
    public class Denomination
    {
        public List<DenominationItem> DenominationItems { get; set; }

        public long TotalValue => this.DenominationItems.Sum<DenominationItem>((Func<DenominationItem, long>)(X => (long)X.denominationValue * X.count));

        public long TotalCount => this.DenominationItems.Sum<DenominationItem>((Func<DenominationItem, long>)(X => X.count));

        public Denomination() => this.DenominationItems = new List<DenominationItem>();

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (DenominationItem denominationItem in this.DenominationItems)
                stringBuilder.Append(string.Format("{0}x {1} {2} = {1} {3}>>", (object)denominationItem.count, (object)denominationItem.Currency, (object)denominationItem.DisplayValue, (object)denominationItem.DisplayTotal));
            return stringBuilder.ToString();
        }

        public static Denomination operator +(Denomination left, Denomination right)
        {
            if (left == null && right == null)
                return (Denomination)null;
            int num;
            if (left != null)
            {
                List<DenominationItem> denominationItems = left.DenominationItems;
                if ((denominationItems != null ? (!denominationItems.Any() ? 1 : 0) : 0) == 0)
                {
                    num = 0;
                    goto label_6;
                }
            }
            num = right == null ? 0 : (right.DenominationItems.Count > 0 ? 1 : 0);
        label_6:
            if (num != 0)
                return right;
            if ((right == null || !right.DenominationItems.Any()) && left != null && left.DenominationItems.Count > 0)
                return left;
            int? denominationItemsCount = left?.DenominationItems?.Count;
            int? itemsCount = right?.DenominationItems?.Count;
            if (!(denominationItemsCount == itemsCount & denominationItemsCount.HasValue == itemsCount.HasValue))
                return null as Denomination;
            Denomination denomination = new Denomination();
            for (int index = 0; index < right.DenominationItems.Count; ++index)
            {
                List<DenominationItem> denominationItems = denomination.DenominationItems;
                DenominationItem denominationItem1 = new DenominationItem
                {
                    type = (DenominationItemType)(left?.DenominationItems[index]?.type ?? right?.DenominationItems[index]?.type),
                    Currency = left?.DenominationItems[index]?.Currency ?? right?.DenominationItems[index]?.Currency ?? "XER",
                    count = (long)(left?.DenominationItems[index]?.count + right?.DenominationItems[index]?.count)
                };
                int? nullable3;
                if (left == null)
                {
                    denominationItemsCount = new int?();
                    nullable3 = denominationItemsCount;
                }
                else
                {
                    DenominationItem denominationItem2 = left.DenominationItems[index];
                    if (denominationItem2 == null)
                    {
                        denominationItemsCount = new int?();
                        nullable3 = denominationItemsCount;
                    }
                    else
                        nullable3 = new int?(denominationItem2.denominationValue);
                }
                itemsCount = nullable3;
                int valueOrDefault;
                if (!itemsCount.HasValue)
                {
                    int? nullable4;
                    if (right == null)
                    {
                        denominationItemsCount = new int?();
                        nullable4 = denominationItemsCount;
                    }
                    else
                    {
                        DenominationItem denominationItem3 = right.DenominationItems[index];
                        if (denominationItem3 == null)
                        {
                            denominationItemsCount = new int?();
                            nullable4 = denominationItemsCount;
                        }
                        else
                            nullable4 = new int?(denominationItem3.denominationValue);
                    }
                    denominationItemsCount = nullable4;
                    valueOrDefault = (int)denominationItemsCount;
                }
                else
                    valueOrDefault = (int)itemsCount;
                denominationItem1.denominationValue = valueOrDefault;
                denominationItems.Add(denominationItem1);
            }
            return denomination;
        }
    }
}
