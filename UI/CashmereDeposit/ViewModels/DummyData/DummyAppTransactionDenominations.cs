
// Type: CashmereDeposit.ViewModels.DummyData.DummyAppTransactionDenominations

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


using Cashmere.Library.Standard.Statuses;
using System.Collections.Generic;

namespace CashmereDeposit.ViewModels.DummyData
{
  public class DummyAppTransactionDenominations
  {
    public int TotalAmountCents { get; internal set; } = 37000000;

    public Denomination TotalDenominationResult { get; internal set; } = new()
    {
      denominationItems = new List<DenominationItem>()
      {
        new()
        {
          count = 200L,
          denominationValue = 10000,
          Currency = "KES",
          type = DenominationItemType.NOTE
        },
        new()
        {
          count = 200L,
          denominationValue = 20000,
          Currency = "KES",
          type = DenominationItemType.NOTE
        },
        new()
        {
          count = 200L,
          denominationValue = 40000,
          Currency = "KES",
          type = DenominationItemType.NOTE
        },
        new()
        {
          count = 200L,
          denominationValue = 100000,
          Currency = "KES",
          type = DenominationItemType.NOTE
        },
        new()
        {
          count = 200L,
          denominationValue = 200000,
          Currency = "KES",
          type = DenominationItemType.NOTE
        }
      }
    };

    public int CountedAmountCents { get; internal set; } = 185000;

    public Denomination CountedDenominationResult { get; internal set; } = new()
    {
      denominationItems = new List<DenominationItem>()
      {
        new()
        {
          count = 100L,
          denominationValue = 5000,
          Currency = "KES",
          type = DenominationItemType.NOTE
        },
        new()
        {
          count = 100L,
          denominationValue = 10000,
          Currency = "KES",
          type = DenominationItemType.NOTE
        },
        new()
        {
          count = 100L,
          denominationValue = 20000,
          Currency = "KES",
          type = DenominationItemType.NOTE
        },
        new()
        {
          count = 100L,
          denominationValue = 50000,
          Currency = "KES",
          type = DenominationItemType.NOTE
        },
        new()
        {
          count = 100L,
          denominationValue = 100000,
          Currency = "KES",
          type = DenominationItemType.NOTE
        }
      }
    };

    public int DroppedAmountCentsResult { get; internal set; } = 185000;

    public Denomination DroppedDenomination { get; internal set; } = new()
    {
      denominationItems = new List<DenominationItem>()
      {
        new()
        {
          count = 100L,
          denominationValue = 5000,
          Currency = "KES",
          type = DenominationItemType.NOTE
        },
        new()
        {
          count = 100L,
          denominationValue = 10000,
          Currency = "KES",
          type = DenominationItemType.NOTE
        },
        new()
        {
          count = 100L,
          denominationValue = 20000,
          Currency = "KES",
          type = DenominationItemType.NOTE
        },
        new()
        {
          count = 100L,
          denominationValue = 50000,
          Currency = "KES",
          type = DenominationItemType.NOTE
        },
        new()
        {
          count = 100L,
          denominationValue = 100000,
          Currency = "KES",
          type = DenominationItemType.NOTE
        }
      }
    };
  }
}
