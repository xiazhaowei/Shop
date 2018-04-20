using Shop.Common.Enums;
using Shop.Domain.Models.Wallets.ShopCashTransfers;
using System;

namespace Shop.Domain.Events.Wallets.ShopCashTransfers
{
    [Serializable]
    public class ShopCashTransferCreatedEvent: TransferEvent
    {
        public ShopCashTransferInfo Info { get; private set; }
        public ShopCashTransferType Type { get; private set; }
        public ShopCashTransferStatus Status { get; private set; }

        public ShopCashTransferCreatedEvent() { }
        public ShopCashTransferCreatedEvent(Guid walletId,string number,ShopCashTransferInfo info,ShopCashTransferType type,ShopCashTransferStatus status):base(walletId,number)
        {
            Info = info;
            Type = type;
            Status = status;
        }
    }
}
