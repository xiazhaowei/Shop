using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Users
{
    [Serializable]
    public class UserFreezeEvent : DomainEvent<Guid>
    {
        public Guid WalletId { get; private set; }

        public UserFreezeEvent() { }
        public UserFreezeEvent(Guid walletId)
        {
            WalletId = walletId;
        }
    }
}
