
//ConversionClasses


using CashAccSysDeviceManager.MessageClasses;
using Cashmere.Library.Standard.Statuses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CashAccSysDeviceManager
{
    internal static class ConversionClasses
    {
        internal static Denomination CreateDenomination(this DispensedNotes dispensedNotes)
        {
            Denomination denomination = new Denomination();
            List<DenominationItem> denominationItemList;
            if (dispensedNotes == null)
            {
                denominationItemList = null;
            }
            else
            {
                List<NoteCount> noteCount = dispensedNotes.NoteCount;
                denominationItemList = noteCount != null ? noteCount.Select(x => new DenominationItem()
                {
                    count = x.Count,
                    Currency = x.Currency,
                    type = DenominationItemType.NOTE,
                    denominationValue = x.Denomination
                }).ToList() : null;
            }
            denomination.DenominationItems = denominationItemList;
            return denomination;
        }

        internal static Denomination CreateDenomination(
          this LastDroppedNotes LastDroppedNotes)
        {
            Denomination denomination = new Denomination();
            List<DenominationItem> denominationItemList;
            if (LastDroppedNotes == null)
            {
                denominationItemList = null;
            }
            else
            {
                List<NoteCount> noteCount = LastDroppedNotes.NoteCount;
                denominationItemList = noteCount != null ? noteCount.Select(x => new DenominationItem()
                {
                    count = x.Count,
                    Currency = x.Currency,
                    type = DenominationItemType.NOTE,
                    denominationValue = x.Denomination
                }).ToList() : null;
            }
            denomination.DenominationItems = denominationItemList;
            return denomination;
        }

        internal static Denomination CreateDenomination(
          this RequestedDispenseNotes RequestedDispenseNotes)
        {
            Denomination denomination = new Denomination();
            List<DenominationItem> denominationItemList;
            if (RequestedDispenseNotes == null)
            {
                denominationItemList = null;
            }
            else
            {
                List<NoteCount> noteCount = RequestedDispenseNotes.NoteCount;
                denominationItemList = noteCount != null ? noteCount.Select(x => new DenominationItem()
                {
                    count = x.Count,
                    Currency = x.Currency,
                    type = DenominationItemType.NOTE,
                    denominationValue = x.Denomination
                }).ToList() : null;
            }
            denomination.DenominationItems = denominationItemList;
            return denomination;
        }

        internal static Denomination CreateDenomination(
          this TotalDroppedNotes TotalDroppedNotes)
        {
            Denomination denomination = new Denomination();
            List<DenominationItem> denominationItemList;
            if (TotalDroppedNotes == null)
            {
                denominationItemList = null;
            }
            else
            {
                List<NoteCount> noteCount = TotalDroppedNotes.NoteCount;
                denominationItemList = noteCount != null ? noteCount.Select(x => new DenominationItem()
                {
                    count = x.Count,
                    Currency = x.Currency,
                    type = DenominationItemType.NOTE,
                    denominationValue = x.Denomination
                }).ToList() : null;
            }
            denomination.DenominationItems = denominationItemList;
            return denomination;
        }

        internal static string ToCashAccSysString(this DropMode dropMode)
        {
            switch (dropMode)
            {
                case DropMode.DROP_NOTES:
                    return "DROP:NOTES";
                case DropMode.DROP_COINS:
                    return "DROP:COINS";
                case DropMode.EXCHANGE_NOTES:
                    return "EXCHANGE:NOTES";
                case DropMode.EXCHANGE_COINS:
                    return "EXCHANGE:COINS";
                case DropMode.MULTIDROP_NOTES:
                    return "MULTIDROP:NOTES";
                case DropMode.MULTIDROP_COINS:
                    return "MULTIDROP:COINS";
                case DropMode.PAYMENT_NOTES:
                    return "PAYMENT:NOTES";
                default:
                    throw new InvalidCastException("DropMode " + dropMode.ToString() + " is invalid for CashAccSys Controller");
            }
        }
    }
}
