using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Users
{
    [Serializable]
    public class UserUnFreezeEvent : DomainEvent<Guid>
    {
        public Guid WalletId { get; private set; }

        public UserUnFreezeEvent() { }
        public UserUnFreezeEvent(Guid walletId)
        {
            WalletId = walletId;
        }
    }
}
