using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Users.UserGifts
{
    [Serializable]
    public class UserGiftPayedEvent:DomainEvent<Guid>
    {
        public Guid UserGiftId { get; private set; }

        public UserGiftPayedEvent() { }
        public UserGiftPayedEvent(Guid userGiftId)
        {
            UserGiftId = userGiftId;
        }
    }
}
