using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Wallets
{
    [Serializable]
    public class TransferEvent:DomainEvent<Guid>
    {
        public Guid WalletId { get; private set; }
        public string Number { get; private set; }

        public TransferEvent() { }
        public TransferEvent(Guid walletId,string number)
        {
            WalletId = walletId;
            Number = number;
        }
    }
}
