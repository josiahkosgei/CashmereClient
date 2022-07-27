using System;
using Cashmere.API.Messaging.Integration;

namespace Cashmere.API.Messaging.CDM.GUIControl.AccountsLists
{
    public sealed class AccountsListRequest : APIDeviceRequestBase
    {
        private int pageNumber;
        private int pageSize = 10;

        public string Currency { get; set; }

        public int TransactionType { get; set; }

        public int PageNumber
        {
            get => pageNumber;
            set => pageNumber = Math.Max(0, value);
        }

        public int PageSize
        {
            get => pageSize;
            set => pageSize = Math.Max(1, value);
        }

        public string SearchText { get; set; }
    }
}
