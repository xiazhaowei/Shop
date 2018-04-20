using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Users.UserGifts
{
    public class UserGiftRemarkChangedEvent:DomainEvent<Guid>
    {
        public Guid UserGiftId { get;private set; }
        public string Remark { get; private set; }

        public UserGiftRemarkChangedEvent() { }
        public UserGiftRemarkChangedEvent(Guid userGiftId,string remark)
        {
            UserGiftId = userGiftId;
            Remark = remark;
        }
    }
}
