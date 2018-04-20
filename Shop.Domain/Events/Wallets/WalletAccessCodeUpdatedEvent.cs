using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Wallets
{
    [Serializable]
    public class WalletAccessCodeUpdatedEvent:DomainEvent<Guid>
    {
        public string AccessCode { get; private set; }

        public WalletAccessCodeUpdatedEvent() { }
        public WalletAccessCodeUpdatedEvent(string accessCode)
        {
            AccessCode = accessCode;
        }
    }
}
