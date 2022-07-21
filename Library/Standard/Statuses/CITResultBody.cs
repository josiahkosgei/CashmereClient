// Decompiled with JetBrains decompiler
// Type: Cashmere.Library.Standard.Statuses.CITResultBody


namespace Cashmere.Library.Standard.Statuses
{
    public class CITResultBody
    {
        public string BagNumber { get; set; }

        public string Currency { get; set; }

        public string Name { get; set; }

        public string DateTime { get; set; }

        public int TransactionCount { get; set; }

        public int CurrencyCount { get; set; }

        public long TotalValue { get; set; }

        public string DeviceSerialNumber { get; set; }

        public Denomination denomination { get; set; }
    }
}
