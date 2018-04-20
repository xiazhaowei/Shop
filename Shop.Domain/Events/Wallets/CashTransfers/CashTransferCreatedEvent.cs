using Shop.Common.Enums;
using Shop.Domain.Models.Wallets.CashTransfers;
using System;

namespace Shop.Domain.Events.Wallets.CashTransfers
{
    [Serializable]
    public class CashTransferCreatedEvent: TransferEvent
    {
        public CashTransferInfo Info { get; private set; }
        public CashTransferType Type { get; private set; }
        public CashTransferStatus Status { get; private set; }

        public CashTransferCreatedEvent() { }
        public CashTransferCreatedEvent(Guid walletId,string number,CashTransferInfo info,CashTransferType type,CashTransferStatus status):base(walletId,number)
        {
            Info = info;
            Type = type;
            Status = status;
        }
    }
}
