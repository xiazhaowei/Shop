using Shop.Common.Enums;
using Shop.Domain.Models.Wallets.BenevolenceTransfers;
using System;

namespace Shop.Domain.Events.Wallets.BenevolenceTransfers
{
    [Serializable]
    public class BenevolenceTransferCreatedEvent: TransferEvent
    {
        public BenevolenceTransferInfo Info { get; private set; }
        public BenevolenceTransferType Type { get; private set; }
        public BenevolenceTransferStatus Status { get; private set; }

        public BenevolenceTransferCreatedEvent() { }
        public BenevolenceTransferCreatedEvent(Guid walletId,string number, BenevolenceTransferInfo info, BenevolenceTransferType type, BenevolenceTransferStatus status):base(walletId,number)
        {
            Info = info;
            Type = type;
            Status = status;
        }
    }
}
