using ENode.Eventing;
using System;
using Shop.Domain.Models.Stores;

namespace Shop.Domain.Events.Stores
{
    [Serializable]
    public class StoreCreatedEvent :DomainEvent<Guid>
    {
        public StoreInfo Info { get; private set; }
        public SubjectInfo SubjectInfo { get; private set; }
        public Guid UserId { get; private set; }

        public StoreCreatedEvent() { }
        public StoreCreatedEvent(Guid userId,StoreInfo info,SubjectInfo subjectInfo)
        {
            UserId = userId;
            Info = info;
            SubjectInfo = subjectInfo;
        }
    }
}
